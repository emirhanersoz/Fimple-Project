using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class PayeeInputModelValidator : AbstractValidator<PayeeInputModel>
    {
        public PayeeInputModelValidator(IJsonStringLocalizer localizer)
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
            RuleFor(x => x.PaymentDay).NotNull();
            RuleFor(x => x.isPayment).NotNull();
        }
    }

    public class CreatePayeeValidator : AbstractValidator<CreatePayeeCommand>
    {
        public CreatePayeeValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new PayeeInputModelValidator());
             */
        }
    }

    public class UpdatePayeeValidator : AbstractValidator<UpdatePayeeCommand>
    {
        public UpdatePayeeValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new PayeeInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}