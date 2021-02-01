using FluentValidation;
using SimpleX.Domain.Models.Relations;

namespace SimpleX.Domain.Validation
{
    public class AppointmentValidator : AbstractValidator<AppointmentDto>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.Subject).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(a => a.Description).MaximumLength(2000);
            RuleFor(a => a.Location).MaximumLength(200);
            RuleFor(a => a.Start).NotNull().LessThanOrEqualTo(a => a.End);
            RuleFor(a => a.End).NotNull().GreaterThanOrEqualTo(a => a.Start);
        }
    }
}
