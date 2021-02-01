using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Common.Enums;
using SimpleX.Data.Context;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models.Shared;
using SimpleX.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.Common
{
    public interface ICategoryService : IDisposable
    {
        Task<IEnumerable<CategoryDto>> GetCategories(EntityType owerType);
        Task<CategoryDto> CreateOrUpdateCategory(CategoryDto category);
        Task<int> DeleteCategories(IEnumerable<CategoryDto> categories);
        Task<CategoryDto> GetCategory(long categoryID);
    }

    public class CategoryService : ICategoryService, IDisposable
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly ILogger _logger;
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authentication;
        private readonly IValidator<CategoryDto> _categoryValidator;

        public CategoryService(
            ILoggerFactory loggerFactory,
            IAuthenticationService authentication,
            IDbContextFactory<SimpleContext> contextFactory,
            IMapper mapper,
            IValidator<CategoryDto> categoryValidator)
        {
            _logger = loggerFactory?.CreateLogger<CategoryService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
            _categoryValidator = categoryValidator ??
                throw new ArgumentNullException(nameof(categoryValidator));

            _logger.LogTrace("Logger injected in CategoryService");
            _logger.LogTrace("ContextFactory injected in CategoryService");
            _logger.LogTrace("Mapper injected in CategoryService");
            _logger.LogTrace("Authentication injected in CategoryService");
            _logger.LogTrace("CategoryValidator injected in CategoryService");

            _logger.LogTrace("Created CategoryService {0}", _guid);
        }

        public async Task<CategoryDto> GetCategory(long categoryID)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var category = await context.Category.Include(c => c.Translations)
                                                         .FirstOrDefaultAsync(c => c.Id == categoryID);
                                                         
                    return _mapper.Map<CategoryDto>(category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories(EntityType owerType)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var categories = await context.Category.Include(c => c.Translations)
                                                           .Where(c => c.OwnerType == owerType)
                                                           .ToListAsync();
                    return _mapper.Map<IEnumerable<CategoryDto>>(categories);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CategoryDto> CreateOrUpdateCategory(CategoryDto category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                var vr = _categoryValidator.Validate(category);
                if (!vr.IsValid)
                    throw new ArgumentException(vr.Errors.ToString());

                if (category.Id == 0)
                    return await CreateCategory(category);
                else
                    return await UpdateCategory(category.Id, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<CategoryDto> CreateCategory(CategoryDto categoryIn)
        {
            try
            {
                _logger.LogTrace("CategoryService.CreateCategory({0})", categoryIn);

                using (var context = _contextFactory.CreateDbContext())
                {
                    var category = _mapper.Map<Category>(categoryIn);

                    context.Category.Add(category);
                    await context.SaveChangesAsync();

                    return _mapper.Map(category, categoryIn);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<CategoryDto> UpdateCategory(long categoryId, CategoryDto categoryIn)
        {
            try
            {
                _logger.LogTrace("CategoryService.UpdateCategory({0},{1})", categoryId, categoryIn);

                using (var context = _contextFactory.CreateDbContext())
                {
                    var category = await context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);

                    if (category == null)
                    {
                        throw new KeyNotFoundException($"CategoryId {categoryId} not found.");
                    }

                    //Map dto values to entity:
                    _mapper.Map(categoryIn, category);

                    if (context.IsDirty())
                        await context.SaveChangesAsync();

                    //Map entity back to dto:
                    return _mapper.Map(category, categoryIn);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"CategoryId {categoryId} has been altered by an other user: {ex.Message}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"CategoryId {categoryId} could not be updated in db: {ex.Message}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<int> DeleteCategories(IEnumerable<CategoryDto> categoriesToRemove)
        {
            try
            {
                _logger.LogTrace($"CategoryService.DeleteCategories({string.Join(",", categoriesToRemove.Select(c => c.Id).ToArray())}");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfIds = categoriesToRemove.Select(a => a.Id).ToArray();
                    var categories = await context.Category.Where(c => arrayOfIds.Contains(c.Id)).ToListAsync();

                    context.RemoveRange(categories);
                    return await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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

            _logger.LogTrace("Disposing CategoryService {0}", _guid);

            disposed = true;
        }

        #endregion
    }
}
