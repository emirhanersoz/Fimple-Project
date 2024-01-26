using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateCustomerCommand(CustomerInputModel Model) : CommandBase<CustomerOutputModel>;

    public record UpdateCustomerCommand(int Id, CustomerInputModel Model) : CommandBase<CustomerOutputModel>;

    public record DeleteCustomerCommand(int Id) : CommandBase<VoidResult>;

    public record GetCustomerByParametersQuery : QueryBase<List<CustomerOutputModel>>;
    
    public record GetCustomerByKeyQuery(int Id) : QueryBase<CustomerOutputModel>;

}