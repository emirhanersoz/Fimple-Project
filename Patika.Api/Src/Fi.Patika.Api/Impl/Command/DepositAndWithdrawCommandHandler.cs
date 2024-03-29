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
using MediatR;
using Confluent.Kafka;
using Fi.Infra.Schema.Const;
using Microsoft.Identity.Client;

namespace Fi.Patika.Api.Impl.Command
{
    public class DepositAndWithdrawCommandHandler :
        IFiRequestHandler<CreateDepositAndWithdrawCommand, DepositAndWithdrawOutputModel>,
        IFiRequestHandler<UpdateDepositAndWithdrawCommand, DepositAndWithdrawOutputModel>,
        IFiRequestHandler<TransactionWithdrawCommand, DepositAndWithdrawOutputModel>,
        IFiRequestHandler<TransactionDepositCommand, DepositAndWithdrawOutputModel>,
        IFiRequestHandler<DeleteDepositAndWithdrawCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        private const decimal dailySingleTransactionLimit = 35000;
        private const decimal dailyTotalTransactionLimit = 50000;

        public DepositAndWithdrawCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<DepositAndWithdrawOutputModel> Handle(CreateDepositAndWithdrawCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            if (message.Model.Amount < 0)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"]);

            var entity = mapper.MapToNewEntityForNameAndDescriptionTranslation<DepositAndWithdrawInputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<DepositAndWithdrawOutputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI, entity);
        }

        public async Task<DepositAndWithdrawOutputModel> Handle(UpdateDepositAndWithdrawCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<DepositAndWithdraw>()
                                        .Include(x => x.Translations)
                                        .FirstOrDefaultAsync(x => x.Id == message.Model.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            fromDb.Translations = TranslationHelper.GetTranslationsForNameAndDescription<DepositAndWithdrawTranslation>(message.Model, fromDb.Id);
            var mapped = mapper.MapToEntityForNameAndDescriptionTranslation<DepositAndWithdrawInputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<DepositAndWithdrawOutputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI, fromDb);
        }

        public async Task<VoidResult> Handle(DeleteDepositAndWithdrawCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<DepositAndWithdraw>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);
            
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }

        public async Task<DepositAndWithdrawOutputModel> Handle(TransactionDepositCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<DepositAndWithdraw>()
                                        .Include(x => x.Translations)
                                        .Include(p => p.Account)
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount < 0)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount > dailySingleTransactionLimit)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedSinlgleTransferLimit, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount + fromDb.Account.TotailDailyTransferAmount > dailyTotalTransactionLimit)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedTransferLimit, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (fromDb.Account.TotailDailyTransferAmount > message.Model.Amount + fromDb.Account.TotailDailyTransferAmount)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedTransferLimit, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            fromDb.Account.Balance += message.Model.Amount;
            fromDb.Account.TotailDailyTransferAmount += message.Model.Amount;
            message.Model.isSucceded = true;
            message.Model.TransactionType = TransactionType.Deposit;

            fromDb.Account.Balance = Math.Round(fromDb.Account.Balance, ISOCurrencyCodes.TRY.DecimalPlace);
            fromDb.Account.TotailDailyTransferAmount = Math.Round(fromDb.Account.TotailDailyTransferAmount, ISOCurrencyCodes.TRY.DecimalPlace);

            fromDb.Translations = TranslationHelper.GetTranslationsForNameAndDescription<DepositAndWithdrawTranslation>(message.Model, fromDb.Id);
            var mapped = mapper.MapToEntityForNameAndDescriptionTranslation<DepositAndWithdrawInputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<DepositAndWithdrawOutputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI, fromDb);
        }

        public async Task<DepositAndWithdrawOutputModel> Handle(TransactionWithdrawCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<DepositAndWithdraw>()
                            .Include(x => x.Translations)
                            .Include(p => p.Account)
                            .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount < 0)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount > dailySingleTransactionLimit)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedSinlgleTransferLimit, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount + fromDb.Account.TotailDailyTransferAmount > dailyTotalTransactionLimit)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ExceedTransferLimit, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            if (message.Model.Amount > fromDb.Account.Balance)
                throw exceptionFactory.BadRequestEx(ErrorCodes.NotEnoughBalance, localizer[FiLocalizedStringType.EntityName, "DepositAndWithdraw"], message.Id);

            fromDb.Account.Balance -= message.Model.Amount;
            fromDb.Account.TotailDailyTransferAmount += message.Model.Amount;
            message.Model.isSucceded = true;
            message.Model.TransactionType = TransactionType.Withdraw;

            fromDb.Account.Balance = Math.Round(fromDb.Account.Balance, ISOCurrencyCodes.TRY.DecimalPlace);
            fromDb.Account.TotailDailyTransferAmount = Math.Round(fromDb.Account.TotailDailyTransferAmount, ISOCurrencyCodes.TRY.DecimalPlace);

            fromDb.Translations = TranslationHelper.GetTranslationsForNameAndDescription<DepositAndWithdrawTranslation>(message.Model, fromDb.Id);
            var mapped = mapper.MapToEntityForNameAndDescriptionTranslation<DepositAndWithdrawInputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<DepositAndWithdrawOutputModel, DepositAndWithdraw, DepositAndWithdrawTranslation>(sessionDI, fromDb);
        }
    }
}