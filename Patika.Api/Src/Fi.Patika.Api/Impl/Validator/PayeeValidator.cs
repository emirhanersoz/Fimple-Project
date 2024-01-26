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