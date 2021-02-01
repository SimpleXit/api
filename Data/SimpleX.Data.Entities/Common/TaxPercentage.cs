using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Interfaces;
using System;

namespace SimpleX.Data.Entities
{
    public class TaxPercentage : IEntity
    {
        public long Id { get; set; }
        public DateTime? From { get; set; }
        public DateTime? Until { get; set; }
        public TaxRate Rate { get; set; }
        public decimal Percentage { get; set; }
    }
}
