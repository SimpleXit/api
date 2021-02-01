using FluentValidation;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class AttachmentValidator : AbstractValidator<AttachmentDto>
    {
        public AttachmentValidator()
        {
            RuleFor(c => c.Title).MaximumLength(200);
            RuleFor(c => c.Description).MaximumLength(2500);
            RuleFor(c => c.FileName).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
