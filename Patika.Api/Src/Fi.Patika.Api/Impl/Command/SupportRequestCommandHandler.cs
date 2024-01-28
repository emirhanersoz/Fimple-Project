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
    public class SupportRequestCommandHandler :
        IFiRequestHandler<CreateSupportRequestCommand, SupportRequestOutputModel>,
        IFiRequestHandler<UpdateSupportRequestCommand, SupportRequestOutputModel>,
        IFiRequestHandler<DeleteSupportRequestCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public SupportRequestCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<SupportRequestOutputModel> Handle(CreateSupportRequestCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = mapper.MapToNewEntityForNameAndDescriptionTranslation<SupportRequestInputModel, SupportRequest, SupportRequestTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<SupportRequestOutputModel, SupportRequest, SupportRequestTranslation>(sessionDI, entity);
        }

        public async Task<SupportRequestOutputModel> Handle(UpdateSupportRequestCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<SupportRequest>()
                                        .Include(x => x.Translations)
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "SupportRequest"], message.Id);

            fromDb.AnsweredDate = DateTimeHelper.UtcNow;
            fromDb.Answered = message.Model.Answered;
            fromDb.isAnswered = true;
            message.Model.isAnswered = true;

            fromDb.Translations = TranslationHelper.GetTranslationsForNameAndDescription<SupportRequestTranslation>(message.Model, fromDb.Id);
            var mapped = mapper.MapToEntityForNameAndDescriptionTranslation<SupportRequestInputModel, SupportRequest, SupportRequestTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<SupportRequestOutputModel, SupportRequest, SupportRequestTranslation>(sessionDI, fromDb);
        }

        public async Task<VoidResult> Handle(DeleteSupportRequestCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<SupportRequest>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "SupportRequest"], message.Id);
            
            dbContext.Remove<SupportRequest>(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }

    }
}