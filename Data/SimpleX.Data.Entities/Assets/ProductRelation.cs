using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleX.Data.Entities.Assets
{
    public class ProductRelation
    {
        public long Id { get; set; }
        public long ProductID { get; set; }
        public long RelatedProductID { get; set; }
        public long? Sequence { get; set; }
        public string Remarks { get; set; }

        public Product RelatedProduct { get; set; }
    }
}
