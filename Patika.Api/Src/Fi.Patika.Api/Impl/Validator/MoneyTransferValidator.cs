using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class MoneyTransferInputModelValidator : AbstractValidator<MoneyTransferInputModel>
    {
        public MoneyTransferInputModelValidator(IJsonStringLocalizer localizer)
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.DestAccountId).NotNull();
            RuleFor(x => x.Amount).NotNull();
            RuleFor(x => x.Comment).NotEmpty();
            RuleFor(x => x.Comment).MaximumLength(200);
        }
    }

    public class CreateMoneyTransferValidator : AbstractValidator<CreateMoneyTransferCommand>
    {
        public CreateMoneyTransferValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new MoneyTransferInputModelValidator());
             */
        }
    }

    public class UpdateMoneyTransferValidator : AbstractValidator<UpdateMoneyTransferCommand>
    {
        public UpdateMoneyTransferValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new MoneyTransferInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}