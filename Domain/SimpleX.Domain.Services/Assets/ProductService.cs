using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Common.Enums;
using SimpleX.Data.Context;
using SimpleX.Data.Entities.Assets;
using SimpleX.Domain.Models.Assets;
using SimpleX.Domain.Models.Shared;
using SimpleX.Domain.Services.Helpers;
using SimpleX.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.Assets
{
    public interface IProductService : IDisposable
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProduct(long productId);
        Task<ProductDto> InitProduct();
        Task<ProductDto> CreateOrUpdateProduct(ProductDto productIn);
        Task DeleteProduct(long productId);
        Task<List<NoteDto>> GetProductNotes(long productId);
    }

    public class ProductService : IProductService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authentication;
        private readonly IValidator<ProductDto> _productValidator;
        private readonly DefaultValueSetter _defValSetter;

        private readonly Guid _guid = Guid.NewGuid();

        private IQueryable<Product> GetQueryAsNoTracking(SimpleContext context)
        {
            return context.Product.AsNoTracking()
                                    .Include(p => p.Attachments)
                                    .Include(p => p.Notes)
                                    .Include(p => p.ProductCharges)
                                        .ThenInclude(pc => pc.ChargeProduct)
                                    .Include(p => p.RelatedProducts)
                                        .ThenInclude(rp => rp.RelatedProduct)
                                    .Include(p => p.ProductStocks)
                                    .Include(p => p.ProductSuppliers)
                                    .Include(p => p.ProductUnits)
                                    .Include(p => p.Translations);
        }

        private IQueryable<Product> GetQueryWithTracking(SimpleContext context)
        {
            return context.Product.Include(p => p.Attachments)
                                    .Include(p => p.Notes)
                                    .Include(p => p.ProductCharges)
                                        .ThenInclude(pc => pc.ChargeProduct)
                                    .Include(p => p.RelatedProducts)
                                        .ThenInclude(rp => rp.RelatedProduct)
                                    .Include(p => p.ProductStocks)
                                    .Include(p => p.ProductSuppliers)
                                    .Include(p => p.ProductUnits)
                                    .Include(p => p.Translations);
        }

        public ProductService(
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IAuthenticationService authentication,
            IDbContextFactory<SimpleContext> contextFactory,
            IValidator<ProductDto> productValidator,
            DefaultValueSetter defValSetter)
        {
            _logger = loggerFactory?.CreateLogger<ProductService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
            _productValidator = productValidator ??
                throw new ArgumentNullException(nameof(productValidator));
            _defValSetter = defValSetter ??
                throw new ArgumentNullException(nameof(defValSetter));


            _logger.LogDebug("Logger injected in ProductService");
            _logger.LogDebug("DataContext injected in ProductService");
            _logger.LogDebug("Mapper injected in ProductService");
            _logger.LogDebug("Authentication injected in ProductService");
            _logger.LogDebug("ProductValidator injected in ProductService");

            _logger.LogDebug($"Created ProductService {_guid}");
        }

        public async Task<ProductDto> InitProduct()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var product = new ProductDto();
                    var defaults = await context.DefaultValue.AsNoTracking()
                                                             .Where(d => d.EntityType == EntityType.Product)
                                                             .ToListAsync();

                    _defValSetter.SetValues(defaults, product);

                    return product;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var products = await GetQueryAsNoTracking(context).ToListAsync();

                    return _mapper.Map<IEnumerable<ProductDto>>(products);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> GetProduct(long productId)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var product = await GetQueryAsNoTracking(context).FirstOrDefaultAsync(c => c.Id == productId);

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"ProductId {productId} not found.");
                    }

                    return _mapper.Map<ProductDto>(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ProductDto> CreateOrUpdateProduct(ProductDto productIn)
        {
            try
            {
                if (productIn == null)
                    throw new ArgumentNullException(nameof(productIn));

                var vr = _productValidator.Validate(productIn);
                if (!vr.IsValid)
                    throw new ArgumentException(vr.Errors.ToString());

                if (productIn.Id == 0)
                    return await CreateProduct(productIn);
                else
                    return await UpdateProduct(productIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<ProductDto> CreateProduct(ProductDto productIn)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    if (context.Product.AsNoTracking().Any(c => c.ProductNumber == productIn.ProductNumber))
                    {
                        throw new ApplicationException($"ProductNumber {productIn.ProductNumber} already exists.");
                    }

                    var product = _mapper.Map<Product>(productIn);

                    context.Product.Add(product);
                    await context.SaveChangesAsync();

                    return _mapper.Map(product, productIn);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                throw;
            }
        }

        private async Task<ProductDto> UpdateProduct(ProductDto productIn)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var product = await GetQueryWithTracking(context).FirstOrDefaultAsync(c => c.Id == productIn.Id);

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"ProductId {productIn.Id} not found.");
                    }

                    //Map dto values to entity:
                    _mapper.Map(productIn, product);

                    //_context.Update(product);

                    if (context.IsDirty())
                        await context.SaveChangesAsync();

                    //Map entity back to dto:
                    return _mapper.Map(product, productIn);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"ProductId {productIn.Id} has been altered by an other user: {ex.Message}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"ProductId {productIn.Id} could not be updated in db: {ex.Message}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteProduct(long productId)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var product = await GetQueryWithTracking(context).FirstOrDefaultAsync(c => c.Id == productId);

                    if (product == null)
                    {
                        throw new KeyNotFoundException($"ProductId {productId} not found.");
                    }


                    bool hasStock = false;
                    bool hasHistory = false;
                    bool hasMovements = false;

                    hasStock = await context.ProductStock.AsNoTracking()
                                                              .Where(s => s.ProductID == productId
                                                                       && s.Stock > 0)
                                                              .AnyAsync();

                    hasMovements = await context.ProductMovement.AsNoTracking()
                                                                 .Where(m => m.ProductID == productId
                                                                          && m.MovementType >= ProductMovementType.StockInventory)
                                                                 .AnyAsync();

                    //If no stock or movements have been found, check if product has been used before:
                    if (!hasStock && !hasMovements)
                        hasHistory = await context.DocumentLineData.AsNoTracking()
                                                                    .Where(d => d.ProductID == productId)
                                                                    .AnyAsync();

                    if (hasStock || hasHistory || hasMovements)
                    {
                        product.IsDeleted = true;
                        product.DeletedOn = DateTime.Now;
                        product.DeletedBy = _authentication.CurrentUsername;
                    }
                    else
                    {
                        context.Product.Remove(product);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                throw;
            }
        }

        public async Task<List<NoteDto>> GetProductNotes(long productId)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var notes = await context.ProductNote.Where(n => n.ProductID == productId).ToListAsync();

                    return _mapper.Map<List<NoteDto>>(notes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        #region IDisposable Members

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            _logger.LogDebug("Disposing ProductService {0}", _guid);

            disposed = true;
        }

        #endregion
    }
}
