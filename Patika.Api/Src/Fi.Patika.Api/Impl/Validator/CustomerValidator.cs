using System;
using Fi.Patika.Api.Cqrs;
using Fi.Patika.Schema.Model;
using Fi.Infra.Abstraction;
using Fi.Infra.Exceptions;
using FluentValidation;

namespace Fi.Patika.Api.Impl.Validator
{
    public class CustomerInputModelValidator : AbstractValidator<CustomerInputModel>
    {
        public CustomerInputModelValidator(IJsonStringLocalizer localizer)
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.PhoneNumber).MaximumLength(20);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).MaximumLength(100);
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.City).MaximumLength(50);
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.State).MaximumLength(50);
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Address).MaximumLength(200);
        }
    }

    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new CustomerInputModelValidator());
             */
        }
    }

    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new CustomerInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}