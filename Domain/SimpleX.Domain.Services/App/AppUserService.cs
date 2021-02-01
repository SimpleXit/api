using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SimpleX.Core.Crypto;
using SimpleX.Data.Context;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models.App;
using SimpleX.Helpers.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.App
{
    public interface IAppUserService : IDisposable
    {
        Task<LoginResult> Login(string username, string password);
    }

    public class AppUserService : IAppUserService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly SecuritySettings _secSettings;
        private readonly ICryptoService _crypto;

        private readonly Guid _guid = Guid.NewGuid();

        public AppUserService(
            ILoggerFactory loggerFactory,
            IDbContextFactory<SimpleContext> contextFactory,
            IMapper mapper,
            SecuritySettings secSettings,
            ICryptoService crypto)
        {
            _logger = loggerFactory?.CreateLogger<AppUserService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _secSettings = secSettings ??
                throw new ArgumentNullException(nameof(secSettings));
            _crypto = crypto ??
                      throw new ArgumentNullException(nameof(crypto));

            _logger.LogTrace("Logger injected in AppUserService");
            _logger.LogTrace("ContextFactory injected in AppUserService");
            _logger.LogTrace("Mapper injected in AppUserService");
            _logger.LogTrace("SecuritySettings injected in AppUserService");
            _logger.LogTrace("CryptoService injected in AppUserService");

            _logger.LogTrace("Created AppUserService {0}", _guid);
        }

        //https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
        public async Task<LoginResult> Login(string username, string password)
        {
            try
            {
                _logger.LogTrace("AppUserService.Login({0},{1})", username, password);

                var result = new LoginResult();

                if (string.IsNullOrEmpty(username))
                {
                    result.IsSucces = false;
                    result.Error = "Username can't be empty.";
                    return result;
                }

                if (string.IsNullOrEmpty(password))
                {
                    result.IsSucces = false;
                    result.Error = "Password can't be empty.";
                    return result;
                }

                AppUser user = null;
                using (var context = _contextFactory.CreateDbContext())
                {
                    user = await context.AppUser.Include(u => u.Roles)
                                                .FirstOrDefaultAsync(u => u.Username == username && u.Password == _crypto.Encrypt(password));


                    if (user == null)
                    {
                        result.IsSucces = false;

                        if (await context.AppUser.FirstOrDefaultAsync(u => u.Username == username) == null)
                            result.Error = "Please enter a valid username.";
                        else
                            result.Error = "Please enter a valid password.";

                        return result;
                    }
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.ASCII.GetBytes(_secSettings.SecurityKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                        new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                        new Claim(ClaimTypes.Email, user.Mail ?? string.Empty),
                        new Claim("Language", user.LanguageCode ?? string.Empty),
                        new Claim("Store", user.StoreID.ToString()),
                        new Claim(ClaimTypes.Role, string.Join(",",user.Roles?.Select(x => x.Role)) ?? string.Empty)
                    }),
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                result.IsSucces = true;
                result.Token = tokenHandler.WriteToken(token); ;
                result.User = _mapper.Map<AppUserDto>(user);

                return result;
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

            _logger.LogTrace("Disposing AppUserService {0}", _guid);

            disposed = true;
        }

        #endregion
    }

    public class LoginResult
    {
        public bool IsSucces { get; set; } = false;
        public AppUserDto User { get; set; } = null;
        public string Token { get; set; } = null;
        public string Error { get; set; } = null;
    }
}
