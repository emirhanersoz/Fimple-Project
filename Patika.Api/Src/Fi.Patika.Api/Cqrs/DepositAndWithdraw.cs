using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateDepositAndWithdrawCommand(DepositAndWithdrawInputModel Model) : CommandBase<DepositAndWithdrawOutputModel>;

    public record UpdateDepositAndWithdrawCommand(int Id, DepositAndWithdrawInputModel Model) : CommandBase<DepositAndWithdrawOutputModel>;

    public record TransactionWithdrawCommand(int Id, DepositAndWithdrawInputModel Model) : CommandBase<DepositAndWithdrawOutputModel>;
    public record TransactionDepositCommand(int Id, DepositAndWithdrawInputModel Model) : CommandBase<DepositAndWithdrawOutputModel>;

    public record DeleteDepositAndWithdrawCommand(int Id) : CommandBase<VoidResult>;

    public record GetDepositAndWithdrawByParametersQuery : QueryBase<List<DepositAndWithdrawOutputModel>>;
    
    public record GetDepositAndWithdrawByKeyQuery(int Id) : QueryBase<DepositAndWithdrawOutputModel>;

}