using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fi.Template.Api.Cqrs;
using Fi.Template.Api.Domain.Entity;
using Fi.TemplateUniqueName.Schema.Model;
using Fi.Template.Api.Persistence;
using Fi.Infra.Context;
using Fi.Infra.Exceptions;
using Fi.Infra.Schema.Const;
using Fi.Infra.Abstraction;
using Fi.Mediator.Interfaces;
using Fi.Mediator.Message;
using Fi.Persistence.Relational.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Fi.Template.Api.Impl
{
    public class SampleCommandHandler :
        IFiRequestHandler<CreateSampleCommand, SampleOutputModel>,
        IFiRequestHandler<UpdateSampleCommand, SampleOutputModel>,
        IFiRequestHandler<DeleteSampleCommand, VoidResult>
    {
        private readonly ISessionContextDI sessionDI;
        private readonly FiTemplateUniqueNameDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IExceptionFactory exceptionFactory;
        private readonly IJsonStringLocalizer localizer;

        public SampleCommandHandler(ISessionContextDI sessionDI, IFiModuleDbContext dbContext, IMapper mapper, IExceptionFactory exceptionFactory, IJsonStringLocalizer localizer)
        {
            this.sessionDI = sessionDI;
            this.dbContext = dbContext as FiTemplateUniqueNameDbContext;
            this.mapper = mapper;
            this.exceptionFactory = exceptionFactory;
            this.localizer = localizer;
        }

        public async Task<SampleOutputModel> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var existedSample = await dbContext.Set<Sample>().FirstOrDefaultAsNoTrackingAsync(x => x.Code == request.Model.Code, cancellationToken);
            if (existedSample != null)
                throw exceptionFactory.BadRequestEx(ErrorCodes.SampleAlreadyExists, localizer[FiLocalizedStringType.EntityName, "Sample"], existedSample.Id);

            var entity = mapper.Map<Sample>(request.Model);

            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SampleOutputModel>(entity);
        }

        public async Task<SampleOutputModel> Handle(UpdateSampleCommand request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();
            
            request.Model.Id = request.Id;
            var mapped = mapper.Map<Sample>(request.Model);

            var fromDb = await dbContext.Set<Sample>().FirstOrDefaultAsync(x => x.Id == request.Model.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Sample"], request.Model.Id);

            var existedSample = await dbContext.Set<Sample>().FirstOrDefaultAsNoTrackingAsync(x => x.Id != fromDb.Id && x.Code == request.Model.Code, cancellationToken);
            if (existedSample != null)
                throw exceptionFactory.BadRequestEx(ErrorCodes.SampleAlreadyExists, localizer[FiLocalizedStringType.EntityName, "Sample"], existedSample.Id);

            await dbContext.UpdatePartial(fromDb, mapped);
            await dbContext.SaveChangesAsync();
            return mapper.Map<SampleOutputModel>(fromDb);
        }

        public async Task<VoidResult> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
        {
            sessionDI.ExecutionTrace.InitTrace();

            var fromDb = await dbContext.Set<Sample>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (fromDb == null)
                throw exceptionFactory.BadRequestEx(BaseErrorCodes.ItemDoNotExists, localizer[FiLocalizedStringType.EntityName, "Sample"], request.Id);
            
            dbContext.Remove(fromDb);
            await dbContext.SaveChangesAsync();
            
            return new VoidResult();
        }
    }
}