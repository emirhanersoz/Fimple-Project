using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateAccountCommand(AccountInputModel Model) : CommandBase<AccountOutputModel>;

    public record UpdateAccountCommand(int Id, AccountInputModel Model) : CommandBase<AccountOutputModel>;

    public record DeleteAccountCommand(int Id) : CommandBase<VoidResult>;

    public record GetAccountByParametersQuery : QueryBase<List<AccountOutputModel>>;
    
    public record GetAccountByKeyQuery(int Id) : QueryBase<AccountOutputModel>;

}