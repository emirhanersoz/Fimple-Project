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