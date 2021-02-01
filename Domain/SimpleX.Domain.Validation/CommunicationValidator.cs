using FluentValidation;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class CommunicationValidator : AbstractValidator<CommunicationDto>
    {
        public CommunicationValidator()
        {
            RuleFor(c => c.Description).MaximumLength(500);
            RuleFor(c => c.Remarks).MaximumLength(500);
            RuleFor(c => c.Correspondents).MaximumLength(500);
        }
    }
}
