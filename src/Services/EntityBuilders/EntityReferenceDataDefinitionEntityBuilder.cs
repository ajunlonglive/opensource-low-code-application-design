using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class EntityReferenceDataDefinitionEntityBuilder
    {
        public static DCAppRefEntityDataDefinition BuildCreatedEntity(DCAppRefEntityDataDefinition entity, DCAppDataTypeDefinition_EntityReferenceViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.IsSingleRecord = model.IsSingleRecord;
           entity.RefDataModelId = model.RefDataModelId;
            return entity;
        }

        public static DCAppDataTypeDefinition_EntityReferenceViewModel BuildViewModel(DCAppRefEntityDataDefinition entity)
        {
            var model = new DCAppDataTypeDefinition_EntityReferenceViewModel(entity);
            model.IsSingleRecord = entity.IsSingleRecord;
            model.RefDataModelId = entity.RefDataModelId;
            return model;
        }

    }
}