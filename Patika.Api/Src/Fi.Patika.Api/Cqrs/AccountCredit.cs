using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateAccountCreditCommand(AccountCreditInputModel Model) : CommandBase<AccountCreditOutputModel>;

    public record GetAccountCreditByParametersQuery : QueryBase<List<AccountCreditOutputModel>>;
    
}