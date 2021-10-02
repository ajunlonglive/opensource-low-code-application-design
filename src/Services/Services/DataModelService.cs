using Domain.Entities;
using Domain.Entities.Data;
using Domain.Factories;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.EntityBuilders;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Presentation.Utility;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Presentation.Services
{
    public class DataModelService : IDataModelService
    {
        private readonly StructureDBContext _structureDBContext;

        public DataModelService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }

        #region DataModels

        public DCAppServiceMessage GetDataModel(Guid dataModelId)
        {
            var dataModel = _structureDBContext.DAppDataModels.Include(x => x.DataFields).FirstOrDefault(x => x.Id == dataModelId);

            if (dataModel != null)
            {
                var items = dataModel.DataFields.Select(f => DataFieldEntityBuilder.BuildViewModel(f)).ToList();

                var dataModelViewModel = DataModelEntityBuilder.BuildViewModel(dataModel);
                dataModelViewModel.DataFields = items;
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dataModelViewModel);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage GetDataModelsList(Guid groupId)
        {
            var dataModelList = _structureDBContext.DAppDataModels.Where(x=>x.GroupId == groupId).ToList();
            if (dataModelList != null)
            {
                var items = dataModelList.Select(f =>  DataModelEntityBuilder.BuildViewModel(f)).ToList();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, items);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, new List<DCAppDataModelViewModel>());
            }
        }

        public DCAppServiceMessage CreateDataModel(DCAppDataModelViewModel model)
        {
            try
            {
                var dataModelEntity = EntityFactory.CreateNewDataModel();
                model.Id = dataModelEntity.Id.ToString();

                dataModelEntity = DataModelEntityBuilder.BuildCreatedEntity(dataModelEntity, model);
                dataModelEntity.Group = _structureDBContext.DAppGroups.Where(x=>x.Id == model.GroupId).FirstOrDefault();
                dataModelEntity.GroupId = dataModelEntity.Group.Id;
               _structureDBContext.DAppDataModels.Add(dataModelEntity);
                _structureDBContext.SaveChanges();

                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, DataModelEntityBuilder.BuildViewModel(dataModelEntity));
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataModel(DCAppDataModelViewModel model)
        {
            try
            {
                DCAppDataModel dataModel = _structureDBContext.DAppDataModels.FirstOrDefault(x => x.Id.ToString() == model.Id);
                dataModel.Name = model.Name;
                dataModel.Description = model.Description;

                _structureDBContext.SaveChanges();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, DataModelEntityBuilder.BuildViewModel(dataModel));
            }
            catch(Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage RemoveDataModel(DCAppDataModelViewModel model)
        {
            var dataModel = _structureDBContext.DAppDataModels.FirstOrDefault(x=> x.Id.ToString() == model.Id);

            // Delete the DataValues
                //foreach
                //_structureDBContext.DAppDataValues.Remove();
            // Delete the DataFields
                //foreach
                //_structureDBContext.DAppDataFields.Remove();
            // Delete the DataModel

                _structureDBContext.DAppDataModels.Remove(dataModel);

            _structureDBContext.SaveChanges();
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded);

        }

        public IEnumerable<SelectListItem> GetDataModelListAsSelectListItems(string modelToSkipId=null)
        {
            var dataModelList = _structureDBContext.DAppDataModels.Include(g=>g.Group).ToList();
            
            if (dataModelList != null)
            {
                if (!string.IsNullOrEmpty(modelToSkipId))
                {
                    dataModelList.RemoveAll(f => f.Id.ToString() == modelToSkipId);
                }

                // Users, Roles, Groups, 
                List<SelectListItem> itemList = dataModelList.Select(f => new SelectListItem { Text = f.Group.Name + " => " + f.Name, Value = f.Id.ToString() }).ToList();
                itemList.Insert(0, new SelectListItem { Text = "sys_base_DataFields", Value = StringConstants.Guid_DataFieldsTable });
                itemList.Insert(0, new SelectListItem { Text = "sys_base_Users", Value = StringConstants.Guid_UsersTable });
                itemList.Insert(0, new SelectListItem { Text = "sys_base_Roles", Value = StringConstants.Guid_RolesTable});
                itemList.Insert(0, new SelectListItem { Text = "sys_base_Groups", Value = StringConstants.Guid_GroupsTable });
                return itemList;
            }

            return new List<SelectListItem>();
        }


        #endregion

        #region DataFields

        public IEnumerable<SelectListItem> GetDataFieldListAsSelectListItems(string dataModelId)
        {
            var dataFieldList = _structureDBContext.DAppDataFields.Include(y => y.DCAppDataModel).Where(x=>x.DCAppDataModelId.ToString() == dataModelId).ToList();

            if (dataFieldList != null)
            {
                return dataFieldList.Select(f => new SelectListItem { Text = f.DCAppDataModel.Name + " => " + f.Name, Value = f.Id.ToString() }).ToList();
            }

            return new List<SelectListItem>();
        }
        public DCAppServiceMessage CreateDataField(DCAppDataFieldViewModel model)
        {
            var dataModel = _structureDBContext.DAppDataModels.Include(x => x.DataFields).FirstOrDefault(x => x.Id.ToString() == model.DataModelId);
            var dataField = EntityFactory.CreateNewDataField();
            model.Id = dataField.Id.ToString();
            dataField = DataFieldEntityBuilder.BuildCreatedEntity(dataField, model);
            dataModel.DataFields.Add(dataField);
            if (dataField.DataDefinition != null)
            {
                _structureDBContext.DCAppDataDefinitions.Add(dataField.DataDefinition);
            }
            _structureDBContext.DAppDataFields.Add(dataField);
            _structureDBContext.SaveChanges();

            if (dataModel != null)
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, model);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataField(DCAppDataFieldViewModel model)
        {
            DCAppDataModel dataModel = _structureDBContext.DAppDataModels.Include(x => x.DataFields).ThenInclude(t => t.DataDefinition).FirstOrDefault(x => x.Id.ToString() == model.DataModelId);
            var dataField = dataModel.DataFields.FirstOrDefault(x => x.Id == Guid.Parse(model.Id));
            bool isDataDefinitionChanged = false;
            if (dataField.DataType != model.DataType)
            {
                // Remove the existing one

                if (dataField.DataType != model.DataType && dataField.DataType != DataDefinitionDataType.Unknown.ToString())
                {
                    isDataDefinitionChanged = true;
                    if (dataField.DataDefinition != null)
                    {
                        _structureDBContext.DCAppDataDefinitions.Remove(dataField.DataDefinition);
                    }
                }
            }

            dataField = DataFieldEntityBuilder.BuildCreatedEntity(dataField, model);
            
            if (isDataDefinitionChanged)
            {
                _structureDBContext.DCAppDataDefinitions.Add(dataField.DataDefinition);
            }

            _structureDBContext.SaveChanges();

            if (dataField != null)
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, model);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage RemoveDataField(DCAppDataFieldViewModel model)
        {
            DCAppDataModel dataModel = _structureDBContext.DAppDataModels.Include(x => x.DataFields).ThenInclude(t => t.DataDefinition).FirstOrDefault(x => x.Id.ToString() == model.DataModelId);
            var dataField = dataModel.DataFields.FirstOrDefault(x => x.Id.ToString() == model.Id);
            dataModel.DataFields.Remove(dataField);

            if (dataField.DataDefinition != null)
            {
                _structureDBContext.DCAppDataDefinitions.Remove(dataField.DataDefinition);
            }

            _structureDBContext.SaveChanges();

            if (dataField != null)
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed);
            }
        }

        #endregion

        #region DataValues

        #endregion

        #region DataProperties

        public DCAppServiceMessage UpdateDataProperties_String(DCAppDataFieldViewModel model)
        {
            try
            {
                var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                if (dataField.DataType == DataDefinitionDataType.String.Value.ToString())
                {
                    dataField.DataDefinition = StringDataDefinitionEntityBuilder
                        .BuildCreatedEntity((DCAppStringDataDefinition)dataField.DataDefinition,
                        (DCAppDataTypeDefinition_StringViewModel)model.DataDefinition);
                }

                _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch(Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_Number(DCAppDataFieldViewModel model)
        {
            try { 
            var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

            if (dataField.DataType == DataDefinitionDataType.Number.Value.ToString())
            {
                dataField.DataDefinition = NumberDataDefinitionEntityBuilder
                    .BuildCreatedEntity((DCAppNumberDataDefinition)dataField.DataDefinition,
                    (DCAppDataTypeDefinition_NumberViewModel)model.DataDefinition);
            }
           
            _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_Bool(DCAppDataFieldViewModel model)
        {
            try
            {
                var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                if (dataField.DataType == DataDefinitionDataType.Bool.Value.ToString())
                {
                    dataField.DataDefinition = BoolDataDefinitionEntityBuilder
                        .BuildCreatedEntity((DCAppBoolDataDefinition)dataField.DataDefinition,
                        (DCAppDataTypeDefinition_BoolViewModel)model.DataDefinition);
                }

                _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_DateTime(DCAppDataFieldViewModel model)
        {
            try
            {
                var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                if (dataField.DataType == DataDefinitionDataType.DateTime.Value.ToString())
                {
                    dataField.DataDefinition = DateTimeDataDefinitionEntityBuilder
                        .BuildCreatedEntity((DCAppDateTimeDataDefinition)dataField.DataDefinition,
                        (DCAppDataTypeDefinition_DateTimeViewModel)model.DataDefinition);
                }

                _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_File(DCAppDataFieldViewModel model)
        {
            try 
            { 
            var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

            if (dataField.DataType == DataDefinitionDataType.File.Value.ToString())
            {
                dataField.DataDefinition = FileDataDefinitionEntityBuilder
                    .BuildCreatedEntity((DCAppFileDataDefinition)dataField.DataDefinition,
                    (DCAppDataTypeDefinition_FileViewModel)model.DataDefinition);
            }

            _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_Choices(DCAppDataFieldViewModel model)
        {
            try 
            { 
            var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

            if (dataField.DataType == DataDefinitionDataType.Choices.Value.ToString())
            {
                dataField.DataDefinition = ChoiceDataDefinitionEntityBuilder
                    .BuildCreatedEntity((DCAppChoiceDataDefinition)dataField.DataDefinition,
                  (DCAppDataTypeDefinition_ChoiceViewModel)model.DataDefinition);
            }
            
            _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataProperties_EntityReference(DCAppDataFieldViewModel model)
        {
            try
            { 
            var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

            if (dataField.DataType == DataDefinitionDataType.EntityReference.Value.ToString())
            {
                dataField.DataDefinition = EntityReferenceDataDefinitionEntityBuilder
                    .BuildCreatedEntity((DCAppRefEntityDataDefinition)dataField.DataDefinition,
                    (DCAppDataTypeDefinition_EntityReferenceViewModel)model.DataDefinition);
            }

            _structureDBContext.SaveChanges();

                return new DCAppServiceMessage("Properties have been updated.", DCAppServiceResult.suceeded, model);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage GetDataProperties(string dataFieldId)
        {
            DCAppDataField dataField = _structureDBContext.DAppDataFields
              .Include(y => y.DataDefinition)
              .Include(y => y.DCAppDataModel)
              .FirstOrDefault(x => x.Id.ToString() == dataFieldId);
            DCAppDataFieldViewModel dataFieldViewModel = DataFieldEntityBuilder.BuildViewModel(dataField);

            if (dataField.DataDefinition is DCAppStringDataDefinition)
            {
                dataFieldViewModel.DataDefinition = StringDataDefinitionEntityBuilder.BuildViewModel((DCAppStringDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppRefEntityDataDefinition)
            {
                dataFieldViewModel.DataDefinition = EntityReferenceDataDefinitionEntityBuilder.BuildViewModel((DCAppRefEntityDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppBoolDataDefinition)
            {
                dataFieldViewModel.DataDefinition = BoolDataDefinitionEntityBuilder.BuildViewModel((DCAppBoolDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppChoiceDataDefinition)
            {
                dataFieldViewModel.DataDefinition = ChoiceDataDefinitionEntityBuilder.BuildViewModel((DCAppChoiceDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppNumberDataDefinition)
            {
                dataFieldViewModel.DataDefinition = NumberDataDefinitionEntityBuilder.BuildViewModel((DCAppNumberDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppDateTimeDataDefinition)
            {
                dataFieldViewModel.DataDefinition = DateTimeDataDefinitionEntityBuilder.BuildViewModel((DCAppDateTimeDataDefinition)dataField.DataDefinition);
            }
            else if (dataField.DataDefinition is DCAppFileDataDefinition)
            {
                dataFieldViewModel.DataDefinition = FileDataDefinitionEntityBuilder.BuildViewModel((DCAppFileDataDefinition)dataField.DataDefinition);
            }

            if (dataField != null)
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dataFieldViewModel);
            }
            else
            {
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed);
            }
        }

        #endregion

        #region ChoiceItems
        public DCAppServiceMessage CreateDataFieldChoiceItem(DCAppDataChoiceItemViewModel model)
        {
            try
            {
                var dataField = _structureDBContext.DAppDataFields.Include(y => y.DataDefinition).FirstOrDefault(x => x.Id == Guid.Parse(model.DataFieldId));
                var dataChoiceItem = EntityFactory.CreateNewDataChoiceItemModel();
                model.Id = dataChoiceItem.Id.ToString();

                dataChoiceItem.ChoiceGroup = model.ChoiceGroup;
                dataChoiceItem.Name = model.Name;

                ((DCAppChoiceDataDefinition)dataField.DataDefinition).Choices.Add(dataChoiceItem);


                _structureDBContext.DAppDataChoiceItems.Add(dataChoiceItem);
                _structureDBContext.SaveChanges();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, model);
            }
            catch(Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage UpdateDataFieldChoiceItem(DCAppDataChoiceItemViewModel model)
        {
            try
            {
                var dataChoiceItem = _structureDBContext.DAppDataChoiceItems.FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                dataChoiceItem.ChoiceGroup = model.ChoiceGroup;
                dataChoiceItem.Name = model.Name;

                _structureDBContext.SaveChanges();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, model);
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage RemoveDataFieldChoiceItem(DCAppDataChoiceItemViewModel model)
        {
            try
            {
                var dataChoiceItem = _structureDBContext.DAppDataChoiceItems.Include(x=>x.ChoiceParent).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                dataChoiceItem.ChoiceParent.Choices.Remove(dataChoiceItem);
                _structureDBContext.DAppDataChoiceItems.Remove(dataChoiceItem);
                _structureDBContext.SaveChanges();
                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, model);
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed);
            }
        }

        public DCAppServiceMessage GetDataFieldChoiceItems(DCAppDataFieldViewModel model)
        {
            try
            {
                var dataField = _structureDBContext.DAppDataFields.Include(x=>x.DataDefinition).ThenInclude(x => (x as DCAppChoiceDataDefinition).Choices).FirstOrDefault(x => x.Id == Guid.Parse(model.Id));

                if (dataField.DataType == DataDefinitionDataType.Choices.Value.ToString())
                {
                    var choices = ((DCAppChoiceDataDefinition)dataField.DataDefinition).Choices
                        .Select(x => new DCAppDataChoiceItemViewModel()
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            DataFieldId = dataField.Id.ToString(),
                            ChoiceGroup = x.ChoiceGroup,
                        }).ToList();
                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, choices);
                }
                return new DCAppServiceMessage("Fetching from a different data definition failed.", DCAppServiceResult.failed);
            }
            catch (Exception ex)
            {
                return new DCAppServiceMessage(ex.Message, DCAppServiceResult.failed);
            }
        }

        #endregion
    }
}