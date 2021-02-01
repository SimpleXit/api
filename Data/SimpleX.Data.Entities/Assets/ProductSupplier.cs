using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities
{
    public class ProductSupplier : TrackEntity
    {
        public long Id { get; set; }
        public long ProductID { get; set; }
        public long SupplierID { get; set; }
        public string SupplierProductRef { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public long SalesUnitID { get; set; }
        public long OrderUnitID { get; set; }

        public Supplier Supplier { get; set; }
    }
}
