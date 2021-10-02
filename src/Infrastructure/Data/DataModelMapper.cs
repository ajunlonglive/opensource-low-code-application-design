using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;

namespace Infrastructure.Data
{
    public static class DataModelMapper
    {
        public const string LastModifiedDateColumnName = "LastModifiedDate";
        public const string LastModifiedUserColumnName = "LastModifiedUser";

        public static void SetupMappings(ModelBuilder builder)
        {
            MapStructure(builder);
            MapRolePermissions(builder);
            MapUser(builder);
            MapGroups(builder);
            MapWorkFlows(builder);
            MapRoleRule(builder);
            MapAppRole(builder);
            MapAppPage(builder);
            
            MapRoleAccessGroups(builder);
            MapRoleRelations(builder);
            MapDataValue(builder);
            MapDataField(builder);
            MapDataDefinition(builder);
            
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                //IsDirty is for local tracking, not persisted in the database
                //builder.Entity(entityType.Name).Ignore("IsDirty");


                if (entityType.ClrType.IsSubclassOf(typeof(Entity)) || entityType.ClrType.IsSubclassOf(typeof(IdentityUser)) || entityType.ClrType.ToString().Contains("Identity"))
                {
                    // Add shadow properties for additional change tracking
                    builder.Entity(entityType.Name).Property<DateTime>(LastModifiedDateColumnName);
                    builder.Entity(entityType.Name).Property<string>(LastModifiedUserColumnName);
                }
            }
        }

        private static void MapDataValue(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppDataValue>();
            entity.HasIndex(l => l.Name);
            entity.Property(e => e.MultipleReferenceRowIds)
              .HasConversion(
                  v => string.Join(',', v),
                  v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList());
        }

        private static void MapDataField(ModelBuilder builder)
        {
           
        }
        private static void MapDataDefinition(ModelBuilder builder)
        {
            builder.Entity<DCAppChoiceDataDefinition>()
                .HasMany(e => e.Choices)
                .WithOne(p=>p.ChoiceParent).HasForeignKey(k=>k.ChoiceParentId);           

            builder.Entity<DCAppStringDataDefinition>()
                .Property(e => e.Length).HasDefaultValue(100);

            builder.Entity<DCAppNumberDataDefinition>()
                .Property(e => e.DecimalPlaces).HasDefaultValue(2);

            builder.Entity<DCAppDateTimeDataDefinition>()
                .Property(e => e.Format).HasDefaultValue("DD/MMM/YY");
        }

        private static void MapAppPage(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppPage>();
            entity.ToTable("DAppPages").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.HasMany(c => c.Controls).WithOne(p => p.DCAppPage).HasForeignKey(k=>k.DCAppPageId);
        }
        private static void MapStructure(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppStructure>();
            entity.ToTable("DAppStructures").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);           
        }


        private static void MapRoleRule(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppRoleRule>();
            entity.ToTable("DAppRoleRules").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.OwnsOne(x => x.RuleType).Property<string>("Value");
            entity.OwnsOne(p => p.RuleType)
                .Property<string>("DisplayName");
        }

        private static void MapUser(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppUser>();
            entity.ToTable("DAppUsers").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);

            var userRole = builder.Entity<DCAppUserRole>();
            userRole.ToTable("DAppUserRoles")
                .HasKey(p => new { p.UserId, p.RoleId});
            
        }

        private static void MapRolePermissions(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppRolePermission>();
            entity.ToTable("DAppRolePermissions").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.OwnsOne(p => p.AccessLevel)
                .Property<string>("Value");
            entity.OwnsOne(p => p.AccessLevel)
                .Property<string>("DisplayName");
        }

        private static void MapAppRole(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppRole>();
            entity.ToTable("DAppRoles");
           entity.HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.OwnsOne(p => p.SystemRole)
                .Property<string>("Value");
            entity.OwnsOne(p => p.SystemRole)
                .Property<string>("DisplayName");
        }

        private static void MapWorkFlows(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppWorkFlow>();
            entity.ToTable("DAppWorkFlows").HasKey(l => l.Id);
            entity.HasIndex(l => l.Name);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);

            entity.OwnsOne(p => p.RowAccessType)
                .Property<string>("Value");
            entity.OwnsOne(p => p.RowAccessType)
                .Property<string>("DisplayName");

            entity.HasMany(p => p.Pages).WithOne(p => p.DCAppWorkFlow).HasForeignKey(k => k.DCAppWorkFlowId);
        }

        private static void MapGroups(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppGroup>();
            entity.ToTable("DAppGroups").HasKey(l => l.Id);
            entity.HasOne(p => p.ParentGroup).WithMany(c => c.ChildGroups)
                .HasForeignKey(k=>k.ParentGroupId);

            entity.HasMany(c => c.Features)
               .WithOne(c => c.Group).HasForeignKey(k => k.GroupId);

            entity.HasMany(c => c.DataModels)
                .WithOne(p => p.Group).HasForeignKey(k => k.GroupId);

            entity.HasMany(c => c.Roles)
               .WithOne(c => c.Group).HasForeignKey(k => k.GroupId);
        }

        private static void MapRoleAccessGroups(ModelBuilder builder)
        {
            var entity = builder.Entity<DCAppRoleAccessGroup>();
            entity.ToTable("DAppRoleAccessGroups").HasKey(l => l.Id);
        }

        private static void MapRoleRelations(ModelBuilder builder)
        {
           //var entity = builder.Entity<DCAppUserOnRole>();
           //entity.ToTable("UserOnRoles").HasKey(l => l.Id);

            //var entity = builder.Entity<DCAppRoleRelatedUser>();
            //entity.ToTable("RoleRelatedUsers").HasKey(l => l.Id);

            //entity.OwnsOne(p => p.Relation)
            //        .Property<string>("Value");
            //entity.OwnsOne(p => p.Relation)
            //    .Property<string>("DisplayName");
        }
    }
}