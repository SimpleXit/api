using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Base;
using System;
using System.Collections.Generic;

namespace SimpleX.Data.Entities
{
    public class Document : TrackEntity
    {
        #region Public Properties

        public long Id { get; set; }

        public DocumentJournal DocumentJournal { get; set; }
        public byte DocumentStoreID { get; set; }
        public byte DocumentLevelID { get; set; }
        public byte DocumentSourceID { get; set; }
        public int DocumentNumber { get; set; }

        public DateTime? DocumentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string Reference { get; set; }
        public string Remarks { get; set; }

        public long AssociateID { get; set; }
        public long? DeliveryAddressID { get; set; }
        public long? InvoiceAddressID { get; set; }

        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }

        public decimal AmountExcl { get; set; }
        public decimal AmountIncl { get; set; }



        #endregion

        #region Navigation Properties

        public DocumentAssociate Associate { get; set; }
        public Address DeliveryAddress { get; set; }
        public Address InvoiceAddres { get; set; }

        public List<DocumentTax> DocumentTaxes { get; set; }
        public List<DocumentLine> DocumentLines { get; set; }
        public List<DocumentNote> Notes { get; set; }
        public List<DocumentAttachment> Attachments { get; set; }
        public List<DocumentPayment> Payments { get; set; }
        public List<DocumentTree> ParentDocuments { get; set; }
        public List<DocumentTree> ChildDocuments { get; set; }

        #endregion

    }
}
