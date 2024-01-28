using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class DepositAndWithdrawInputModelValidator : AbstractValidator<DepositAndWithdrawInputModel>
    {
        public DepositAndWithdrawInputModelValidator(IJsonStringLocalizer localizer)
        {
            RuleFor(x => x).NotEmpty();
            RuleForEach(x => x.NameML).ChildRules(items =>
            {
                items.RuleFor(x => x.Value).NotEmpty().WithMessage(x => localizer[BaseErrorCodes.TranslationValueCanNotBeEmpty, x.LanguageCode, "Name"]);
                items.RuleFor(x => x.Value).MaximumLength(100).WithMessage(x => localizer[BaseErrorCodes.MaximumLengthWarning, "100", "Name"]);
            });
            RuleForEach(x => x.DescriptionML).ChildRules(items =>
            {
                items.RuleFor(x => x.Value).NotEmpty().WithMessage(x => localizer[BaseErrorCodes.TranslationValueCanNotBeEmpty, x.LanguageCode, "Description"]);
                items.RuleFor(x => x.Value).MaximumLength(255).WithMessage(x => localizer[BaseErrorCodes.MaximumLengthWarning, "255", "Description"]);
            });

            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.AccountId).NotNull();
            RuleFor(x => x.Amount).NotNull();
            RuleFor(x => x.isSucceded).NotNull();
        }
    }

    public class CreateDepositAndWithdrawValidator : AbstractValidator<CreateDepositAndWithdrawCommand>
    {
        public CreateDepositAndWithdrawValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new DepositAndWithdrawInputModelValidator());
             */
        }
    }

    public class UpdateDepositAndWithdrawValidator : AbstractValidator<UpdateDepositAndWithdrawCommand>
    {
        public UpdateDepositAndWithdrawValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new DepositAndWithdrawInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}