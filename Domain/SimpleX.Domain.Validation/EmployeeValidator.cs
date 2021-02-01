using FluentValidation;
using SimpleX.Domain.Models.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(c => c.FirstName).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
