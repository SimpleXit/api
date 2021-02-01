using Microsoft.AspNetCore.Http;
using SimpleX.Domain.Models;
using SimpleX.Domain.Models.App;
using SimpleX.Helpers.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleX.Web.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _context;

        public AuthenticationService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public bool IsAuthenticated => _context?.HttpContext?.User != null;

        public string CurrentUsername => GetUser()?.Username;

        public string CurrentUserLanguageCode => GetUser()?.LanguageCode;

        public byte? CurrentUserStoreID => GetUser()?.StoreID;

        public long? CurrentUserId => GetUser()?.Id;

        public AppUserDto GetUser()
        {
            if (_context?.HttpContext == null)
                return null;
            else
                return new AppUserDto()
                {
                    Id = Convert.ToInt64(_context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                    Username = _context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value,
                    FirstName = _context.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value,
                    LastName = _context.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value,
                    Mail = _context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value,
                    LanguageCode = _context.HttpContext.User.FindFirst("Language")?.Value,
                    StoreID = byte.Parse(_context.HttpContext.User.FindFirst("Store")?.Value),
                    Roles = _context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value.Split(',').ToList()
                };
        }

        public bool UserHasRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class SeedingAuthenticationService : IAuthenticationService
    { 
        public SeedingAuthenticationService()
        {
            
        }

        public bool IsAuthenticated => true;

        public string CurrentUsername => "Admin";

        public string CurrentUserLanguageCode => "nl";

        public byte? CurrentUserStoreID => 1;

        public long? CurrentUserId => 1;

        public AppUserDto GetUser()
        {
            throw new NotImplementedException();
        }

        public bool UserHasRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
