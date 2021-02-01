using SimpleX.Common.Enums;
using SimpleX.Domain.Models.Base;
using SimpleX.Domain.Models.Common;
using SimpleX.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SimpleX.Domain.Models.Assets
{
    public class ProductDto : Dto<ProductDto>
    {
        private long _id;
        private string _productNumber;
        private string _productEAN;
        private string _shortName;
        private long? _supplierID;
        private string _supplierProductRef;
        private long? _categoryID;
        private int? _brandID;
        private string _brandProductRef;
        private decimal _cost;
        private decimal _costAverage;
        private decimal _price;
        private decimal _priceRetail;
        private TaxRate _taxRate;

        private long? _salesUnitID;
        private long? _orderUnitID;
        private decimal? _netWeight;
        private decimal? _grossWeight;
        private decimal? _length;
        private decimal? _width;
        private decimal? _height;
        private decimal _minStock;
        private decimal _maxStock;
        private bool _followStock;
        private byte[] _productImage;

        private DateTime? _createdOn;
        private string _createdBy;
        private DateTime? _updatedOn;
        private string _updatedBy;
        private byte[] _rowVersion;


        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string ProductNumber
        {
            get => _productNumber;
            set => SetProperty(ref _productNumber, value);
        }
        public string ProductEAN
        {
            get => _productEAN;
            set => SetProperty(ref _productEAN, value);
        }
        public string ShortName
        {
            get => _shortName;
            set => SetProperty(ref _shortName, value);
        }
        public long? SupplierID
        {
            get => _supplierID;
            set => SetProperty(ref _supplierID, value);
        }
        public string SupplierProductRef
        {
            get => _supplierProductRef;
            set => SetProperty(ref _supplierProductRef, value);
        }
        public long? CategoryID
        {
            get => _categoryID;
            set => SetProperty(ref _categoryID, value);
        }
        public int? BrandID
        {
            get => _brandID;
            set => SetProperty(ref _brandID, value);
        }
        public string BrandProductRef
        {
            get => _brandProductRef;
            set => SetProperty(ref _brandProductRef, value);
        }
        public decimal Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }
        public decimal CostAverage
        {
            get => _costAverage;
            set => SetProperty(ref _costAverage, value);
        }
        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }
        public decimal PriceRetail
        {
            get => _priceRetail;
            set => SetProperty(ref _priceRetail, value);
        }
        public TaxRate TaxRate
        {
            get => _taxRate;
            set => SetProperty(ref _taxRate, value);
        }

        public long? SalesUnitID
        {
            get => _salesUnitID;
            set => SetProperty(ref _salesUnitID, value);
        }
        public long? OrderUnitID
        {
            get => _orderUnitID;
            set => SetProperty(ref _orderUnitID, value);
        }
        public decimal? NetWeight
        {
            get => _netWeight;
            set => SetProperty(ref _netWeight, value);
        }
        public decimal? GrossWeight
        {
            get => _grossWeight;
            set => SetProperty(ref _grossWeight, value);
        }
        public decimal? Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }
        public decimal? Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }
        public decimal? Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }
        public decimal MinStock
        {
            get => _minStock;
            set => SetProperty(ref _minStock, value);
        }
        public decimal MaxStock
        {
            get => _maxStock;
            set => SetProperty(ref _maxStock, value);
        }
        public bool FollowStock
        {
            get => _followStock;
            set => SetProperty(ref _followStock, value);
        }

        public byte[] ProductImage
        {
            get => _productImage;
            set => SetProperty(ref _productImage, value);
        }

        public DateTime? CreatedOn
        {
            get => _createdOn;
            set => SetProperty(ref _createdOn, value);
        }
        public string CreatedBy
        {
            get => _createdBy;
            set => SetProperty(ref _createdBy, value);
        }
        public DateTime? UpdatedOn
        {
            get => _updatedOn;
            set => SetProperty(ref _updatedOn, value);
        }
        public string UpdatedBy
        {
            get => _updatedBy;
            set => SetProperty(ref _updatedBy, value);
        }
        public byte[] RowVersion
        {
            get => _rowVersion;
            set => SetProperty(ref _rowVersion, value);
        }

        public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
        public List<AttributeDto> Attributes { get; set; } = new List<AttributeDto>();
        public List<NoteDto> Notes { get; set; } = new List<NoteDto>();
        public List<ProductSupplierDto> ProductSuppliers { get; set; } = new List<ProductSupplierDto>();
        public List<ProductDto> RelatedProducts { get; set; } = new List<ProductDto>();
    }

    public class ProductSearchResult
    {
        //TODO:

        public decimal ProductStock { get; set; }
        public decimal ProductStockReserved { get; set; }

        public Color StockAvailable
        {
            get
            {
                switch (ProductStock - ProductStockReserved)
                {
                    case decimal n when (n > 0):
                        return Color.Green;
                    case 0:
                        return Color.Orange;
                    case decimal n when (n < 0):
                    default:
                        return Color.Red;
                }
            }
        }
    }
}
