using Domain.Entities;
using Domain.Factories;
using Domain.ValueObjects;
using Infrastructure.Data;
using Markdig.Extensions.Yaml;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Presentation.EntityBuilders;
using Presentation.Utility;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Services
{
    public class GroupService : IGroupService
    {
        private readonly StructureDBContext _structureDBContext;

        public GroupService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }

        public IEnumerable<SelectListItem> GetGroupsAsSelectListItems()
        {
            return _structureDBContext.DAppDataValues.Include(d => d.DataField).ThenInclude(d=>d.DCAppDataModel)
                        .Where(x => x.DataField.DCAppDataModel.Name == EFStringConstants.GroupsTable
                        && x.DataField.Name == EFStringConstants.NameField).ToList()
              .Select(c => new SelectListItem { Text = c.Name, Value = c.RowId.ToString() })
              .ToList();
        }

        private IEnumerable<DCAppGroup> GetChildren(DCAppGroup group)
        {
            foreach (var rChild in group.ChildGroups.SelectMany(child => GetChildren(child)))
            {
                yield return rChild;
            }
        }

        public DCAppServiceMessage GetGroupList(bool isInternal)
        {
            Guid groupId = Guid.Empty;
            if (isInternal)
            {
                groupId = _structureDBContext.DAppStructures.Include(y => y.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }
            else
            {
                groupId = _structureDBContext.DAppStructures.Include(y => y.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            var group = _structureDBContext.DAppGroups.Include(g => g.ChildGroups).AsEnumerable()
                .Where(x => x.Id == groupId).SelectManyRecursive(l => l.ChildGroups).ToList();

            group.Insert(0, _structureDBContext.DAppGroups
               .Where(x => x.Id == groupId).FirstOrDefault());

            if (group.Count() > 0 || group.Count() > 0)
            {
                var items = group.Select(f => new DCAppGroupViewModel()
                {
                    Id = f.Id.ToString(),
                    Name = f.Name,
                    Description = f.Description,
                    ParentGroupId = f.ParentGroupId.ToGuid(),
                    FeatureCount = (short)f.Features.Count(),
                    RoleCount = (short)f.Roles.Count(),
                    DataModelCount = (short)f.DataModels.Count(),
                    CapabilityDataModelCount = (short)f.CapabilityDataModels.Count(),
                    HasChildren = f.HasChildGroups
                }).ToList();

                items[0].ParentGroupId = null;

                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, items);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, new List<DCAppGroupViewModel>());
            }
        }


        public DCAppServiceMessage CreateGroup(DCAppGroupViewModel model)
        {
            try
            {
                var groupEntity = EntityFactory.CreateNewGroup();
                model.Id = groupEntity.Id.ToString();

                groupEntity = GroupEntityBuilder.BuildCreatedEntity(groupEntity, model);

                if (model.ParentGroupId != Guid.Empty)
                {
                    groupEntity.ParentGroupId = model.ParentGroupId;
                    groupEntity.ParentGroup = _structureDBContext.DAppGroups.Where(x => x.Id == model.ParentGroupId).FirstOrDefault();
                }

                _structureDBContext.DAppGroups.Add(groupEntity);
                _structureDBContext.SaveChanges();

                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, GroupEntityBuilder.BuildViewModel(groupEntity));
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }


        public DCAppServiceMessage RemoveGroup(DCAppGroupViewModel model)
        {
            var group = _structureDBContext.DAppGroups.FirstOrDefault(x => x.Id.ToString() == model.Id);

            // Delete the DataValues
            //foreach
            //_structureDBContext.DAppDataValues.Remove();
            // Delete the DataFields
            //foreach
            //_structureDBContext.DAppDataFields.Remove();
            // Delete the DataModel

            _structureDBContext.DAppGroups.Remove(group);

            _structureDBContext.SaveChanges();
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded);
        }

        public DCAppServiceMessage UpdateGroup(DCAppGroupViewModel model)
        {
            try
            {
                DCAppGroup group = _structureDBContext.DAppGroups.FirstOrDefault(x => x.Id.ToString() == model.Id);
                group.Name = model.Name;
                group.Description = model.Description;

                if (model.ParentGroupId != Guid.Empty)
                {
                    group.ParentGroupId = model.ParentGroupId;
                    group.ParentGroup = _structureDBContext.DAppGroups.Where(x => x.Id == model.ParentGroupId).FirstOrDefault();
                }
                else
                {
                    group.ParentGroup = null;
                    group.ParentGroupId = Guid.Empty;
                }

                _structureDBContext.SaveChanges();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, GroupEntityBuilder.BuildViewModel(group));
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }
    }
}