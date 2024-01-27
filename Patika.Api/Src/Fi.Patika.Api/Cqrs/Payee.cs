using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreatePayeeCommand(PayeeInputModel Model) : CommandBase<PayeeOutputModel>;

    public record UpdatePayeeCommand(int Id, PayeeInputModel Model) : CommandBase<PayeeOutputModel>;
    public record PaymentPayeeCommand(int Id, PayeeInputModel Model) : CommandBase<PayeeOutputModel>;

    public record DeletePayeeCommand(int Id) : CommandBase<VoidResult>;

    public record GetPayeeByParametersQuery : QueryBase<List<PayeeOutputModel>>;
    
    public record GetPayeeByKeyQuery(int Id) : QueryBase<PayeeOutputModel>;
}