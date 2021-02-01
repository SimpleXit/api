using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Common.Enums;
using SimpleX.Data.Entities;
using SimpleX.Data.Entities.App;
using SimpleX.Data.Entities.Assets;
using System;
using System.Linq;

namespace SimpleX.Data.Context
{
    public partial class SimpleContext : DbContext, IDisposable
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogTrace($"DataContext.OnModelCreating() {0}", _guid);

            modelBuilder.EnableAutoHistory<DataHistory>(o => { });

            //CreateViewModel(modelBuilder);

            CreateAppMenuModel(modelBuilder);
            CreateAppMenuPermissionModel(modelBuilder);
            CreateAppSettingsModel(modelBuilder);
            CreateAppTaskModel(modelBuilder);
            CreateAppTaskLogModel(modelBuilder);
            CreateAppUserModel(modelBuilder);
            CreateAppUserRoleModel(modelBuilder);

            CreateAddressModel(modelBuilder);
            CreateAppointmentModel(modelBuilder);
            CreateAttachmentModel(modelBuilder);
            CreateStoreModel(modelBuilder);
            CreateCategoryModel(modelBuilder);
            CreateCommunicationModel(modelBuilder);
            CreateContactInfoModel(modelBuilder);
            CreateCustomerModel(modelBuilder);
            CreateDefaultValueModel(modelBuilder);
            CreateDocumentModel(modelBuilder);
            CreateDocumentAssociateModel(modelBuilder);
            CreateDocumentTaxModel(modelBuilder);
            CreateDocumentSettingsModel(modelBuilder);
            CreateDocumentLineModel(modelBuilder);
            CreateDocumentLineDataModel(modelBuilder);
            CreateDocumentPaymentModel(modelBuilder);
            CreateDocumentTree(modelBuilder);
            CreateEmployeeModel(modelBuilder);
            CreateNoteModel(modelBuilder);
            CreatePersonModel(modelBuilder);
            CreateProductModel(modelBuilder);
            CreateProductAttributeModel(modelBuilder);
            CreateProductChargeModel(modelBuilder);
            CreateProductMovementModel(modelBuilder);
            CreateProductRelationModel(modelBuilder);
            CreateProductSupplierModel(modelBuilder);
            CreateProductStockModel(modelBuilder);
            CreateProductUnitModel(modelBuilder);
            CreateProjectModel(modelBuilder);
            CreateSupplierModel(modelBuilder);
            CreateTranslationModel(modelBuilder);
            CreateUnitModel(modelBuilder);

            CreateWarehouseModel(modelBuilder);
            CreateVatPercentageModel(modelBuilder);

            //Set all decimal properties to ColumnType decimal(18,6):
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                       .SelectMany(t => t.GetProperties())
                                                       .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18, 6)");
            }

        }

        private void CreateAddressModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Address>().Property(a => a.Name).HasMaxLength(250);
            modelBuilder.Entity<Address>().Property(a => a.StreetAndNumber).HasMaxLength(250);
            modelBuilder.Entity<Address>().Property(a => a.ZipCode).HasMaxLength(25);
            modelBuilder.Entity<Address>().Property(a => a.City).HasMaxLength(100);
            modelBuilder.Entity<Address>().Property(a => a.CountryCode).HasMaxLength(25);

            modelBuilder.Entity<Address>()
                        .HasDiscriminator<EntityType>("OwnerType")
                        .HasValue<Address>(0)
                        .HasValue<CustomerAddress>(EntityType.Customer)
                        .HasValue<DocumentAssociateAddress>(EntityType.DocumentAssociate)
                        .HasValue<EmployeeAddress>(EntityType.Employee)
                        .HasValue<PersonAddress>(EntityType.Person)
                        .HasValue<SupplierAddress>(EntityType.Supplier);

            modelBuilder.Entity<Address>()
                        .HasIndex(a => a.ZipCode);
            modelBuilder.Entity<Address>()
                        .HasIndex(a => a.City);
        }
        private void CreateAppSettingsModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>().HasKey(e => e.Id);
            modelBuilder.Entity<AppSetting>().Property(e => e.Id).ValueGeneratedNever();    //No Identity

            modelBuilder.Entity<AppSetting>().Property(e => e.Category).HasMaxLength(100);
            modelBuilder.Entity<AppSetting>().Property(e => e.Caption).HasMaxLength(100);
            modelBuilder.Entity<AppSetting>().Property(e => e.Description).HasMaxLength(1000);
            modelBuilder.Entity<AppSetting>().Property(e => e.DefaultValue).HasMaxLength(250);
            modelBuilder.Entity<AppSetting>().Property(e => e.CurrentValue).HasMaxLength(250);
            modelBuilder.Entity<AppSetting>().Property(e => e.TypeOfValue).HasMaxLength(25);
            modelBuilder.Entity<AppSetting>().Property(e => e.ItemSource).HasMaxLength(1000);
        }
        private void CreateAppMenuModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppMenu>().HasKey(m => m.Id);
            modelBuilder.Entity<AppMenu>().Property(m => m.Id).ValueGeneratedNever();    //No Identity

            modelBuilder.Entity<AppMenu>().Property(m => m.Name).HasMaxLength(50);
            modelBuilder.Entity<AppMenu>().Property(m => m.View).HasMaxLength(250);
            modelBuilder.Entity<AppMenu>().Property(m => m.Args).HasMaxLength(250);

            modelBuilder.Entity<AppMenu>()
                        .HasMany(m => m.SubMenus)
                        .WithOne()
                        .HasForeignKey(m => m.ParentID)
                        .HasConstraintName("FK_SubMenu_Menu")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppMenu>()
                        .HasMany(t => t.Translations)
                        .WithOne()
                        .HasForeignKey(m => m.MenuID)
                        .HasConstraintName("FK_Translation_Menu")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppMenu>()
                        .HasIndex(m => m.ParentID);
            modelBuilder.Entity<AppMenu>()
                        .HasIndex(m => m.Name)
                        .IsUnique(true);
        }
        private void CreateAppTaskModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTask>().HasKey(t => t.FullName);
            modelBuilder.Entity<AppTask>().Property(t => t.FullName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<AppTask>().Property(t => t.DisplayName).HasMaxLength(50);
            modelBuilder.Entity<AppTask>().Property(t => t.Description).HasMaxLength(500);
   
        }
        private void CreateAppTaskLogModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTaskLog>().HasKey(t => t.FullName);
            modelBuilder.Entity<AppTaskLog>().Property(t => t.Log).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<AppTaskLog>().Property(t => t.Log).HasMaxLength(2500);
        }
        private void CreateAppMenuPermissionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppPermission>().HasKey(m => m.Id);

            modelBuilder.Entity<AppPermission>().Property(m => m.Role).HasMaxLength(20).IsRequired();
            modelBuilder.Entity<AppPermission>().Property(m => m.View).HasMaxLength(250).IsRequired();
            modelBuilder.Entity<AppPermission>().Property(m => m.Action).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<AppPermission>()
                .HasIndex(m => m.View);
        }
        private void CreateAppUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
            modelBuilder.Entity<AppUser>().Property(u => u.Id).ValueGeneratedNever();      //No Identity
            modelBuilder.Entity<AppUser>().Property(u => u.Username).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AppUser>().Property(u => u.Password).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<AppUser>().Property(u => u.FirstName).HasMaxLength(100);
            modelBuilder.Entity<AppUser>().Property(u => u.LastName).HasMaxLength(100);
            modelBuilder.Entity<AppUser>().Property(u => u.LanguageCode).HasMaxLength(25);
            modelBuilder.Entity<AppUser>().Property(u => u.Mail).HasMaxLength(250);

            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(r => r.UserID)
                .HasConstraintName("FK_Role_AppUser")
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AppUser>()
                        .HasIndex(u => u.Username)
                        .IsUnique();
            modelBuilder.Entity<AppUser>()
                        .HasIndex(u => new { u.Username, u.Password })
                        .IsUnique();
        }
        private void CreateAppUserRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserRole>().HasKey(u => u.Id);
            modelBuilder.Entity<AppUserRole>().Property(u => u.UserID).IsRequired();
            modelBuilder.Entity<AppUserRole>().Property(u => u.Role).HasMaxLength(20).IsRequired();

            modelBuilder.Entity<AppUserRole>()
                .HasIndex(u => u.UserID);
                
            modelBuilder.Entity<AppUserRole>()
                .HasIndex(u => new { u.UserID, u.Role })
                .IsUnique();
        }
        private void CreateAppointmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasKey(m => m.Id);

            modelBuilder.Entity<Appointment>().Property(a => a.Subject).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Appointment>().Property(a => a.Description).HasMaxLength(2500);
            modelBuilder.Entity<Appointment>().Property(a => a.Location).HasMaxLength(200);

            modelBuilder.Entity<Appointment>()
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey(a => a.CategoryID)
                        .HasConstraintName("FK_Appointment_Category")
                        .OnDelete(DeleteBehavior.Restrict); ;
            modelBuilder.Entity<Appointment>()
                        .HasMany(n => n.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.AppointmentID)
                        .HasConstraintName("FK_Note_Appointment")
                        .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Appointment>()
                        .HasOne(a => a.Customer)
                        .WithMany(c => c.Appointments)
                        .HasForeignKey(a => a.CustomerID)
                        .HasConstraintName("FK_Appointment_Customer");
            modelBuilder.Entity<Appointment>()
                        .HasOne(a => a.Employee)
                        .WithMany(e => e.Appointments)
                        .HasForeignKey(a => a.EmployeeID)
                        .HasConstraintName("FK_Appointment_Employee");
            modelBuilder.Entity<Appointment>()
                        .HasOne(a => a.Project)
                        .WithMany(p => p.Appointments)
                        .HasForeignKey(a => a.ProjectID)
                        .HasConstraintName("FK_Appointment_Project");

            modelBuilder.Entity<Appointment>()
                        .HasIndex(a => a.Subject);
            modelBuilder.Entity<Appointment>()
                        .HasIndex(a => a.StatusID);
            modelBuilder.Entity<Appointment>()
                        .HasIndex(a => new { a.Start, a.End });

        }
        private void CreateAttachmentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>().HasKey(a => a.Id);

            modelBuilder.Entity<Attachment>().Property(a => a.Title).HasMaxLength(200);
            modelBuilder.Entity<Attachment>().Property(a => a.Description).HasMaxLength(2500);
            modelBuilder.Entity<Attachment>().Property(a => a.FileName).HasMaxLength(100);

            modelBuilder.Entity<Attachment>()
                        .HasDiscriminator<EntityType>("OwerType")
                        .HasValue<Attachment>(0)
                        .HasValue<CustomerAttachment>(EntityType.Customer)
                        .HasValue<SupplierAttachment>(EntityType.Supplier)
                        .HasValue<DocumentAttachment>(EntityType.Document)
                        .HasValue<EmployeeAttachment>(EntityType.Employee)
                        .HasValue<ProductAttachment>(EntityType.Product)
                        .HasValue<ProjectAttachment>(EntityType.Project);
        }
        private void CreateStoreModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().HasKey(b => b.Id);
            modelBuilder.Entity<Store>().Property(b => b.Id).ValueGeneratedNever();

            modelBuilder.Entity<Store>().Property(b => b.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Store>().Property(b => b.TaxNumber).HasMaxLength(25);
            modelBuilder.Entity<Store>().Property(b => b.IBAN).HasMaxLength(50);
            modelBuilder.Entity<Store>().Property(b => b.BIC).HasMaxLength(25);

            modelBuilder.Entity<Store>()
                        .HasOne(b => b.Address)
                        .WithOne()
                        .HasForeignKey<Store>(a => a.AddressID);

            modelBuilder.Entity<Store>()
                        .HasOne(c => c.ContactInfo)
                        .WithOne()
                        .HasForeignKey<Store>(c => c.ContactInfoID);
        }
        private void CreateCategoryModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            modelBuilder.Entity<Category>().Property(c => c.CategoryCode).HasMaxLength(25).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(200);
            modelBuilder.Entity<Category>().Property(c => c.Description).HasMaxLength(2500);

            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.OwnerType);


            modelBuilder.Entity<Category>()
                        .HasMany(t => t.Translations)
                        .WithOne()
                        .HasForeignKey(t => t.CategoryID)
                        .HasConstraintName("FK_Translation_Category")
                        .OnDelete(DeleteBehavior.NoAction);
        }
        private void CreateCommunicationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Communication>().HasKey(c => c.Id);

            modelBuilder.Entity<Communication>().Property(c => c.Description).HasMaxLength(500);
            modelBuilder.Entity<Communication>().Property(c => c.Remarks).HasMaxLength(500);
            modelBuilder.Entity<Communication>().Property(c => c.Correspondents).HasMaxLength(500);

            modelBuilder.Entity<Communication>()
                        .HasDiscriminator<EntityType>("OwerType")
                        .HasValue<Communication>(0)
                        .HasValue<CustomerCommunication>(EntityType.Customer)
                        .HasValue<SupplierCommunication>(EntityType.Supplier);

            modelBuilder.Entity<Communication>()
                        .HasOne(c => c.Employee)
                        .WithMany(e => e.Communications)
                        .HasForeignKey(c => c.EmployeeID)
                        .HasConstraintName("FK_Communication_Employee")
                        .OnDelete(DeleteBehavior.NoAction);
        }
        private void CreateContactInfoModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactInfo>().HasKey(c => c.Id);
            modelBuilder.Entity<ContactInfo>().Property(c => c.Mail).HasMaxLength(250);
            modelBuilder.Entity<ContactInfo>().Property(c => c.Tel).HasMaxLength(100);
            modelBuilder.Entity<ContactInfo>().Property(c => c.Mob).HasMaxLength(100);
            modelBuilder.Entity<ContactInfo>().Property(c => c.Fax).HasMaxLength(100);
            modelBuilder.Entity<ContactInfo>().Property(c => c.Web).HasMaxLength(100);

            modelBuilder.Entity<ContactInfo>()
            .HasDiscriminator<EntityType>("OwnerType")
            .HasValue<ContactInfo>(0)
            .HasValue<CustomerContactInfo>(EntityType.Customer)
            .HasValue<DocumentAssociateContactInfo>(EntityType.DocumentAssociate)
            .HasValue<EmployeeContactInfo>(EntityType.Employee)
            .HasValue<PersonContactInfo>(EntityType.Person)
            .HasValue<SupplierContactInfo>(EntityType.Supplier);
        }

        private void CreateCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().Property(c => c.Number).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.ShortName).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.Prefix).HasMaxLength(25);
            modelBuilder.Entity<Customer>().Property(c => c.Suffix).HasMaxLength(25);
            modelBuilder.Entity<Customer>().Property(c => c.LanguageCode).HasMaxLength(3).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.TaxNumber).HasMaxLength(25);
            modelBuilder.Entity<Customer>().Property(c => c.IBAN).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(c => c.BIC).HasMaxLength(25);
            modelBuilder.Entity<Customer>().Property(c => c.Currency).HasMaxLength(3).IsRequired(); ;

            modelBuilder.Entity<Customer>()
                       .HasOne(c => c.ContactInfo)
                       .WithOne()
                       .HasForeignKey<CustomerContactInfo>(c => c.CustomerID)
                       .HasConstraintName("FK_Contact_Customer")
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey(c => c.CategoryID)
                        .HasConstraintName("FK_Customer_Category")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Addresses)
                        .WithOne()
                        .HasForeignKey(c => c.CustomerID)
                        .HasConstraintName("FK_Address_Customer")
                        .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Communications)
                        .WithOne()
                        .HasForeignKey(c => c.CustomerID)
                        .HasConstraintName("FK_Communication_Customer")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.CustomerID)
                        .HasConstraintName("FK_Note_Customer")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                        .HasMany(s => s.Persons)
                        .WithOne()
                        .HasForeignKey(p => p.CustomerID)
                        .HasConstraintName("FK_Person_Customer")
                        .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Customer>()
                        .HasMany(c => c.Attachments)
                        .WithOne()
                        .HasForeignKey(a => a.CustomerID)
                        .HasConstraintName("FK_Attachment_Customer")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.Number)
                        .IsUnique();
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => new { c.FirstName, c.LastName });
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => new { c.LastName, c.FirstName });
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.ShortName);
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.TaxNumber);
            modelBuilder.Entity<Customer>()
                        .HasIndex(c => c.IsDeleted);
        }
        private void CreateDefaultValueModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DefaultValue>().HasKey(d => d.Id);

            modelBuilder.Entity<DefaultValue>().Property(d => d.PropertyName).HasMaxLength(100);
            modelBuilder.Entity<DefaultValue>().Property(d => d.PropertyValue).HasMaxLength(250);
            modelBuilder.Entity<DefaultValue>().Property(d => d.PropertyType).HasMaxLength(50);

            modelBuilder.Entity<DefaultValue>()
                        .HasIndex(d => d.EntityType);

            modelBuilder.Entity<DefaultValue>()
                        .HasIndex(d => new { d.EntityType, d.PropertyName })
                        .IsUnique(true);
        }

        private void CreateDocumentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().HasKey(d => d.Id);

            modelBuilder.Entity<Document>().Property(d => d.Reference).HasMaxLength(50);
            modelBuilder.Entity<Document>().Property(d => d.Remarks).HasMaxLength(500);
            modelBuilder.Entity<Document>().Property(d => d.Remarks).HasMaxLength(500);

            modelBuilder.Entity<Document>().Property(d => d.Currency).HasMaxLength(3);

            //Indexes
            modelBuilder.Entity<Document>()
                        .HasIndex(d => new { d.DocumentJournal, d.DocumentStoreID, d.DocumentLevelID, d.DocumentSourceID, d.DocumentNumber })
                        .IsUnique(true);

            modelBuilder.Entity<Document>()
                        .HasIndex(d => new { d.DocumentJournal, d.AssociateID });

            modelBuilder.Entity<Document>()
                        .HasIndex(d => new { d.DocumentJournal, d.DocumentStoreID, d.DocumentLevelID, d.DocumentSourceID });

            //Foreign Keys
            modelBuilder.Entity<Document>()
                        .HasMany(d => d.DocumentTaxes)
                        .WithOne()
                        .HasForeignKey(v => v.DocumentID);

            modelBuilder.Entity<Document>()
                        .HasMany(d => d.DocumentLines)
                        .WithOne(l => l.Document)
                        .HasForeignKey(l => l.DocumentID);

            modelBuilder.Entity<Document>()
                        .HasOne(d => d.Associate)
                        .WithOne()
                        .HasForeignKey<Document>(d => d.AssociateID)
                        .HasConstraintName("FK_Document_Associate");

            modelBuilder.Entity<Document>()
                        .HasOne(d => d.DeliveryAddress)
                        .WithOne()
                        .HasForeignKey<Document>(d => d.DeliveryAddressID)
                        .HasConstraintName("FK_Document_DeliveryAddress");

            modelBuilder.Entity<Document>()
                        .HasOne(d => d.InvoiceAddres)
                        .WithOne()
                        .HasForeignKey<Document>(d => d.InvoiceAddressID)
                        .HasConstraintName("FK_Document_InvoiceAddress");

            modelBuilder.Entity<Document>()
                        .HasMany(d => d.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.DocumentID)
                        .HasConstraintName("FK_Note_Document");

            modelBuilder.Entity<Document>()
                        .HasMany(d => d.Attachments)
                        .WithOne()
                        .HasForeignKey(n => n.DocumentID)
                        .HasConstraintName("FK_Attachment_Document")
                        .OnDelete(DeleteBehavior.NoAction);
        }
        private void CreateDocumentAssociateModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentAssociate>().Property(d => d.Number).HasMaxLength(25).IsRequired();
            modelBuilder.Entity<DocumentAssociate>().Property(d => d.FirstName).HasMaxLength(100);
            modelBuilder.Entity<DocumentAssociate>().Property(d => d.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<DocumentAssociate>().Property(s => s.Prefix).HasMaxLength(25);
            modelBuilder.Entity<DocumentAssociate>().Property(s => s.Suffix).HasMaxLength(25);
            modelBuilder.Entity<DocumentAssociate>().Property(d => d.LanguageCode).HasMaxLength(25);
            modelBuilder.Entity<DocumentAssociate>().Property(d => d.TaxNumber).HasMaxLength(25);

            modelBuilder.Entity<DocumentAssociate>()
                        .HasOne(d => d.Address)
                        .WithOne()
                        .HasForeignKey<DocumentAssociateAddress>(d => d.DocumentAssociateID)
                        .HasConstraintName("FK_Address_DocumentAssociate");

            modelBuilder.Entity<DocumentAssociate>()
                        .HasOne(d => d.ContactInfo)
                        .WithOne()
                        .HasForeignKey<DocumentAssociateContactInfo>(d => d.DocumentAssociateID)
                        .HasConstraintName("FK_ContactInfo_DocumentAssociate");

            modelBuilder.Entity<DocumentAssociate>()
                        .HasOne(d => d.Customer)
                        .WithOne()
                        .HasForeignKey<DocumentAssociate>(d => d.CustomerID)
                        .HasConstraintName("FK_DocumentAssociate_Customer");

            modelBuilder.Entity<DocumentAssociate>()
                        .HasOne(d => d.Supplier)
                        .WithOne()
                        .HasForeignKey<DocumentAssociate>(d => d.SupplierID)
                        .HasConstraintName("FK_DocumentAssociate_Supplier");
        }
        private void CreateDocumentTaxModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentTax>().HasKey(d => d.Id);

            modelBuilder.Entity<DocumentTax>()
                        .HasIndex(d => new { d.DocumentID, d.TaxRate })
                        .IsUnique(true);

        }
        private void CreateDocumentSettingsModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentNumber>().HasKey(dn => new { dn.Year, dn.Journal, dn.StoreID, dn.LevelID, dn.SourceID });

            modelBuilder.Entity<DocumentSource>().HasKey(ds => new { ds.Journal, ds.StoreID, ds.LevelID, ds.SourceID });
            modelBuilder.Entity<DocumentSource>().Property(ds => ds.Description).HasMaxLength(50);

            modelBuilder.Entity<DocumentLevel>().HasKey(dl => new { dl.Journal, dl.StoreID, dl.LevelID });
            modelBuilder.Entity<DocumentLevel>().Property(dl => dl.Description).HasMaxLength(50);

            modelBuilder.Entity<DocumentDefaultTree>().HasKey(dt => new { dt.Journal, dt.StoreID, dt.ParentLevelID, dt.ParentSourceID, dt.Sequence });
        }
        private void CreateDocumentLineModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentLine>().HasKey(d => d.Id);
            //TODO



            modelBuilder.Entity<DocumentLine>()
                        .HasOne(d => d.DocumentLineData)
                        .WithMany()
                        .HasForeignKey(d => d.LineDataID);
        }
        private void CreateDocumentLineDataModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentLineData>().HasKey(d => d.Id);

            modelBuilder.Entity<DocumentLineData>().Property(p => p.ProductNumber).HasMaxLength(50);
            modelBuilder.Entity<DocumentLineData>().Property(p => p.ProductName).HasMaxLength(200);
            modelBuilder.Entity<DocumentLineData>().Property(p => p.ProductDescription).HasMaxLength(2500);
            //TODO
        }
        private void CreateDocumentPaymentModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentPayment>().HasKey(d => d.Id);

            modelBuilder.Entity<DocumentPayment>()
                        .HasOne(dp => dp.Document)
                        .WithMany(d => d.Payments)
                        .HasForeignKey(dp => dp.DocumentID)
                        .HasConstraintName("FK_DocumentPayment_Document");
        }
        private void CreateDocumentTree(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentTree>()
                        .HasKey(t => t.Id);

            modelBuilder.Entity<DocumentTree>()
                        .HasIndex(t => new { t.DocumentFromID, t.DocumentToID })
                        .IsUnique(true);

            modelBuilder.Entity<DocumentTree>()
                        .HasOne(dt => dt.DocumentFrom)
                        .WithMany(d => d.ParentDocuments)
                        .HasForeignKey(pt => pt.DocumentFromID)
                        .HasConstraintName("DocumentTree_DocumentFrom")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocumentTree>()
                        .HasOne(dt => dt.DocumentTo)
                        .WithMany(d => d.ChildDocuments)
                        .HasForeignKey(dt => dt.DocumentToID)
                        .HasConstraintName("DocumentTree_DocumentTo")
                        .OnDelete(DeleteBehavior.Restrict);
        }

        private void CreateEmployeeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);

            modelBuilder.Entity<Employee>().Property(e => e.Number).HasMaxLength(25).IsRequired();
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasMaxLength(100).IsRequired(true);
            modelBuilder.Entity<Employee>().Property(e => e.LastName).HasMaxLength(100).IsRequired(true);
            modelBuilder.Entity<Employee>().Property(e => e.Nationality).HasMaxLength(25);
            modelBuilder.Entity<Employee>().Property(e => e.NationalNumber).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(e => e.IdentityCardNumber).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(e => e.BirthPlace).HasMaxLength(100);
            modelBuilder.Entity<Employee>().Property(e => e.LanguageCode).HasMaxLength(25);
            modelBuilder.Entity<Employee>().Property(e => e.IBAN).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(e => e.BIC).HasMaxLength(25);

            modelBuilder.Entity<Employee>()
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey(e => e.CategoryID)
                        .HasConstraintName("FK_Employee_Category")
                        .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<Employee>()
                        .HasOne(e => e.ContactInfo)
                        .WithOne()
                        .HasForeignKey<EmployeeContactInfo>(e => e.EmployeeID)
                        .HasConstraintName("FK_Contact_Employee")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.Addresses)
                        .WithOne()
                        .HasForeignKey(n => n.EmployeeID)
                        .HasConstraintName("FK_Address_Employee")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.EmployeeID)
                        .HasConstraintName("FK_Note_Employee")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.Attachments)
                        .WithOne()
                        .HasForeignKey(n => n.EmployeeID)
                        .HasConstraintName("FK_Attachment_Employee")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                        .HasMany(e => e.Persons)
                        .WithOne()
                        .HasForeignKey(n => n.EmployeeID)
                        .HasConstraintName("FK_Person_Employee")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                        .HasIndex(e => new { e.FirstName, e.LastName });
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => new { e.LastName, e.FirstName });
        }

        private void CreateNoteModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasKey(n => n.Id);

            modelBuilder.Entity<Note>().Property(n => n.Title).HasMaxLength(200);
            modelBuilder.Entity<Note>().Property(n => n.Description).HasMaxLength(2500);

            modelBuilder.Entity<Note>()
                        .HasDiscriminator<EntityType>("OwerType")
                        .HasValue<Note>(0)
                        .HasValue<AppointmentNote>(EntityType.Appointment)
                        .HasValue<CustomerNote>(EntityType.Customer)
                        .HasValue<SupplierNote>(EntityType.Supplier)
                        .HasValue<DocumentNote>(EntityType.Document)
                        .HasValue<EmployeeNote>(EntityType.Employee)
                        .HasValue<ProductNote>(EntityType.Product)
                        .HasValue<ProjectNote>(EntityType.Project);
        }
        private void CreatePersonModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
            modelBuilder.Entity<Person>().Property(p => p.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Person>().Property(p => p.LastName).HasMaxLength(100);
            modelBuilder.Entity<Person>().Property(c => c.Prefix).HasMaxLength(25);
            modelBuilder.Entity<Person>().Property(c => c.Suffix).HasMaxLength(25);
            modelBuilder.Entity<Person>().Property(c => c.Function).HasMaxLength(50);
            modelBuilder.Entity<Person>().Property(p => p.LanguageCode).HasMaxLength(25);

            modelBuilder.Entity<Person>()
                        .HasDiscriminator<EntityType>("OwerType")
                        .HasValue<Person>(0)
                        .HasValue<CustomerPerson>(EntityType.Customer)
                        .HasValue<SupplierPerson>(EntityType.Supplier)
                        .HasValue<EmployeePerson>(EntityType.Person);

            modelBuilder.Entity<Person>()
                       .HasOne(c => c.ContactInfo)
                       .WithOne()
                       .HasForeignKey<PersonContactInfo>(c => c.PersonID)
                       .HasConstraintName("FK_Person_Contact");

            modelBuilder.Entity<Person>()
                       .HasOne(c => c.Address)
                       .WithOne()
                       .HasForeignKey<PersonAddress>(c => c.PersonID)
                       .HasConstraintName("FK_Person_Address");
        }

        private void CreateProductModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);

            modelBuilder.Entity<Product>().Property(p => p.ProductNumber).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.ProductEAN).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.ShortName).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.SupplierProductRef).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.BrandProductRef).HasMaxLength(50);

            modelBuilder.Entity<Product>()
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey(p => p.CategoryID)
                        .HasConstraintName("FK_Product_Category")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                        .HasMany(n => n.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.ProductID)
                        .HasConstraintName("FK_Note_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(n => n.Attachments)
                        .WithOne()
                        .HasForeignKey(n => n.ProductID)
                        .HasConstraintName("FK_Attachment_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(t => t.Translations)
                        .WithOne()
                        .HasForeignKey(t => t.ProductID)
                        .HasConstraintName("FK_Translation_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(s => s.ProductSuppliers)
                        .WithOne()
                        .HasForeignKey(s => s.ProductID)
                        .HasConstraintName("FK_ProductSupplier_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(s => s.ProductCharges)
                        .WithOne()
                        .HasForeignKey(c => c.ProductID)
                        .HasConstraintName("FK_ProductCharge_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(s => s.ProductUnits)
                        .WithOne()
                        .HasForeignKey(s => s.ProductID)
                        .HasConstraintName("FK_ProductUnit_Product");

            modelBuilder.Entity<Product>()
                        .HasMany(s => s.ProductStocks)
                        .WithOne()
                        .HasForeignKey(s => s.ProductID)
                        .HasConstraintName("FK_ProductStock_Product");

            modelBuilder.Entity<Product>()
                        .HasIndex(p => p.ProductNumber)
                        .IsUnique();

            modelBuilder.Entity<Product>()
                        .HasIndex(p => p.ProductEAN);

            modelBuilder.Entity<Product>()
                        .HasIndex(p => new { p.SupplierID, p.SupplierProductRef });

            modelBuilder.Entity<Product>()
                        .HasIndex(p => new { p.BrandID, p.BrandProductRef });

        }
        private void CreateProductAttributeModel(ModelBuilder modelBuilder)
        {

        }
        private void CreateProductChargeModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCharge>().HasKey(c => c.Id);

            modelBuilder.Entity<ProductCharge>().Property(c => c.ChargeProductNumber).HasMaxLength(50);
            modelBuilder.Entity<ProductCharge>().Property(c => c.ChargeDescription).HasMaxLength(250);

            modelBuilder.Entity<ProductCharge>()
                        .HasOne(c => c.ChargeProduct)
                        .WithOne()
                        .HasForeignKey<ProductCharge>(c => c.ChargeProductID)
                        .HasConstraintName("FK_Product_ProductCharge");

            modelBuilder.Entity<ProductCharge>()
                .HasIndex(p => p.ProductID);
        }

        private void CreateProductSupplierModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSupplier>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductSupplier>().Property(p => p.SupplierProductRef).HasMaxLength(50);

            modelBuilder.Entity<ProductSupplier>()
                        .HasIndex(p => p.SupplierProductRef);
            modelBuilder.Entity<ProductSupplier>()
                        .HasIndex(p => new { p.SupplierID, p.SupplierProductRef });

        }
        private void CreateProductRelationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductRelation>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductRelation>().Property(p => p.Remarks).HasMaxLength(500);

            modelBuilder.Entity<ProductRelation>()
                .HasIndex(p => new { p.ProductID, p.RelatedProductID })
                .IsUnique(true);

            modelBuilder.Entity<ProductRelation>()
                .HasOne(r => r.RelatedProduct)
                .WithMany()
                .HasForeignKey(r => r.RelatedProductID)
                .HasConstraintName("FK_ProductRelation_Product");
        }
        private void CreateProductUnitModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductUnit>().HasKey(p => p.Id);

            modelBuilder.Entity<ProductUnit>().Property(p => p.ShortName).HasMaxLength(50);
            modelBuilder.Entity<ProductUnit>().Property(p => p.UnitEAN).HasMaxLength(50);

            modelBuilder.Entity<ProductUnit>()
                        .HasOne(p => p.Unit)
                        .WithMany()
                        .HasForeignKey(p => p.UnitID)
                        .HasConstraintName("FK_Unit_ProductUnit");

            modelBuilder.Entity<ProductUnit>()
                        .HasIndex(p => p.UnitEAN);

            modelBuilder.Entity<ProductUnit>()
                        .HasIndex(p => p.ShortName);
        }

        private void CreateProductMovementModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductMovement>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductMovement>().Property(p => p.Location).HasMaxLength(50);
            modelBuilder.Entity<ProductMovement>().Property(p => p.Reference).HasMaxLength(250);
            modelBuilder.Entity<ProductMovement>().Property(p => p.MovementReason).HasMaxLength(100);

            modelBuilder.Entity<ProductMovement>()
                        .HasIndex(p => p.ProductID)
                        .IsUnique(false);

            modelBuilder.Entity<ProductMovement>()
                        .HasIndex(p => new { p.ProductID, p.StoreID, p.WarehouseID })
                        .IsUnique(false);
        }
        private void CreateProductStockModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductStock>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductStock>().Property(p => p.Location).HasMaxLength(50);

            modelBuilder.Entity<ProductStock>()
                        .HasIndex(p => p.ProductID)
                        .IsUnique(false);

            modelBuilder.Entity<ProductStock>()
                        .HasIndex(p => new { p.ProductID, p.StoreID, p.WarehouseID })
                        .IsUnique(true);
        }
        private void CreateProjectModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                        .HasMany(p => p.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.ProjectID)
                        .HasConstraintName("FK_Note_Project");

            modelBuilder.Entity<Project>()
                        .HasMany(p => p.Translations)
                        .WithOne()
                        .HasForeignKey(t => t.ProjectID)
                        .HasConstraintName("FK_Translation_Project");

            modelBuilder.Entity<Project>()
                        .HasOne(c => c.Customer)
                        .WithMany()
                        .HasForeignKey(t => t.CustomerID)
                        .HasConstraintName("FK_Project_Customer");

            modelBuilder.Entity<Project>()
                        .HasIndex(p => p.CustomerID);
        }
        private void CreateSupplierModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>().HasKey(s => s.Id);
            modelBuilder.Entity<Supplier>().Property(s => s.Number).HasMaxLength(25).IsRequired();
            modelBuilder.Entity<Supplier>().Property(s => s.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Supplier>().Property(s => s.ShortName).HasMaxLength(50);
            modelBuilder.Entity<Supplier>().Property(s => s.Prefix).HasMaxLength(25);
            modelBuilder.Entity<Supplier>().Property(s => s.Suffix).HasMaxLength(25);
            modelBuilder.Entity<Supplier>().Property(s => s.LanguageCode).HasMaxLength(25);
            modelBuilder.Entity<Supplier>().Property(s => s.TaxNumber).HasMaxLength(25);
            modelBuilder.Entity<Supplier>().Property(s => s.IBAN).HasMaxLength(50);
            modelBuilder.Entity<Supplier>().Property(s => s.BIC).HasMaxLength(25);
            modelBuilder.Entity<Supplier>().Property(s => s.Currency).HasMaxLength(3);

            modelBuilder.Entity<Supplier>()
                        .HasOne(s => s.ContactInfo)
                        .WithOne()
                        .HasForeignKey<Supplier>(s => s.ContactInfoID)
                        .HasConstraintName("FK_Supplier_ContactInfo");

            modelBuilder.Entity<Supplier>()
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey(s => s.CategoryID)
                        .HasConstraintName("FK_Supplier_Category")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                        .HasMany(s => s.Addresses)
                        .WithOne()
                        .HasForeignKey(a => a.SupplierID)
                        .HasConstraintName("FK_Address_Supplier")
                        .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Supplier>()
                        .HasMany(s => s.Attachments)
                        .WithOne()
                        .HasForeignKey(a => a.SupplierID)
                        .HasConstraintName("FK_Attachment_Supplier")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Supplier>()
                        .HasMany(s => s.Communications)
                        .WithOne()
                        .HasForeignKey(c => c.SupplierID)
                        .HasConstraintName("FK_Communication_Supplier")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Supplier>()
                        .HasMany(s => s.Notes)
                        .WithOne()
                        .HasForeignKey(n => n.SupplierID)
                        .HasConstraintName("FK_Note_Supplier")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Supplier>()
                        .HasMany(s => s.Persons)
                        .WithOne()
                        .HasForeignKey(p => p.SupplierID)
                        .HasConstraintName("FK_Person_Supplier")
                        .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<Supplier>()
                        .HasIndex(s => s.Number)
                        .IsUnique(true);

            modelBuilder.Entity<Supplier>()
                        .HasIndex(s => s.TaxNumber)
                        .IsUnique(false);

            modelBuilder.Entity<Supplier>()
                        .HasIndex(s => s.Name)
                        .IsUnique(false);
        }
        private void CreateTranslationModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translation>().HasKey(t => t.Id);

            modelBuilder.Entity<Translation>().Property(t => t.LanguageCode).HasMaxLength(25);
            modelBuilder.Entity<Translation>().Property(t => t.Name).HasMaxLength(200);
            modelBuilder.Entity<Translation>().Property(t => t.Description).HasMaxLength(2500);

            modelBuilder.Entity<Translation>()
                        .HasDiscriminator<EntityType>("TranslationType")
                        .HasValue<CategoryTranslation>(EntityType.Category)
                        .HasValue<AppMenuTranslation>(EntityType.Menu)
                        .HasValue<ProductTranslation>(EntityType.Product)
                        .HasValue<ProjectTranslation>(EntityType.Project)
                        .HasValue<UnitTranslation>(EntityType.ProductUnit);

            modelBuilder.Entity<Translation>()
                        .HasIndex(t => t.Name);
            modelBuilder.Entity<Translation>()
                        .HasIndex(t => t.LanguageCode);
        }
        private void CreateUnitModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>().HasKey(u => u.Id);
            modelBuilder.Entity<Unit>().Property(u => u.Id).ValueGeneratedNever();    //No Identity
            modelBuilder.Entity<Unit>().Property(u => u.UnitCode).HasMaxLength(25);

            modelBuilder.Entity<Unit>()
                        .HasMany(t => t.Translations)
                        .WithOne()
                        .HasForeignKey(t => t.UnitID)
                        .HasConstraintName("FK_Translation_Unit")
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Unit>()
                        .HasIndex(u => u.UnitCode);
        }

        private void CreateVatPercentageModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxPercentage>().HasKey(w => w.Id);

            modelBuilder.Entity<TaxPercentage>()
                        .HasIndex(v => new { v.From, v.Until, v.Rate })
                        .IsUnique(true);

            modelBuilder.Entity<TaxPercentage>()
                        .HasIndex(v => new { v.From, v.Until })
                        .IsUnique(false);
        }
        private void CreateWarehouseModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().HasKey(w => w.Id);
            modelBuilder.Entity<Warehouse>().Property(w => w.Id).ValueGeneratedNever();      //No Identity
            modelBuilder.Entity<Warehouse>().Property(w => w.Name).HasMaxLength(100).IsRequired();

            modelBuilder.Entity<Warehouse>()
                       .HasOne(c => c.ContactInfo)
                       .WithOne()
                       .HasForeignKey<Warehouse>(c => c.ContactInfoID)
                       .HasConstraintName("FK_Warehouse_Contact");

            modelBuilder.Entity<Warehouse>()
                       .HasOne(c => c.Address)
                       .WithOne()
                       .HasForeignKey<Warehouse>(c => c.AddressID)
                       .HasConstraintName("FK_Warehouse_Address");
        }

        //private void CreateViewModel(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<FunctionResult>().HasNoKey();
        //    modelBuilder.Entity<CustomerSearchResult>().HasNoKey();
        //    modelBuilder.Entity<ProductSearchResult>().HasNoKey();
        //}

    }
}
