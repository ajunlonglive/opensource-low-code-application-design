using Infrastructure.Data;
using Presentation.EntityBuilders;
using Presentation.ViewModels;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Presentation.Services
{
    public class StructureService : IStructureService
    {
        public StructureDBContext _structureDBContext;

        public StructureService(StructureDBContext dbContext)
        {
            _structureDBContext = dbContext;
        }
        /*
        public ICollection<ParentNodeViewModel> GetLightStructuresForUsers(IPrincipal principal)
        {
            List<DCAppStructureViewModel> list = new List<DCAppStructureViewModel>();

            // StructureRepository gets the structures and presents them.
            var structures = _structureDBContext.DAppStructures;
            if (structures.Count() > 0)
            {
                var structureFromDB = structures.FirstOrDefault(); // TODO

                DCAppStructureViewModel structure = StructureEntityBuilder.BuildViewModel(structureFromDB);

                list.Add(structure);
            }
            var nodelist = presentStructureList(list);
            AddHtmlAttributes(nodelist);
            return nodelist;
        }

        private void AddHtmlAttributes(ICollection<ParentNodeViewModel> list)
        {
            foreach (ParentNodeViewModel model in list)
            {
                model.HtmlAttributes.Add("NodeType", model.NodeType);
                model.HtmlAttributes.Add("StructureId", model.NodeId);
                var hubId = model.child.Find(x => x.NodeType == "Hub").NodeId;
                model.HtmlAttributes.Add("HubId", hubId);
                AddHtmlAttributes(model.NodeId, hubId, model.child);
            }
        }

        private void AddHtmlAttributes(string structureId, string hubId, ICollection<ChilditemViewModel> children)
        {
            foreach (ChilditemViewModel model in children)
            {
                model.HtmlAttributes.Add("NodeType", model.NodeType);
                model.HtmlAttributes.Add("StructureId", structureId);
                model.HtmlAttributes.Add("HubId", hubId);
                AddHtmlAttributes(structureId, hubId, model.child);
            }

            return;
        }

        private ICollection<ParentNodeViewModel> presentStructureList(List<DCAppStructureViewModel> list)
        {
            var parentList = new List<ParentNodeViewModel>();
            try
            {
                foreach (DCAppStructureViewModel model in list)
                {
                    var parent = new ParentNodeViewModel
                    {
                        NodeId = model.Id.ToString(),
                        NodeText = model.Name,
                        Expanded = true,
                        Icon = StringConstants.Icon_Structure,
                        NodeType = StringConstants.NodeType_Structure
                    };

                    parentList.Add(parent);

                    AddFirstLevelChildren(parent, model);
                }
            }
            catch (Exception exception)
            {
            }
            return parentList;
        }

        private void AddFirstLevelChildren(ParentNodeViewModel parent, DCAppStructureViewModel model)
        {
            var users = new ChilditemViewModel
            {
                NodeId = model.Users.Id.ToString(),
                NodeText = model.Users.Name,
                Icon = StringConstants.Icon_Users,
                NodeType = StringConstants.NodeType_Users,
            };

            //AddUsersToNode(users, model);

            var roles = new ChilditemViewModel
            {
                NodeId = model.Roles.Id.ToString(),
                NodeText = model.Roles.Name,
                Icon = StringConstants.Icon_Roles,
                NodeType = StringConstants.NodeType_Roles
            };

            //AddRolesToNode(roles, model);

            var hub = new ChilditemViewModel
            {
                NodeId = model.Hub.Id.ToString(),
                NodeText = model.Hub.Name,
                Icon = StringConstants.Icon_Hub,
                NodeType = StringConstants.NodeType_Hub
            };

            AddHubChildElements(hub, model);

            parent.child.Add(users);
            parent.child.Add(roles);
            parent.child.Add(hub);
        }

        private void AddUsersToNode(ChilditemViewModel users, DCAppStructureViewModel model)
        {
            var internalUsers = new ChilditemViewModel
            {
                NodeId = "internalUsers",
                NodeText = "Intenal",
            };
            users.child.Add(internalUsers);

            foreach (DCAppUserViewModel user in model.Users.Internal)
            {
                var userNode = new ChilditemViewModel
                {
                    NodeId = user.Id.ToString(),
                    NodeText = user.Name,
                };
                internalUsers.child.Add(userNode);
            }

            var externalUsers = new ChilditemViewModel
            {
                NodeId = "externalUsers",
                NodeText = "Extenal",
            };
            users.child.Add(externalUsers);
            foreach (DCAppUserViewModel user in model.Users.External)
            {
                var userNode = new ChilditemViewModel
                {
                    NodeId = user.Id.ToString(),
                    NodeText = user.Name,
                };
                externalUsers.child.Add(userNode);
            }
        }

        private void AddRolesToNode(ChilditemViewModel roles, DCAppStructureViewModel model)
        {
            var internalRoles = new ChilditemViewModel
            {
                NodeId = "internalRoles",
                NodeText = "Intenal",
            };
            roles.child.Add(internalRoles);

            foreach (DCAppRoleViewModel role in model.Roles.Internal)
            {
                var roleNode = new ChilditemViewModel
                {
                    NodeId = role.Id.ToString(),
                    NodeText = role.Name,
                };
                internalRoles.child.Add(roleNode);
                foreach (DCAppRoleViewModel childRole in role.ChildRoles)
                {
                    AddRolesRecursivelyToNode(childRole, roleNode.child);
                }
            }

            var externalRoles = new ChilditemViewModel
            {
                NodeId = "externalRoles",
                NodeText = "Extenal",
            };

            roles.child.Add(externalRoles);

            foreach (DCAppRoleViewModel role in model.Roles.External)
            {
                var roleNode = new ChilditemViewModel
                {
                    NodeId = role.Id.ToString(),
                    NodeText = role.Name,
                };
                externalRoles.child.Add(roleNode);
                foreach (DCAppRoleViewModel childRole in role.ChildRoles)
                {
                    AddRolesRecursivelyToNode(childRole, roleNode.child);
                }
            }
        }

        private void AddRolesRecursivelyToNode(DCAppRoleViewModel role, List<ChilditemViewModel> child)
        {
            var roleNode = new ChilditemViewModel
            {
                NodeId = role.Id.ToString(),
                NodeText = role.Name,
            };

            child.Add(roleNode);

            foreach (DCAppRoleViewModel childRole in role.ChildRoles)
            {
                AddRolesRecursivelyToNode(childRole, roleNode.child);
            }
        }

        private void AddHubChildElements(ChilditemViewModel hub, DCAppStructureViewModel model)
        {
            var dataModels = new ChilditemViewModel
            {
                NodeId = "dataModelTree" + model.Hub.Id.ToString(),
                NodeText = "Data Tables",
                Icon = StringConstants.Icon_DataModel,
                NodeType = StringConstants.NodeType_DataModels
            };

            hub.child.Add(dataModels);
            AddDataModelstoNode(dataModels, model);

            var capabilityDataModels = new ChilditemViewModel
            {
                NodeId = "capabilityDataModelTree" + model.Hub.Id.ToString(),
                NodeText = "Capability Settings",
                Icon = StringConstants.Icon_CapabilityDataModel,
                NodeType = StringConstants.NodeType_CapabilityDataModel
            };

            hub.child.Add(capabilityDataModels);
            AddCapabilityDataModelstoNode(capabilityDataModels, model);

            var features = new ChilditemViewModel
            {
                NodeId = "FeatureTree" + model.Hub.Id.ToString(),
                NodeText = "Features",
                Icon = StringConstants.Icon_Features,
                NodeType = StringConstants.NodeType_Features
            };

            //hub.child.Add(features);
            //AddFeaturestoNode(features, model);

            var permissions = new ChilditemViewModel
            {
                NodeId = "permissionsTree" + model.Hub.Id.ToString(),
                NodeText = "Permissions",
                Icon = "Admin"
            };

            //hub.child.Add(permissions);
            //AddPermissionstoNode(permissions, model);

            var groups = new ChilditemViewModel
            {
                NodeId = "groupsTree" + model.Hub.Id.ToString(),
                NodeText = "Groups",
                Icon = StringConstants.Icon_GroupsAndFeatures,
                NodeType = StringConstants.NodeType_GroupsAndFeatures
            };

            hub.child.Add(groups);
            AddGroupstoNode(groups.child, model);
        }

        private void AddGroupstoNode(List<ChilditemViewModel> child, DCAppStructureViewModel model)
        {
            foreach (DCAppGroupViewModel group in model.Hub.Groups)
            {
                var groupNode = new ChilditemViewModel
                {
                    NodeId = group.Id.ToString(),
                    NodeText = group.Name,
                    Icon = StringConstants.Icon_GroupsData,
                    NodeType = StringConstants.NodeType_GroupsData
                };

                child.Add(groupNode);
                if (group.Features.Count > 0)
                {
                    AddFeaturestoNode(groupNode, group);
                }
                AddSubGroupstoNodeRecursively(groupNode.child, group);
            }
        }

        private void AddSubGroupstoNodeRecursively(List<ChilditemViewModel> child, DCAppGroupViewModel group)
        {
            foreach (DCAppGroupViewModel childGroup in group.ChildGroups)
            {
                var childGroupNode = new ChilditemViewModel
                {
                    NodeId = childGroup.Id.ToString(),
                    NodeText = childGroup.Name,
                    Icon = StringConstants.Icon_GroupsData,
                    NodeType = StringConstants.NodeType_GroupsData
                };

                child.Add(childGroupNode);

                AddFeaturestoNode(childGroupNode, childGroup);
                AddSubGroupstoNodeRecursively(childGroupNode.child, childGroup);
            }
            return;
        }

        private void AddPermissionstoNode(ChilditemViewModel permissions, DCAppStructureViewModel model)
        {
            var permissionNode = new ChilditemViewModel
            {
                NodeId = "Role Access" + model.Hub.AccessPermissions.Id.ToString(),
                NodeText = model.Hub.AccessPermissions.Name,
            };

            permissions.child.Add(permissionNode);
        }

        private void AddFeaturestoNode(ChilditemViewModel features, DCAppStructureViewModel model)
        {
            foreach (DCAppFeatureViewModel feature in model.Hub.Features)
            {
                var featureNode = new ChilditemViewModel
                {
                    NodeId = feature.Id.ToString(),
                    NodeText = feature.Name,
                };

                features.child.Add(featureNode);
            }
        }

        private void AddFeaturestoNode(ChilditemViewModel groupNode, DCAppGroupViewModel group)
        {
            //var featureNode = new ChilditemViewModel
            //{
            //    NodeId = "featureTree" + group.Id.ToString(),
            //    NodeText = "Features",
            //    Icon = StringConstants.Icon_Features,
            //    NodeType = StringConstants.NodeType_Features
            //};

            //groupNode.Child.Add(featureNode);

            foreach (DCAppFeatureViewModel feature in group.Features)
            {
                var childFeatureNode = new ChilditemViewModel
                {
                    NodeId = feature.Id.ToString(),
                    NodeText = feature.Name,
                    Icon = StringConstants.Icon_FeaturesData,
                    NodeType = StringConstants.NodeType_FeaturesData
                };

                groupNode.child.Add(childFeatureNode);
            }
            return;
        }

        private void AddCapabilityDataModelstoNode(ChilditemViewModel capabilityDataModels, DCAppStructureViewModel model)
        {
            foreach (DCAppCapabilityDataModelViewModel dataModel in model.Hub.CapabilityDataModels)
            {
                var capabilityDataModelNode = new ChilditemViewModel
                {
                    NodeId = dataModel.Id.ToString(),
                    NodeText = dataModel.Name,
                    Icon = StringConstants.Icon_CapabilityDataModelData,
                    NodeType = StringConstants.NodeType_CapabilityDataModelData
                };

                capabilityDataModels.child.Add(capabilityDataModelNode);
            }
        }

        private void AddDataModelstoNode(ChilditemViewModel dataModels, DCAppStructureViewModel model)
        {
            foreach (DCAppDataModelViewModel dataModel in model.Hub.DataModels)
            {
                var dataModelNode = new ChilditemViewModel
                {
                    NodeId = dataModel.Id.ToString(),
                    NodeText = dataModel.Name,
                    Icon = StringConstants.Icon_DataModelData,
                    NodeType = StringConstants.NodeType_DataModelData
                };

                dataModels.child.Add(dataModelNode);
            }
        }
        */
    }
}