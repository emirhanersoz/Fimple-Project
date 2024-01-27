using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Api.Persistence;
using Fi.Patika.Schema.Model;
using Fi.Infra.Context;
using Fi.Infra.Exceptions;
using Fi.Infra.Persistence;
using Fi.Infra.Abstraction;
using Fi.Infra.Schema.Utility;
using Fi.Mediator.Interfaces;
using Fi.Mediator.Message;
using Fi.Persistence.Relational.Context;
using Fi.Persistence.Relational.Domain;
using Fi.Persistence.Relational.Interfaces;
using Fi.Persistence.Relational.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Google.Protobuf;

namespace Fi.Patika.Api.Impl.Command
{
    public class AccountCommandHandler :
        IFiRequestHandler<CreateAccountCommand, AccountOutputModel>,
        IFiRequestHandler<UpdateAccountCommand, AccountOutputModel>,
        IFiRequestHandler<DeleteAccountCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public AccountCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<AccountOutputModel> Handle(CreateAccountCommand message, CancellationToken cancellationToken)
        {
            if(message.Model.Balance <= 0)
            {
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.MustBeGreaterThan, message.Model.Balance, 0);
            }

            sessionDI.ExecutionTrace.InitTrace();

            var entity = mapper.Map<Account>(message.Model);
            
            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<AccountOutputModel>(entity);
        }

        public async Task<AccountOutputModel> Handle(UpdateAccountCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<Account>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Account"], message.Id);

            var mapped = mapper.Map<Account>(message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<AccountOutputModel>(fromDb);
        }

        public async Task<VoidResult> Handle(DeleteAccountCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<Account>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Account"], message.Id);
            
            dbContext.Remove<Account>(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }
    }
}