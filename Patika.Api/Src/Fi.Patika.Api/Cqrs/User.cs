using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateUserCommand(UserInputModel Model) : CommandBase<UserOutputModel>;

    public record UpdateUserCommand(int Id, UserInputModel Model) : CommandBase<UserOutputModel>;

    public record DeleteUserCommand(int Id) : CommandBase<VoidResult>;

    public record GetUserByParametersQuery : QueryBase<List<UserOutputModel>>;
    
    public record GetUserByKeyQuery(int Id) : QueryBase<UserOutputModel>;

}