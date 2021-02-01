using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Core.Crypto;
using SimpleX.Data.Entities.Base;

using SimpleX.Helpers.Interfaces;
using SimpleX.Helpers.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Data.Context
{
    public partial class SimpleContext : DbContext, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authentication;

        private readonly Guid _guid = Guid.NewGuid();

        public SimpleContext(
            DbContextOptions options,
            ILogger<SimpleContext> logger,
            IAuthenticationService authentication) : base(options)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));

            _logger.LogTrace("Options injected in DataContext");
            _logger.LogTrace("Authentication injected in DataContext");

            _logger.LogTrace($"Created DataContext {0}", _guid);
        }

        public bool IsDirty()
        {
            try
            {
                return ChangeTracker.Entries()
                                    .Where(x => x.State == EntityState.Added ||
                                                x.State == EntityState.Modified ||
                                                x.State == EntityState.Deleted).Any();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public override int SaveChanges()
        {
            try
            {
                _logger.LogTrace($"DataContext.SaveChanges() {0}", _guid);

                this.SetTrackInfo();
                this.SetDataHistory();

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _logger.LogTrace($"DataContext.SaveChangesAsync() {0}", _guid);

                this.SetTrackInfo();
                this.SetDataHistory();

                return (await base.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private void SetDataHistory()
        {
            this.EnsureAutoHistory(() => new DataHistory()
            {
                UpdatedBy = _authentication.CurrentUsername,
                UpdatedOn = DateTime.Now
            });
        }

        private void SetTrackInfo()
        {
            var createdOrUpdatedEntityList = ChangeTracker.Entries()
                                                            .Where(x => x.Entity is TrackEntity &&
                                                                       (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in createdOrUpdatedEntityList)
            {
                if (entity.State == EntityState.Added)
                {
                    ((TrackEntity)entity.Entity).CreatedBy = _authentication.CurrentUsername;
                    ((TrackEntity)entity.Entity).CreatedOn = DateTime.Now;
                    ((TrackEntity)entity.Entity).UpdatedBy = _authentication.CurrentUsername;
                    ((TrackEntity)entity.Entity).UpdatedOn = DateTime.Now;
                }
                else if (entity.State == EntityState.Modified)
                {
                    ((TrackEntity)entity.Entity).UpdatedBy = _authentication.CurrentUsername;
                    ((TrackEntity)entity.Entity).UpdatedOn = DateTime.Now;
                }
            }
        }

        #region IDisposable Members

        private bool disposed;
        public override void Dispose()
        {
            Dispose(true);
            base.Dispose();
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            _logger.LogTrace("Disposing DataContext {0}", _guid);

            disposed = true;
        }

        #endregion

        //public async Task CreateFnAndSp()
        //{
        //    try
        //    {
        //        _logger.LogTrace($"DataContext.CreateFnAndSp() {0}", _guid);

        //        //Functions:
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewCustomerNumber.Drop());
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewCustomerNumber.Create());
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewEmployeeNumber.Drop());
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewEmployeeNumber.Create());
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewSupplierNumber.Drop());
        //        //await this.Database.ExecuteSqlRawAsync(fnGetNewSupplierNumber.Create());

        //        //Stored Procedures:
        //        //await this.Database.ExecuteSqlRawAsync(spSearchCustomers.Drop());
        //        //await this.Database.ExecuteSqlRawAsync(spSearchCustomers.Create());
        //        //await this.Database.ExecuteSqlRawAsync(spSearchProducts.Drop());
        //        //await this.Database.ExecuteSqlRawAsync(spSearchProducts.Create());
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}

        public void BackupDatabase(string path)
        {
            string db = this.Database.GetDbConnection().Database;

            this.Database.ExecuteSqlInterpolated($"BACKUP DATABASE [{db}] TO DISK = N'{path}' WITH NOFORMAT, NOINIT;");

            //BACKUP DATABASE [SimpleX] 
            //TO  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\Backup\SimpleX.bak' 
            //WITH NOFORMAT, NOINIT,  NAME = N'SimpleX-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10

            //https://www.entityframeworktutorial.net/efcore/working-with-stored-procedure-in-ef-core.aspx
            //context.Database.ExecuteSqlCommand("CreateStudents @p0, @p1", parameters: new[] { "Bill", "Gates" });
        }

    }
}
