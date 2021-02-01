using SimpleX.Common.Enums;
using SimpleX.Data.Entities.Assets;
using SimpleX.Data.Entities.Base;
using SimpleX.Data.Entities.Interfaces;
using System.Collections.Generic;

namespace SimpleX.Data.Entities.Assets
{
    public class Product : TrackEntity, ITrackEntity
    {
        #region Public Properties

        public long Id { get; set; }
        public string ProductNumber { get; set; }
        public string ProductEAN { get; set; }
        public string ShortName { get; set; }
        public long? SupplierID { get; set; }
        public string SupplierProductRef { get; set; }
        public long? CategoryID { get; set; }
        public int? BrandID { get; set; }
        public string BrandProductRef { get; set; }
        public decimal Cost { get; set; }
        public decimal CostAverage { get; set; }
        public decimal Price { get; set; }
        public decimal PriceRetail { get; set; }
        public TaxRate TaxRate { get; set; }

        public long? SalesUnitID { get; set; }
        public long? OrderUnitID { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public bool FollowStock { get; set; }

        public byte[] ProductImage { get; set; }

        #endregion

        #region Navigation Properties

        public List<ProductAttachment> Attachments { get; set; }
        public List<ProductAttribute> Attributes { get; set; }
        public List<ProductNote> Notes { get; set; }
        public List<ProductStock> ProductStocks { get; set; }
        public List<ProductSupplier> ProductSuppliers { get; set; }
        public List<ProductTranslation> Translations { get; set; }
        public List<ProductUnit> ProductUnits { get; set; }
        public List<ProductCharge> ProductCharges { get; set; }
        public List<ProductRelation> RelatedProducts { get; set; }

        #endregion

    }
}
