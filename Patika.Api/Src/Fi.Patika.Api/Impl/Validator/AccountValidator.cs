using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class AccountInputModelValidator : AbstractValidator<AccountInputModel>
    {
        public AccountInputModelValidator(IJsonStringLocalizer localizer)
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.Balance).NotNull();
            RuleFor(x => x.Salary).NotNull();
            RuleFor(x => x.BankScore).NotNull();
            RuleFor(x => x.TotailDailyTransferAmount).NotNull();
            //RuleFor(x => x.AccountType).NotEmpty();
        }
    }

    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new AccountInputModelValidator());
             */
        }
    }

    public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new AccountInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}