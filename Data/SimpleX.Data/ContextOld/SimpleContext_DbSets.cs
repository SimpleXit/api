using Microsoft.EntityFrameworkCore;
using SimpleX.Data.Entities;
using SimpleX.Data.Entities.App;
using SimpleX.Data.Entities.Assets;
using SimpleX.Data.Entities.Common;
//using SimpleX.Data.Functions;
//using SimpleX.Data.StoredProcedures;

namespace SimpleX.Data.Context
{
    public partial class SimpleContext
    {   
        #region DbSets

        public DbSet<AppMenu> AppMenu { get; set; }
        public DbSet<AppMenuTranslation> AppMenuTranslation { get; set; }
        public DbSet<AppPermission> AppPermission { get; set; }
        public DbSet<AppSetting> AppSetting { get; set; }
        public DbSet<AppTask> AppTask { get; set; }
        public DbSet<AppTaskLog> AppTaskLog { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserRole> AppUserRole { get; set; }

        public DbSet<Address> Address { get; set; }
        //public DbSet<Person> Person { get; set; }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<CustomerContactInfo> CustomerContactInfo { get; set; }
        public DbSet<CustomerPerson> CustomerPerson { get; set; }
        public DbSet<CustomerCommunication> CustomerCommunication { get; set; }
        public DbSet<CustomerAttachment> CustomerAttachment { get; set; }
        public DbSet<CustomerNote> CustomerNote { get; set; }

        
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentLine> DocumentLine { get; set; }
        public DbSet<DocumentLineData> DocumentLineData { get; set; }
        public DbSet<DocumentAssociate> DocumentAssociate { get; set; }
        public DbSet<DocumentLevel> DocumentLevel { get; set; }
        public DbSet<DocumentSource> DocumentSource { get; set; }
        public DbSet<DocumentNumber> DocumentNumber { get; set; }

        public DbSet<Employee> Employee { get; set; }



        public DbSet<Product> Product { get; set; }
        public DbSet<ProductAttribute> ProductAttribute { get; set; }
        public DbSet<ProductCharge> ProductCharge { get; set; }
        public DbSet<ProductMovement> ProductMovement { get; set; }
        public DbSet<ProductNote> ProductNote { get; set; }
        public DbSet<ProductRelation> ProductRelation { get; set; }
        public DbSet<ProductStock> ProductStock { get; set; }
        public DbSet<ProductSupplier> ProductSupplier { get; set; }
        public DbSet<ProductUnit> ProductUnit { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Attribute> Attribute { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<DefaultValue> DefaultValue { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<TaxPercentage> TaxPercentage { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }

        #endregion
    }
}
