using SimpleX.Common.Enums;

namespace SimpleX.Data.Entities
{
    public class DocumentLineData
    {
        public DocumentLineData()
        {

        }

        public long Id { get; set; }

        public LineType LineType { get; set; }

        public int? ProductID { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? ProductCost { get; set; }

    }
}
