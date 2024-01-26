
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Schema.Model;
using Fi.Patika.Api.Persistence;
using Fi.Infra.Context;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using Fi.Infra.Persistence;
using Fi.Mediator.Interfaces;
using Fi.Mediator.Message;
using Fi.Persistence.Relational.Context;
using Fi.Persistence.Relational.Domain;
using Fi.Persistence.Relational.Interfaces;
using Fi.Persistence.Relational.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Fi.Patika.Api.Impl.Query
{
    public class MoneyTransferQueryHandler:
        IFiRequestHandler<GetMoneyTransferByParametersQuery, List<MoneyTransferOutputModel>>,
        IFiRequestHandler<GetMoneyTransferByKeyQuery, MoneyTransferOutputModel>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public MoneyTransferQueryHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext, 
            IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;  
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }
        
        public async Task<MoneyTransferOutputModel> Handle(GetMoneyTransferByKeyQuery message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entity = await dbContext.Set<MoneyTransfer>()
                                        .FirstOrDefaultAsNoTrackingAsync(x => x.Id == message.Id, cancellationToken);

            if (entity == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "MoneyTransfer"], message.Id);

            return mapper.Map<MoneyTransferOutputModel>(entity);
        }

        public async Task<List<MoneyTransferOutputModel>> Handle(GetMoneyTransferByParametersQuery message, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var entityList = await dbContext.Set<MoneyTransfer>()
                                                .ToListAsNoTrackingAsync(sessionDI.MessageContext);

            return mapper.Map<List<MoneyTransferOutputModel>>(entityList);
        }
    }
}