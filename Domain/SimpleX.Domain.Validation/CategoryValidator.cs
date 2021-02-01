using FluentValidation;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.CategoryCode).MaximumLength(25);
            RuleFor(c => c.Name).MaximumLength(200);
            RuleFor(c => c.Description).MaximumLength(2500);

            RuleForEach(c => c.Translations).SetValidator(new TranslationValidator());
        }
    }
}
