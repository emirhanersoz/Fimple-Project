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
using Fi.Infra.Utility;

namespace Fi.Patika.Api.Impl.Command
{
    public class PayeeCommandHandler :
        IFiRequestHandler<CreatePayeeCommand, PayeeOutputModel>,
        IFiRequestHandler<UpdatePayeeCommand, PayeeOutputModel>,
        IFiRequestHandler<PaymentPayeeCommand, PayeeOutputModel>,
        IFiRequestHandler<DeletePayeeCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public PayeeCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<PayeeOutputModel> Handle(CreatePayeeCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = mapper.Map<Payee>(message.Model);
            
            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<PayeeOutputModel>(entity);
        }

        public async Task<PayeeOutputModel> Handle(UpdatePayeeCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<Payee>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Payee"], message.Id);

            var mapped = mapper.Map<Payee>(message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<PayeeOutputModel>(fromDb);
        }

        public async Task<PayeeOutputModel> Handle(PaymentPayeeCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDbPayee = await dbContext.Set<Payee>()
                                        .Include(p => p.Account)
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

            if (fromDbPayee == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Payee"], message.Id);

            if (fromDbPayee.Account.Balance < message.Model.Amount)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Payee"], message.Id, fromDbPayee.Account.Balance);

            if (message.Model.isPayment == true)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Payee"], message.Id, message.Model.isPayment);

            fromDbPayee.Account.Balance -= message.Model.Amount;
            message.Model.isPayment = true;

            var mapped = mapper.Map<Payee>(message.Model);

            await dbContext.UpdatePartial(fromDbPayee, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<PayeeOutputModel>(fromDbPayee);
        }

        public async Task<VoidResult> Handle(DeletePayeeCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<Payee>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Payee"], message.Id);
            
            dbContext.Remove<Payee>(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }

    }
}