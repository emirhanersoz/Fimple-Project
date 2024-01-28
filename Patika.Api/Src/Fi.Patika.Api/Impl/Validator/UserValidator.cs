using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class UserInputModelValidator : AbstractValidator<UserInputModel>
    {
        public UserInputModelValidator(IJsonStringLocalizer localizer)
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
            RuleFor(x => x.PasswordHash).NotEmpty();
            RuleFor(x => x.PasswordSalt).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
            RuleFor(x => x.RefreshToken).MaximumLength(100);
            RuleFor(x => x.TokenCreated).NotEmpty();
            RuleFor(x => x.TokenExpires).NotEmpty();
        }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new UserInputModelValidator());
             */
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new UserInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}