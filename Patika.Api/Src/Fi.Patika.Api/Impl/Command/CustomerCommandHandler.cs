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
    public class CustomerCommandHandler :
        IFiRequestHandler<CreateCustomerCommand, CustomerOutputModel>,
        IFiRequestHandler<UpdateCustomerCommand, CustomerOutputModel>,
        IFiRequestHandler<DeleteCustomerCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public CustomerCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext,
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<CustomerOutputModel> Handle(CreateCustomerCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = mapper.MapToNewEntityForNameAndDescriptionTranslation<CustomerInputModel, Customer, CustomerTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<CustomerOutputModel, Customer, CustomerTranslation>(sessionDI, entity);
        }

        public async Task<CustomerOutputModel> Handle(UpdateCustomerCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            message.Model.Id = message.Id;

            var fromDb = await dbContext.Set<Customer>()
                                        .Include(x => x.Translations)
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Customer"], message.Id);

            fromDb.Translations = TranslationHelper.GetTranslationsForNameAndDescription<CustomerTranslation>(message.Model, fromDb.Id);
            var mapped = mapper.MapToEntityForNameAndDescriptionTranslation<CustomerInputModel, Customer, CustomerTranslation>(sessionDI.TenantContext.Language.ISOCode, message.Model);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.MapToModelForNameAndDescriptionTranslation<CustomerOutputModel, Customer, CustomerTranslation>(sessionDI, fromDb);
        }

        public async Task<VoidResult> Handle(DeleteCustomerCommand message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<Customer>()
                                        .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);
            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Customer"], message.Id);
            
            dbContext.Remove<Customer>(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new VoidResult();
        }
    }
}