using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateLoginCommand(LoginInputModel Model) : CommandBase<LoginOutputModel>;

    public record UpdateLoginCommand(int Id, LoginInputModel Model) : CommandBase<LoginOutputModel>;

    public record DeleteLoginCommand(int Id) : CommandBase<VoidResult>;

    public record GetLoginByParametersQuery : QueryBase<List<LoginOutputModel>>;
    
    public record GetLoginByKeyQuery(int Id) : QueryBase<LoginOutputModel>;

}