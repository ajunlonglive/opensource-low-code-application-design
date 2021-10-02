using Domain.Entities;
using Presentation.ViewModels;

namespace Presentation.EntityBuilders
{
    public static class StructureEntityBuilder
    {
        public static DCAppStructure BuildCreatedEntity(DCAppStructure entity, DCAppStructureViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            return entity;
        }

        public static DCAppStructureViewModel BuildViewModel(DCAppStructure entity)
        {

            var structure = new DCAppStructureViewModel(entity.Id.ToString());
            structure.Name = entity.Name;
           
            /*foreach(DCAppInternalUser user in entity.Users.)
            structure.Users.Internal.Add(new DCAppUserViewModel(Guid.NewGuid().ToString()) { Name = "Suresh" });
            structure.Users.Internal.Add(new DCAppUserViewModel(Guid.NewGuid().ToString()) { Name = "Tinku" });

            structure.Users.External.Add(new DCAppUserViewModel(Guid.NewGuid().ToString()) { Name = "Anil" });
            structure.Users.External.Add(new DCAppUserViewModel(Guid.NewGuid().ToString()) { Name = "John" });

            var role = new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "System Admin" };
            var swRole = new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "Software Admin" };
            role.ChildRoles.Add(swRole);
            swRole.ChildRoles.Add(new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "Hardware admin" });
            structure.Roles.Internal.Add(role);

            structure.Roles.Internal.Add(new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "CMO" });

            structure.Roles.External.Add(new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "Care Taker" });
            structure.Roles.External.Add(new DCAppRoleViewModel(Guid.NewGuid().ToString()) { Name = "TGT Teacher" });

            structure.Hub.DataModels.Add(new DCAppDataModelViewModel(Guid.NewGuid().ToString()) { Name = "Test DataModel" });
            structure.Hub.CapabilityDataModels.Add(new DCAppCapabilityDataModelViewModel(Guid.NewGuid().ToString()) { Name = "Test Capability DataModel" });

            structure.Hub.Features.Add(new DCAppFeatureViewModel(Guid.NewGuid().ToString()) { Name = "TT Feature" });

            var group = new DCAppGroupViewModel(Guid.NewGuid().ToString()) { Name = "PG GRP" };
            var ptFeature = new DCAppFeatureViewModel(Guid.NewGuid().ToString()) { Name = "Pt Feat" };
            group.Features.Add(ptFeature);
            var childGroup = new DCAppGroupViewModel(Guid.NewGuid().ToString()) { Name = "CH GRP" };
            var feature = new DCAppFeatureViewModel(Guid.NewGuid().ToString()) { Name = "SM Feat" };
            childGroup.Features.Add(feature);
            group.ChildGroups.Add(childGroup);
            structure.Hub.Groups.Add(group);
            */

            return structure;
        }
    }
}