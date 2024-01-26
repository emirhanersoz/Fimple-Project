using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateSupportRequestCommand(SupportRequestInputModel Model) : CommandBase<SupportRequestOutputModel>;

    public record UpdateSupportRequestCommand(int Id, SupportRequestInputModel Model) : CommandBase<SupportRequestOutputModel>;

    public record DeleteSupportRequestCommand(int Id) : CommandBase<VoidResult>;

    public record GetSupportRequestByParametersQuery : QueryBase<List<SupportRequestOutputModel>>;
    
    public record GetSupportRequestByKeyQuery(int Id) : QueryBase<SupportRequestOutputModel>;

}