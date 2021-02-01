using FluentValidation;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class TranslationValidator : AbstractValidator<TranslationDto>
    {
        public TranslationValidator()
        {
            RuleFor(c => c.LanguageCode).NotNull().NotEmpty().Length(2);
            RuleFor(c => c.Name).MaximumLength(250);
            RuleFor(c => c.Description).MaximumLength(2500);
        }
    }
}
