using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class AccountCreditInputModelValidator : AbstractValidator<AccountCreditInputModel>
    {
        public AccountCreditInputModelValidator(IJsonStringLocalizer localizer)
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.CreditId).NotNull();
        }
    }

    public class CreateAccountCreditValidator : AbstractValidator<CreateAccountCreditCommand>
    {
        public CreateAccountCreditValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new AccountCreditInputModelValidator());
             */
        }
    }

    /*public class UpdateAccountCreditValidator : AbstractValidator<UpdateAccountCreditCommand>
    {
        public UpdateAccountCreditValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new AccountCreditInputModelValidator());
             
        }
    }*/
}