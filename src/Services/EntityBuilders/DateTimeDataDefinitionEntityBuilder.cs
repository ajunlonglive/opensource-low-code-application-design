using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class DateTimeDataDefinitionEntityBuilder
    {
        public static DCAppDateTimeDataDefinition BuildCreatedEntity(DCAppDateTimeDataDefinition entity, DCAppDataTypeDefinition_DateTimeViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.Format = model.Format;
            return entity;
        }

        public static DCAppDataTypeDefinition_DateTimeViewModel BuildViewModel(DCAppDateTimeDataDefinition entity)
        {
            var model = new DCAppDataTypeDefinition_DateTimeViewModel(entity);
            model.Format = entity.Format;
            return model;
        }

    }
}