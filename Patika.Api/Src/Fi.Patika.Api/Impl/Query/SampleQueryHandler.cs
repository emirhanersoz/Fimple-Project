/*using System;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fi.Infra.Context;
using Fi.Mediator.Interfaces;
using Fi.Persistence.Relational.Interfaces;
using Fi.Infra.Exceptions;
using Fi.Infra.Abstraction;
using Fi.Infra.Schema.Const;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Api.Domain.Entity;
using Fi.Patika.Schema.Model;
using Fi.Patika.Api.Persistence;

namespace Fi.Patika.Api.Impl
{
    public class SampleQueryHandler:
        IFiRequestHandler<GetSampleByCodeQuery, List<SampleOutputModel>>,
        IFiRequestHandler<GetSampleByKeyQuery, SampleOutputModel>,
        IFiRequestHandler<GetAllSampleQuery, List<SampleOutputModel>>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiPatikaDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public SampleQueryHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext, IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiPatikaDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<List<SampleOutputModel>> Handle(GetSampleByCodeQuery request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();
            
            var list = await dbContext.Set<Sample>().Where(x => x.Code == request.Code)
                                                    .OrderBy(x => x.Name)
                                                    .ToListAsNoTrackingAsync(sessionDI.MessageContext);

            return mapper.Map<List<SampleOutputModel>>(list);
        }

        public async Task<SampleOutputModel> Handle(GetSampleByKeyQuery request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var result = await dbContext.Set<Sample>()
                                        .FirstOrDefaultAsNoTrackingAsync(x => x.Id == request.Id, cancellationToken);
            if (result == null)
                throw exceptionFactory.BadRequestEx(ErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Sample"], request.Id);

            return mapper.Map<SampleOutputModel>(result);
        }

        public async Task<List<SampleOutputModel>> Handle(GetAllSampleQuery request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var list = await dbContext.Set<Sample>()
                                      .OrderBy(x => x.Name)
                                      .ToListAsNoTrackingAsync(sessionDI.MessageContext);

            return mapper.Map<List<SampleOutputModel>>(list);
        }
    }
}*/