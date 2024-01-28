using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class CreditInputModelValidator : AbstractValidator<CreditInputModel>
    {
        public CreditInputModelValidator(IJsonStringLocalizer localizer)
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
            RuleFor(x => x.TotalAmount).NotNull();
            RuleFor(x => x.MontlyPayment).NotNull();
            RuleFor(x => x.RepaymentPeriodMonths).NotNull();
            RuleFor(x => x.LoanDate).NotEmpty();
        }
    }

    public class CreateCreditValidator : AbstractValidator<CreateCreditCommand>
    {
        public CreateCreditValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new CreditInputModelValidator());
             */
        }
    }

    public class UpdateCreditValidator : AbstractValidator<UpdateCreditCommand>
    {
        public UpdateCreditValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new CreditInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}