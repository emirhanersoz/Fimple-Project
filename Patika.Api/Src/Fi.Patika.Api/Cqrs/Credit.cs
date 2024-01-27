using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateCreditCommand(CreditInputModel Model) : CommandBase<CreditOutputModel>;

    public record GetLoanCreditCommand(int accountId, int creditId) : CommandBase<AccountCreditOutputModel>;

    public record UpdateCreditCommand(int Id, CreditInputModel Model) : CommandBase<CreditOutputModel>;

    public record DeleteCreditCommand(int Id) : CommandBase<VoidResult>;

    public record GetCreditByParametersQuery : QueryBase<List<CreditOutputModel>>;
    
    public record GetCreditByKeyQuery(int Id) : QueryBase<CreditOutputModel>;

}