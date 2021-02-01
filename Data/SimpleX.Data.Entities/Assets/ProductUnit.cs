using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities
{
    public class ProductUnit : TrackEntity
    {
        public long Id { get; set; }
        public long ProductID { get; set; }
        public long UnitID { get; set; }
        public decimal SalesQuantity { get; set; }
        public decimal OrderQuantity { get; set; }
        public string UnitEAN { get; set; }
        public string ShortName { get; set; }

        public Unit Unit { get; set; }
    }
}
