using System;
using System.Collections.Generic;
using Fi.Patika.Schema.Model;
using Fi.Mediator.Message;

namespace Fi.Patika.Api.Cqrs
{
    public record CreateMoneyTransferCommand(MoneyTransferInputModel Model) : CommandBase<MoneyTransferOutputModel>;

    public record UpdateMoneyTransferCommand(int Id, MoneyTransferInputModel Model) : CommandBase<MoneyTransferOutputModel>;

    public record DeleteMoneyTransferCommand(int Id) : CommandBase<VoidResult>;

    public record GetMoneyTransferByParametersQuery : QueryBase<List<MoneyTransferOutputModel>>;
    
    public record GetMoneyTransferByKeyQuery(int Id) : QueryBase<MoneyTransferOutputModel>;
    public record TransferMoneyCommand(int Id,int AccountId, int DestId, MoneyTransferInputModel Model) : CommandBase<MoneyTransferOutputModel>;

}