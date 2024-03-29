using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class SupportRequestInputModelValidator : AbstractValidator<SupportRequestInputModel>
    {
        public SupportRequestInputModelValidator(IJsonStringLocalizer localizer)
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
            RuleFor(x => x.CustomerId).NotNull();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Subject).MaximumLength(100);
            RuleFor(x => x.Comment).NotEmpty();
            RuleFor(x => x.Comment).MaximumLength(100);
            RuleFor(x => x.isAnswered).NotNull();
            RuleFor(x => x.Answered).MaximumLength(100);
        }
    }

    public class CreateSupportRequestValidator : AbstractValidator<CreateSupportRequestCommand>
    {
        public CreateSupportRequestValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new SupportRequestInputModelValidator());
             */
        }
    }

    public class UpdateSupportRequestValidator : AbstractValidator<UpdateSupportRequestCommand>
    {
        public UpdateSupportRequestValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new SupportRequestInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}