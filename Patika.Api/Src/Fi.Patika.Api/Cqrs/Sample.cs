/*using System;
using System.Collections.Generic;
using Fi.Mediator.Message;
using Fi.Patika.Schema.Model;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateSampleCommand(SampleInputModel Model) : CommandBase<SampleOutputModel>;

    public record UpdateSampleCommand(int Id, SampleInputModel Model) : CommandBase<SampleOutputModel>;

    public record DeleteSampleCommand(int Id) : CommandBase<VoidResult>;

    public record GetSampleByCodeQuery(string Code) : QueryBase<List<SampleOutputModel>>;

    public record GetSampleByKeyQuery(int Id) : QueryBase<SampleOutputModel>;
    
    public record GetAllSampleQuery : QueryBase<List<SampleOutputModel>>;
}*/