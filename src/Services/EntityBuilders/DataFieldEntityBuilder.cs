using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class DataFieldEntityBuilder
    {
        public static DCAppDataField BuildCreatedEntity(DCAppDataField entity, DCAppDataFieldViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity = UpdateDataPropertyDefinition(entity, model.DataType);
            entity.DataType = model.DataType;
            return entity;
        }

        public static DCAppDataFieldViewModel BuildViewModel(DCAppDataField entity)
        {
            var model = new DCAppDataFieldViewModel(entity);
            model.DataType = entity.DataType;
            model.DataModelId = entity.DCAppDataModelId.ToString();

            return model;
        }

        private static DCAppDataField UpdateDataPropertyDefinition(DCAppDataField dataField, string dataType)
        {
            if (dataField.DataType != dataType)
            {
                if (DataDefinitionDataType.String.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppStringDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.Bool.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppBoolDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.Number.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppNumberDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.DateTime.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppDateTimeDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.Choices.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppChoiceDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.File.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppFileDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.EntityReference.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = new DCAppRefEntityDataDefinition(Guid.NewGuid());
                }
                else if (DataDefinitionDataType.Unknown.Value.ToString() == dataType)
                {
                    dataField.DataDefinition = null;
                }
            }

            return dataField;
        }
    }
}