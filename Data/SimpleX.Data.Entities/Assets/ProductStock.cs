using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities
{
    public class ProductStock : TrackEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long ProductID { get; set; }
        public byte StoreID { get; set; }
        public byte WarehouseID { get; set; }
        public string Location { get; set; }
        public decimal Stock { get; set; }
        public decimal Reserved { get; set; }
        public decimal Ordered { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }

        #endregion

    }
}
