using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using SimpleX.Data.Entities.App;
using SimpleX.Domain.Models;
using SimpleX.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.App
{
    public interface IAppMenuService : IDisposable
    {
        Task<IEnumerable<AppMenuDto>> GetAppMenu(string currentUserLanguageCode);
        Task<IEnumerable<AppMenuDto>> GetUserMenu(long? userId = null);
        Task<IEnumerable<AppMenuDto>> GetUserFavorites(long? userId = null);
    }

    public class AppMenuService : IAppMenuService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authentication;
        private readonly Guid _guid = Guid.NewGuid();

        public AppMenuService(
            ILoggerFactory loggerFactory,
            IDbContextFactory<SimpleContext> contextFactory,
            IMapper mapper,
            IAuthenticationService authentication)
        {
            _logger = loggerFactory?.CreateLogger<AppMenuService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));

            _logger.LogTrace("LoggerFactory injected in AppMenuService");
            _logger.LogTrace("ContextFactory injected in AppMenuService");
            _logger.LogTrace("Mapper injected in AppMenuService");
            _logger.LogTrace("Authentication injected in AppMenuService");

            _logger.LogTrace("Created AppMenuService {0}", _guid);
        } 

        public async Task<IEnumerable<AppMenuDto>> GetAppMenu(string languageCode = null)
        {
            try
            {
                languageCode = languageCode ?? _authentication.CurrentUserLanguageCode;

                if (languageCode == null)
                    throw new ArgumentNullException(nameof(languageCode));

                _logger.LogTrace("AppMenuService.GetAppMenu({0})", languageCode);

                using (SimpleContext context = _contextFactory.CreateDbContext())
                {
                    var q = from m in context.AppMenu
                            from t in context.AppMenuTranslation.Where(t => t.MenuID == m.Id && t.LanguageCode == languageCode).DefaultIfEmpty()
                            select new AppMenuDto()
                            {
                                Id = m.Id,
                                Caption = t.Name ?? m.Name,
                                Name = m.Name,
                                View = m.View,
                                ParentID = m.ParentID
                            };

                    return await q.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AppMenuDto>> GetUserMenu(long? userId = null)
        {
            try
            {
                userId = userId ?? _authentication.CurrentUserId;

                if (userId == null)
                    throw new ArgumentNullException(nameof(userId));

                _logger.LogTrace("AppMenuService.GetUserMenu({0})", userId);

                using (var context = _contextFactory.CreateDbContext())
                {
                    var userMenu = await context.AppMenu.Where(m => m.View != null && m.IsActive == true)
                                                         .ToListAsync();

                    //TODO: User Security

                    var menusDto = _mapper.Map<List<AppMenuDto>>(userMenu);

                    return menusDto;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<AppMenuDto>> GetUserFavorites(long? userId = null)
        {
            try
            {
                userId = userId ?? _authentication.CurrentUserId;

                if (userId == null)
                    throw new ArgumentNullException(nameof(userId));

                _logger.LogTrace("AppMenuService.GetUserFavorites({0})", userId);

                using (var context = _contextFactory.CreateDbContext())
                {
                    //TODO:
                    var userMenu = await context.AppMenu.Where(m => m.View == "CustomerView")
                                                         .ToListAsync();

                    List<AppMenuDto> menuDtos = _mapper.Map<List<AppMenuDto>>(userMenu);

                    foreach (var mnu in menuDtos)
                    {
                        mnu.IsFavorite = true;
                    }

                    return menuDtos;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task LoadChildMenus(IEnumerable<AppMenu> rootMenu)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    foreach (var menu in rootMenu)
                    {
                        await context.Entry(menu)
                                      .Collection(m => m.SubMenus)
                                      .Query()
                                      .LoadAsync();
                        await LoadChildMenus(menu.SubMenus);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        private async Task LoadTranslation(IEnumerable<AppMenu> menus, string languageCode)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    foreach (var menu in menus)
                    {
                        await context.Entry(menu)
                                      .Collection(m => m.Translations)
                                      .Query()
                                      .Where(t => t.LanguageCode == languageCode)
                                      .LoadAsync();
                    }
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

            _logger.LogTrace("Disposing AppMenuService {0}", _guid);

            disposed = true;
        }

        #endregion
    }
}
