using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using System;

namespace SimpleX.Data.Entities
{
    public partial class ProductMovement : TrackEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long ProductID { get; set; }
        public byte StoreID { get; set; }
        public byte WarehouseID { get; set; }
        public string Location { get; set; }
        public DateTime MovementDate { get; set; }
        public ProductMovementType MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal NewStock { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public string Reference { get; set; }
        public long? DocumentID { get; set; }
        public long? DocumentLineID { get; set; }
        public string MovementReason { get; set; }

        #endregion
    }
}
