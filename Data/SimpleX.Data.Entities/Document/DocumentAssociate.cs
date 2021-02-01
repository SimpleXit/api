using SimpleX.Common.Enums;

namespace SimpleX.Data.Entities
{
    public class DocumentAssociate
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public long? CategoryID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string TaxNumber { get; set; }
        public TaxCode TaxCode { get; set; }
        public string LanguageCode { get; set; }

        public DocumentAssociateAddress Address { get; set; }
        public DocumentAssociateContactInfo ContactInfo { get; set; }

        public long? CustomerID { get; set; }
        public long? SupplierID { get; set; }
        public Customer Customer { get; set; }
        public Supplier Supplier { get; set; }
    }
}
