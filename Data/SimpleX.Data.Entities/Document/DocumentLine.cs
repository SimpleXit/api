using SimpleX.Data.Entities.Base;

namespace SimpleX.Data.Entities
{
    public class DocumentLine : TrackEntity
    {
        public DocumentLine()
        {

        }

        #region Public Properties

        public long Id { get; set; }

        public long DocumentID { get; set; }
        public long LineDataID { get; set; }

        public long ParentDocumentID { get; set; }
        public long ParentLineID { get; set; }

        public int Sequence { get; set; }

        public decimal Quantity { get; set; }
        public decimal BackOrder { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public decimal Discount { get; set; }
        ///<summary>AmountExcl = Quantity * (Price - Discount)</summary>
        public decimal AmountExcl { get; set; }
        public decimal TaxPercentage { get; set; }



        #endregion

        #region Navigation Properties

        public Document Document { get; set; }
        public DocumentLineData DocumentLineData { get; set; }

        #endregion

    }

}
