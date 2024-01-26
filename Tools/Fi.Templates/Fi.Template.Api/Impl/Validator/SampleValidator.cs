using System;
using FluentValidation;
using Fi.Template.Api.Cqrs;
using Fi.TemplateUniqueName.Schema.Model;

namespace Fi.Template.Api.Impl.Validator
{
    public class SampleInputModelValidator : AbstractValidator<SampleInputModel>
    {
        public SampleInputModelValidator ()
        {
            RuleFor(x => x).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Code).MaximumLength(10);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.SampleType).NotEmpty();
        }
    }

    public class CreateSampleValidator : AbstractValidator<CreateSampleCommand>
    {
        public CreateSampleValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new SampleInputModelValidator());
             */
        }
    }
    
    public class UpdateSampleValidator : AbstractValidator<UpdateSampleCommand>
    {
        public UpdateSampleValidator()
        {
            /*
             * If you want to customize at command level, you can use here
             * RuleFor(x => x.Model).SetValidator(new SampleInputModelValidator());
             */
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}