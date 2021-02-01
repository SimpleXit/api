namespace SimpleX.Data.Entities
{
    public class DocumentTax
    {
        public DocumentTax()
        {

        }

        public long Id { get; set; }
        public long DocumentID { get; set; }
        public int TaxRate { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal AmountExcl { get; set; }
        public decimal AmountIncl { get; set; }
    }
}
