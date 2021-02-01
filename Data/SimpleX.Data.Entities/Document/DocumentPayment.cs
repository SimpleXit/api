using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using System;

namespace SimpleX.Data.Entities
{
    public class DocumentPayment : TrackEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long DocumentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }

        public bool IsLocked { get; set; }

        #endregion


        public Document Document { get; set; }
    }
}
