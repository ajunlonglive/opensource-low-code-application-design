using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class FileDataDefinitionEntityBuilder
    {
        public static DCAppFileDataDefinition BuildCreatedEntity(DCAppFileDataDefinition entity, DCAppDataTypeDefinition_FileViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.Location = model.Location;
            return entity;
        }

        public static DCAppDataTypeDefinition_FileViewModel BuildViewModel(DCAppFileDataDefinition entity)
        {
            var model = new DCAppDataTypeDefinition_FileViewModel(entity);
            model.Location = entity.Location;
            return model;
        }

    }
}