using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class LoginInputModelValidator : AbstractValidator<LoginInputModel>
    {
        public LoginInputModelValidator(IJsonStringLocalizer localizer)
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
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.LoginTime).NotEmpty();
        }
    }

    public class CreateLoginValidator : AbstractValidator<CreateLoginCommand>
    {
        public CreateLoginValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new LoginInputModelValidator());
             */
        }
    }

    public class UpdateLoginValidator : AbstractValidator<UpdateLoginCommand>
    {
        public UpdateLoginValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new LoginInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}