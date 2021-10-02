using Domain.Entities;
using Domain.Entities.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StructureDBContext : IdentityDbContext<DCAppUser>
    {
        public StructureDBContext(DbContextOptions<StructureDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataModelMapper.SetupMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);

            //var entity = modelBuilder.Entity<DCAppDataField>();
            //// this is a continuation of the OnModelCreating method
            //entity.Property(e => e.DataType)
            //    .HasConversion(x => x.Value, x => DataDefinitionDataType.GetByValue(x));

            //modelBuilder.Entity<>().OwnsMany(p => p.ChildRoles, a =>
            //{
            //    a.HasForeignKey("Id");
            //    a.Property<Guid>("Id");
            //    a.HasKey("Id", "Id");
            //});
        }

        //entities

        public override int SaveChanges()
        {
            OnBeforeSaveChanges();

            ChangeTracker.DetectChanges();
            //UpdateUpdatedProperty<LocalizationRecord>();
            return base.SaveChanges();
        }

        private void OnBeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified))
            {
               
                if (entry.Entity is IAssociationEntity) continue;
                //if (entry.Entity is ApplicationUser) continue;
                //if (entry.Entity is IdentityRole) continue;
                //if (entry.Entity is IdentityUserRole<string>) continue;

                entry.Property(DataModelMapper.LastModifiedDateColumnName).CurrentValue = DateTime.Now;
                entry.Property(DataModelMapper.LastModifiedUserColumnName).CurrentValue = "ADMIN";
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            //var triggers = RegisterTriggers();

            OnBeforeSaveChanges();

            int saveResult = await base.SaveChangesAsync(cancellationToken);

            //await RunTrigges(triggers);

            return saveResult;
        }


        public DbSet<DCAppStructure> DAppStructures { get; set; }
        //public DbSet<DCAppRoleCollection> DAppRoleCollections { get; set; }
        //public DbSet<DCAppExternalRole> DAppExternalRoles { get; set; }
        //public DbSet<DCAppInternalRole> DAppInternalRoles { get; set; }
        //public DbSet<DCAppExternalUser> DAppExternalUsers { get; set; }
        //public DbSet<DCAppInternalUser> DAppInternalUsers { get; set; }

        //public DbSet<DCAppRolePermission> DCAppPermissions { get; set; }
        public DbSet<DCAppGroup> DAppGroups { get; set; }
        public DbSet<DCAppInternalGroup> DAppInternalGroups { get; set; }
        public DbSet<DCAppExternalGroup> DAppExternalGroups { get; set; }
        public DbSet<DCAppFeature> DAppFeatures { get; set; }
        //public DbSet<DCAppWorkFlow> DAppWorkFlows { get; set; }
        //public DbSet<DCAppRoleAccessGroup> DAppRoleAccessGroups { get; set; }
        //public DbSet<DCAppPage> DAppPages { get; set; }
        public DbSet<DCAppControl> DAppControls { get; set; }

        public DbSet<DCAppControlAction> DAppControlAction { get; set; }
        public DbSet<DCAppControlProperty> DAppControlProperties { get; set; }
        public DbSet<DCAppEntityRowReference> DCAppEntityRowReferences { get; set; }
        
        public DbSet<DCAppDataModel> DAppDataModels { get; set; }
       
        public DbSet<DCAppCapabilityDataModel> DAppCapabilityDataModels { get; set; }
        
        public DbSet<DCAppDataField> DAppDataFields { get; set; }
        
        public DbSet<DCAppDataValue> DAppDataValues { get; set; }
        public DbSet<DCAppDataDefinitionBase> DCAppDataDefinitions { get; set; }

        public DbSet<DCAppStringDataDefinition> DCAppStringDataDefinitions { get; set; }

        public DbSet<DCAppRefEntityDataDefinition> DCAppRefEntityDataDefinitions { get; set; }

        public DbSet<DCAppBoolDataDefinition> DCAppBoolDataDefinitions { get; set; }
        public DbSet<DCAppChoiceDataDefinition> DCAppChoiceDataDefinitions { get; set; }

        public DbSet<DCAppDataChoiceItem> DAppDataChoiceItems { get; set; } 
        public DbSet<DCAppFileDataDefinition> DCAppFileDataDefinitions { get; set; }
        public DbSet<DCAppNumberDataDefinition> DCAppNumberDataDefinitions { get; set; }
    }
}