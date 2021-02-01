namespace SimpleX.Data.Entities
{
    public class Store
    {
        #region Public Properties

        public byte Id { get; set; }
        public string Name { get; set; }
        public long? AddressID { get; set; }
        public string TaxNumber { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public long? ContactInfoID { get; set; }

        public ContactInfo ContactInfo { get; set; }
        public Address Address { get; set; }

        #endregion
    }
}
