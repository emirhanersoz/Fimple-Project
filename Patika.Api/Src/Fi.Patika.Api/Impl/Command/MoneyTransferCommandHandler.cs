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

namespace Fi.Patika.Api.Impl.Command
{
    public class MoneyTransferCommandHandler :
        IFiRequestHandler<CreateMoneyTransferCommand, MoneyTransferOutputModel>,
        IFiRequestHandler<UpdateMoneyTransferCommand, MoneyTransferOutputModel>,
        IFiRequestHandler<TransferMoneyCommand, MoneyTransferOutputModel>,
        IFiRequestHandler<DeleteMoneyTransferCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        private const decimal totalDailyTransferLimit = 10000;

        public MoneyTransferCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<MoneyTransferOutputModel> Handle(CreateMoneyTransferCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = mapper.Map<MoneyTransfer>(message.Model);

            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<MoneyTransferOutputModel>(entity);
        }

        public async Task<MoneyTransferOutputModel> Handle(UpdateMoneyTransferCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<MoneyTransfer>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Id);

            var mapped = mapper.Map<MoneyTransfer>(message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<MoneyTransferOutputModel>(fromDb);
        }

        public async Task<VoidResult> Handle(DeleteMoneyTransferCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<MoneyTransfer>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Id);

            dbContext.Remove<MoneyTransfer>(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }

        public async Task<MoneyTransferOutputModel> Handle(TransferMoneyCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var fromDbAccount = await dbContext.Set<Account>()
                                                .FirstOrDefaultAsync(x => x.Id == message.Model.AccountId, cancellationToken);

            var fromDbDescAccount = await dbContext.Set<Account>()
                                                    .FirstOrDefaultAsync(x => x.Id == message.Model.DestAccountId, cancellationToken);

            if (fromDbAccount == null || fromDbDescAccount == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Model.AccountId);

            if (fromDbAccount.Balance < message.Model.Amount)
                throw exceptionFactory.BadRequestEx(ErrorCodes.NotEnoughBalance, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Model.AccountId);

            if (fromDbAccount.TotailDailyTransferAmount + message.Model.Amount > totalDailyTransferLimit)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedTransferLimit, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Model.AccountId);

            fromDbAccount.Balance -= message.Model.Amount;
            fromDbAccount.TotailDailyTransferAmount += message.Model.Amount;
            fromDbDescAccount.Balance += message.Model.Amount;

            var entity = mapper.Map<MoneyTransfer>(message.Model);

            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<MoneyTransferOutputModel>(entity);
        }
    }
}