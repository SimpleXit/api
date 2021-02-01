using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Data.Entities.Assets
{
    public class ProductCharge
    {
        public long Id { get; set; }
        public long ProductID { get; set; }

        public long? ChargeProductID { get; set; }
        public string ChargeProductNumber { get; set; }
        public string ChargeDescription { get; set; }

        public bool IncludedInProductPrice { get; set; }
        public decimal? Price { get; set; }

        public Product ChargeProduct { get; set; }
    }
}
