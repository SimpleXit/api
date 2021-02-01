using FluentValidation;
using SimpleX.Domain.Models.Shared;

namespace SimpleX.Domain.Validation
{
    public class NoteValidator : AbstractValidator<NoteDto>
    {
        public NoteValidator()
        {
            RuleFor(c => c.Title).MaximumLength(200);
            RuleFor(c => c.Description).NotNull().NotEmpty().MaximumLength(2500);
        }
    }
}
