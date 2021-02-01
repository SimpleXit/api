using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Supplier : TrackEntity, IEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string Number { get; set; }
        public long? CategoryID { get; set; }
        public string LanguageCode { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string TaxNumber { get; set; }
        public TaxCode TaxCode { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string Currency { get; set; }
        public long ContactInfoID { get; set; }
        public byte StoreID { get; set; }

        #endregion

        #region Navigation Properties

        public SupplierContactInfo ContactInfo { get; set; }

        public List<SupplierAddress> Addresses { get; set; }
        public List<SupplierAttachment> Attachments { get; set; }
        public List<SupplierCommunication> Communications { get; set; }
        public List<SupplierNote> Notes { get; set; }
        public List<SupplierPerson> Persons { get; set; }

        #endregion

    }
}
