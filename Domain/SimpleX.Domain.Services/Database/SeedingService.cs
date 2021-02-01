using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Common.Enums;
using SimpleX.Core.Crypto;
using SimpleX.Data.Context;
using SimpleX.Data.Entities;
using SimpleX.Data.Entities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.Database
{
    public interface ISeedingService : IDisposable
    {
        void SeedDb();
        Task SeedDbAsync();
    }

    public class DebugSeedingService : SeedingService
    {
        public DebugSeedingService(
            IDbContextFactory<SimpleContext> contextFactory, 
            ILoggerFactory loggerFactory, 
            ICryptoService crypto) : base(contextFactory, loggerFactory, crypto)
        {

        }
    }

    public class SeedingService : ISeedingService
    {
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly ILogger _logger;
        private readonly ICryptoService _crypto;

        public SeedingService(IDbContextFactory<SimpleContext> contextFactory, ILoggerFactory loggerFactory, ICryptoService crypto)
        {
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _logger = loggerFactory?.CreateLogger<SeedingService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _crypto = crypto ??
                throw new ArgumentNullException(nameof(crypto));

            _logger.LogTrace("ContextFactory injected in SeedingService");
            _logger.LogTrace("LoggerFactory injected in SeedingService");
            _logger.LogTrace("CryptoService injected in SeedingService");

            _logger.LogTrace("Created SeedingService");
        }
                
        public void SeedDb()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    SeedUser(context);
                    SeedStore(context);
                    SeedWarehouse(context);
                    SeedDocumentLevel(context);
                    SeedCustomerDefaultValue(context);
                    SeedAppMenu(context);
                    SeedUnit(context);
                    SeedTaxPercentage(context);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task SeedDbAsync()
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    await SeedUserAsync(context);
                    await SeedStoreAsync(context);
                    await SeedWarehouseAsync(context);
                    await SeedDocumentLevelAsync(context);
                    await SeedCustomerDefaultValueAsync(context);
                    await SeedAppMenuAsync(context);
                    await SeedUnitAsync(context);
                    await SeedTaxPercentageAsync(context);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private void SeedStore(SimpleContext context)
        {
            //Default Store
            if (context.Store.FirstOrDefault(b => b.Id == (byte)1) == null)
            {
                context.Store.Add(new Store
                {
                    Id = 1,
                    Name = "Your company name..",
                    AddressID = null
                });
            }
        }
        private async Task SeedStoreAsync(SimpleContext context)
        {
            //Default Store
            if (await context.Store.FirstOrDefaultAsync(b => b.Id == (byte)1) == null)
            {
                context.Store.Add(new Store
                {
                    Id = 1,
                    Name = "Your company name..",
                    AddressID = null
                });
            }
        }

        private void SeedWarehouse(SimpleContext context)
        {
            //Default Warehouse
            if (context.Warehouse.FirstOrDefault(w => w.Id == (long)1) == null)
            {
                context.Warehouse.Add(new Warehouse
                {
                    Id = 1,
                    Name = "Your warehouse name.."
                });
            }
        }
        private async Task SeedWarehouseAsync(SimpleContext context)
        {
            //Default Warehouse
            if (await context.Warehouse.FirstOrDefaultAsync(w => w.Id == (long)1) == null)
            {
                context.Warehouse.Add(new Warehouse
                {
                    Id = 1,
                    Name = "Your warehouse name.."
                });
            }
        }

        private void SeedDocumentLevel(SimpleContext context)
        {
            try
            {
                if (context.DocumentLevel.FirstOrDefault(d => d.Journal == DocumentJournal.Sale && d.StoreID == 1 && d.LevelID == 1) == null)
                {
                    context.DocumentLevel.Add(new DocumentLevel
                    {
                        Journal = DocumentJournal.Sale,
                        StoreID = 1,
                        LevelID = 1,
                        Description = "Invoice"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        private async Task SeedDocumentLevelAsync(SimpleContext context)
        {
            try
            {
                if (await context.DocumentLevel.FirstOrDefaultAsync(d => d.Journal == DocumentJournal.Sale && d.StoreID == 1 && d.LevelID == 1) == null)
                {
                    context.DocumentLevel.Add(new DocumentLevel
                    {
                        Journal = DocumentJournal.Sale,
                        StoreID = 1,
                        LevelID = 1,
                        Description = "Invoice"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private void SeedTaxPercentage(SimpleContext context)
        {
            if (!context.TaxPercentage.Any())
            {
                TaxPercentage vatNone = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.None, Percentage = 0 };
                TaxPercentage vatLow = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.Low, Percentage = 6 };
                TaxPercentage vatHigh = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.High, Percentage = 21 };

                context.TaxPercentage.Add(vatNone);
                context.TaxPercentage.Add(vatLow);
                context.TaxPercentage.Add(vatHigh);
            }
        }
        private async Task SeedTaxPercentageAsync(SimpleContext context)
        {
            if (!await context.TaxPercentage.AnyAsync())
            {
                TaxPercentage vatNone = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.None, Percentage = 0 };
                TaxPercentage vatLow = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.Low, Percentage = 6 };
                TaxPercentage vatHigh = new TaxPercentage() { From = null, Until = null, Rate = TaxRate.High, Percentage = 21 };

                context.TaxPercentage.Add(vatNone);
                context.TaxPercentage.Add(vatLow);
                context.TaxPercentage.Add(vatHigh);
            }
        }

        private void SeedUser(SimpleContext context)
        {
            //Default User:
            if (context.AppUser.FirstOrDefault(u => u.Id == 1) == null)
                context.AppUser.Add(
                        new AppUser
                        {
                            Id = 1,
                            Username = "Admin",
                            Password = _crypto.Encrypt("Incorr3ct"),
                            LanguageCode = "nl",
                            StoreID = 1
                        });

            //Add all roles:
            foreach (string role in AppRole.All)
            {
                if (context.AppUserRole.FirstOrDefault(u => u.UserID == 1 && u.Role == role) == null)
                    context.AppUserRole.Add(
                        new AppUserRole
                        {
                            UserID = 1,
                            Role = role
                        });
            }

        }
        private async Task SeedUserAsync(SimpleContext context)
        {
            //Default User:
            if (await context.AppUser.FirstOrDefaultAsync(u => u.Id == 1) == null)
                context.AppUser.Add(
                        new AppUser
                        {
                            Id = 1,
                            Username = "Admin",
                            Password = _crypto.Encrypt("Incorr3ct"),
                            LanguageCode = "nl",
                            StoreID = 1
                        });

            //Add all roles:
            foreach (string role in AppRole.All)
            {
                if (await context.AppUserRole.FirstOrDefaultAsync(u => u.UserID == 1 && u.Role == role) == null)
                    context.AppUserRole.Add(
                        new AppUserRole
                        {
                            UserID = 1,
                            Role = role
                        });
            }

        }

        private void SeedCustomerDefaultValue(SimpleContext context)
        {
            if (context.DefaultValue.FirstOrDefault(d => d.EntityType == EntityType.Customer &&
                                                      d.PropertyName == "LanguageCode") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "LanguageCode",
                    PropertyValue = "nl",
                    PropertyType = "system.string",
                });
            }

            if (context.DefaultValue.FirstOrDefault(d => d.EntityType == EntityType.Customer &&
                                                      d.PropertyName == "CountryCode") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "CountryCode",
                    PropertyValue = "BE",
                    PropertyType = "system.string",
                });
            }

            if (context.DefaultValue.FirstOrDefault(d => d.EntityType == EntityType.Customer &&
                                                      d.PropertyName == "Currency") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "Currency",
                    PropertyValue = "EUR",
                    PropertyType = "system.string",
                });
            }
        }
        private async Task SeedCustomerDefaultValueAsync(SimpleContext context)
        {
            if (await context.DefaultValue.FirstOrDefaultAsync(d => d.EntityType == EntityType.Customer &&
                                                                    d.PropertyName == "LanguageCode") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "LanguageCode",
                    PropertyValue = "nl",
                    PropertyType = "system.string",
                });
            }

            if (await context.DefaultValue.FirstOrDefaultAsync(d => d.EntityType == EntityType.Customer &&
                                                                    d.PropertyName == "CountryCode") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "CountryCode",
                    PropertyValue = "BE",
                    PropertyType = "system.string",
                });
            }

            if (await context.DefaultValue.FirstOrDefaultAsync(d => d.EntityType == EntityType.Customer &&
                                                                    d.PropertyName == "Currency") == null)
            {
                context.DefaultValue.Add(new DefaultValue()
                {
                    EntityType = EntityType.Customer,
                    PropertyName = "Currency",
                    PropertyValue = "EUR",
                    PropertyType = "system.string",
                });
            }
        }

        private void SeedAppMenu(SimpleContext context)
        {
            if (context.AppMenu.Find((long)000001) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 000001,
                    ParentID = null,
                    Name = "acgFavorites",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Favorieten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Favorites"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)100000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100000,
                    ParentID = null,
                    Name = "acgRelations",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Relaties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Relations"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)100100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100100,
                    ParentID = null,
                    Name = "aceCustomers",
                    View = "CustomerView",
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Klanten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Customers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)100200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100200,
                    ParentID = null,
                    Name = "aceSuppliers",
                    View = "SupplierView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Leveranciers"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Suppliers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)100300) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100300,
                    ParentID = null,
                    Name = "aceEmployees",
                    View = "EmployeeView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Werknemers"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Employees"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)200000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200000,
                    ParentID = null,
                    Name = "acgProducts",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Producten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Products"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)200100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200100,
                    ParentID = null,
                    Name = "aceProducts",
                    View = "ProductView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Producten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Products"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)200200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200200,
                    ParentID = null,
                    Name = "aceInventory",
                    View = "InventoryView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Inventaris"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Inventory"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300000,
                    ParentID = null,
                    Name = "acgBilling",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Facturatie"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Billing"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300100,
                    ParentID = null,
                    Name = "aceInvoices",
                    View = "InvoiceView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Facturen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Invoices"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300200,
                    ParentID = null,
                    Name = "aceDeliveries",
                    View = "DeliveryView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Leveringen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Deliveries"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300300) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300300,
                    ParentID = null,
                    Name = "aceReservations",
                    View = "ReservationView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Reservaties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Reservations"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300400) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300400,
                    ParentID = null,
                    Name = "aceOffers",
                    View = "OfferView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Offertes"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Offers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300800) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300800,
                    ParentID = null,
                    Name = "aceReceptions",
                    View = "ReceptionView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Recepties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Receptions"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)300900) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300900,
                    ParentID = null,
                    Name = "aceOrders",
                    View = "OrderView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Bestellingen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Orders"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)400000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400000,
                    ParentID = null,
                    Name = "acgScheduler",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Planning"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Scheduler"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)400100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400100,
                    ParentID = null,
                    Name = "aceAgenda",
                    View = "AgendaView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Agenda"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Agenda"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)400200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400200,
                    ParentID = null,
                    Name = "aceTasks",
                    View = "TaskView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Taken"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Tasks"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.AppMenu.Find((long)900000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 900000,
                    ParentID = null,
                    Name = "acgAccountancy",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Boekhouding"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Accountancy"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
        }
        private async Task SeedAppMenuAsync(SimpleContext context)
        {
            if (await context.AppMenu.FindAsync((long)000001) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 000001,
                    ParentID = null,
                    Name = "acgFavorites",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Favorieten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Favorites"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)100000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100000,
                    ParentID = null,
                    Name = "acgRelations",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Relaties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Relations"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)100100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100100,
                    ParentID = null,
                    Name = "aceCustomers",
                    View = "CustomerView",
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Klanten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Customers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)100200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100200,
                    ParentID = null,
                    Name = "aceSuppliers",
                    View = "SupplierView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Leveranciers"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Suppliers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)100300) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 100300,
                    ParentID = null,
                    Name = "aceEmployees",
                    View = "EmployeeView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Werknemers"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Employees"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)200000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200000,
                    ParentID = null,
                    Name = "acgProducts",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Producten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Products"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)200100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200100,
                    ParentID = null,
                    Name = "aceProducts",
                    View = "ProductView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Producten"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Products"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)200200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 200200,
                    ParentID = null,
                    Name = "aceInventory",
                    View = "InventoryView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Inventaris"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Inventory"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300000,
                    ParentID = null,
                    Name = "acgBilling",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Facturatie"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Billing"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300100,
                    ParentID = null,
                    Name = "aceInvoices",
                    View = "InvoiceView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Facturen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Invoices"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300200,
                    ParentID = null,
                    Name = "aceDeliveries",
                    View = "DeliveryView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Leveringen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Deliveries"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300300) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300300,
                    ParentID = null,
                    Name = "aceReservations",
                    View = "ReservationView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Reservaties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Reservations"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300400) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300400,
                    ParentID = null,
                    Name = "aceOffers",
                    View = "OfferView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Offertes"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Offers"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300800) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300800,
                    ParentID = null,
                    Name = "aceReceptions",
                    View = "ReceptionView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Recepties"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Receptions"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)300900) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 300900,
                    ParentID = null,
                    Name = "aceOrders",
                    View = "OrderView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Bestellingen"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Orders"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)400000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400000,
                    ParentID = null,
                    Name = "acgScheduler",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Planning"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Scheduler"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)400100) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400100,
                    ParentID = null,
                    Name = "aceAgenda",
                    View = "AgendaView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Agenda"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Agenda"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)400200) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 400200,
                    ParentID = null,
                    Name = "aceTasks",
                    View = "TaskView",
                    IsActive = false,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Taken"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Tasks"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.AppMenu.FindAsync((long)900000) == null)
                context.AppMenu.Add(new AppMenu
                {
                    Id = 900000,
                    ParentID = null,
                    Name = "acgAccountancy",
                    View = null,
                    IsActive = true,
                    Translations = new List<AppMenuTranslation>
                    {
                        new AppMenuTranslation {LanguageCode = "nl", Name = "Boekhouding"},
                        new AppMenuTranslation {LanguageCode = "fr", Name = ""},
                        new AppMenuTranslation {LanguageCode = "en", Name = "Accountancy"},
                        new AppMenuTranslation {LanguageCode = "de", Name = ""}
                    }
                });
        }

        private void SeedUnit(SimpleContext context)
        {
            if ( context.Unit.Find((long)1) == null)
                context.Unit.Add(new Unit
                {
                    Id = 1,
                    UnitCode = "p",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Stuk"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Piece"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if ( context.Unit.Find((long)2) == null)
                context.Unit.Add(new Unit
                {
                    Id = 2,
                    UnitCode = "kg",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Kilogram"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Kilogram"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if ( context.Unit.Find((long)3) == null)
                context.Unit.Add(new Unit
                {
                    Id = 3,
                    UnitCode = "m",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Meter"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Meter"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.Unit.Find((long)4) == null)
                context.Unit.Add(new Unit
                {
                    Id = 4,
                    UnitCode = "l",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Liter"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Liter"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.Unit.Find((long)5) == null)
                context.Unit.Add(new Unit
                {
                    Id = 5,
                    UnitCode = "box",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Doos"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Box"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (context.Unit.Find((long)6) == null)
                context.Unit.Add(new Unit
                {
                    Id = 6,
                    UnitCode = "plt",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Pallet"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Pallet"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
        }
        private async Task SeedUnitAsync(SimpleContext context)
        {
            if (await context.Unit.FindAsync((long)1) == null)
                context.Unit.Add(new Unit
                {
                    Id = 1,
                    UnitCode = "p",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Stuk"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Piece"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.Unit.FindAsync((long)2) == null)
                context.Unit.Add(new Unit
                {
                    Id = 2,
                    UnitCode = "kg",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Kilogram"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Kilogram"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.Unit.FindAsync((long)3) == null)
                context.Unit.Add(new Unit
                {
                    Id = 3,
                    UnitCode = "m",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Meter"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Meter"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.Unit.FindAsync((long)4) == null)
                context.Unit.Add(new Unit
                {
                    Id = 4,
                    UnitCode = "l",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Liter"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Liter"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.Unit.FindAsync((long)5) == null)
                context.Unit.Add(new Unit
                {
                    Id = 5,
                    UnitCode = "box",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Doos"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Box"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
            if (await context.Unit.FindAsync((long)6) == null)
                context.Unit.Add(new Unit
                {
                    Id = 6,
                    UnitCode = "plt",
                    Translations = new List<UnitTranslation>
                    {
                        new UnitTranslation {LanguageCode = "nl", Name = "Pallet"},
                        new UnitTranslation {LanguageCode = "fr", Name = ""},
                        new UnitTranslation {LanguageCode = "en", Name = "Pallet"},
                        new UnitTranslation {LanguageCode = "de", Name = ""}
                    }
                });
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
