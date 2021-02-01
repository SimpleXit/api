using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Domain.Models.Assets
{
    public class ProductSupplierDto
    {
        public long Id { get; set; }
        public long ProductID { get; set; }
        public long SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierProductRef { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public long SalesUnitID { get; set; }
        public long OrderUnitID { get; set; }

    }
}
