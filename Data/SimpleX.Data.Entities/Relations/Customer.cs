using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Customer : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public long Number { get; set; }
        public long? CategoryID { get; set; }
        public string LanguageCode { get; set; }

        //Personal info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public string TaxNumber { get; set; }
        public TaxCode TaxCode { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public TermOfPayment TermOfPayment { get; set; }
        public string Currency { get; set; }
        public byte StoreID { get; set; }

        #endregion

        #region Navigation Properties

        public CustomerContactInfo ContactInfo { get; set; } = new CustomerContactInfo();

        public List<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<CustomerAttachment> Attachments { get; set; } = new List<CustomerAttachment>();
        public List<CustomerCommunication> Communications { get; set; } = new List<CustomerCommunication>();
        public List<CustomerNote> Notes { get; set; } = new List<CustomerNote>();
        public List<CustomerPerson> Persons { get; set; } = new List<CustomerPerson>();

        #endregion
    }
}
