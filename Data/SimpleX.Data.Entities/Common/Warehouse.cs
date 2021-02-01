namespace SimpleX.Data.Entities
{
    public class Warehouse
    {
        #region Public Properties

        public byte Id { get; set; }
        public string Name { get; set; }

        public long? AddressID { get; set; }
        public Address Address { get; set; }
        public long? ContactInfoID { get; set; }
        public ContactInfo ContactInfo { get; set; }


        #endregion
    }
}
