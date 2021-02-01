using FluentValidation;
using SimpleX.Domain.Models.Assets;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Validation
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(c => c.ProductNumber).NotEmpty().MaximumLength(50);
            RuleFor(c => c.ProductEAN).MaximumLength(50);
            RuleFor(c => c.ShortName).MaximumLength(50);
            RuleFor(c => c.SupplierProductRef).MaximumLength(50);
            RuleFor(c => c.BrandProductRef).MaximumLength(50);
        }
    }
}
