using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Data.Context;
using System;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.Database
{
    public interface IMigrationService : IDisposable
    {
        void Migrate();
        Task MigrateAsync();
    }

    public class MigrationService : IMigrationService
    {
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly ILogger _logger;

        public MigrationService(
            IDbContextFactory<SimpleContext> contextFactory,
            ILoggerFactory loggerFactory)
        {
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _logger = loggerFactory?.CreateLogger<MigrationService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger.LogTrace("ContextFactory injected in MigrationService");
            _logger.LogTrace("LoggerFactory injected in MigrationService");

            _logger.LogTrace("Created MigrationService");
        }

        public async Task MigrateAsync()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void Migrate()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    context.Database.Migrate();
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

            disposed = true;
        }

        #endregion
    }

    public class DebugMigrationService : IMigrationService
    {
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly ILogger _logger;

        public DebugMigrationService(IDbContextFactory<SimpleContext> contextFactory, ILoggerFactory loggerFactory)
        {
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _logger = loggerFactory?.CreateLogger<DebugMigrationService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));

            _logger.LogTrace("ContextFactory injected in DebugMigrationService");
            _logger.LogTrace("LoggerFactory injected in DebugMigrationService");

            _logger.LogTrace("Created DebugMigrationService");
        }

        public async Task MigrateAsync()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void Migrate()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    context.Database.EnsureCreated();
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

            disposed = true;
        }

        #endregion
    }
}
