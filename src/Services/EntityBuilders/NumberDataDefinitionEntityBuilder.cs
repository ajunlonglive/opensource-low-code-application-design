using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class NumberDataDefinitionEntityBuilder
    {
        public static DCAppNumberDataDefinition BuildCreatedEntity(DCAppNumberDataDefinition entity, DCAppDataTypeDefinition_NumberViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.HasDecimals = model.HasDecimals;
            entity.DecimalPlaces = string.IsNullOrEmpty(model.DecimalPlaces) ? (short)2 : short.Parse(model.DecimalPlaces);
            return entity;
        }

        public static DCAppDataTypeDefinition_NumberViewModel BuildViewModel(DCAppNumberDataDefinition entity)
        {
            var model = new DCAppDataTypeDefinition_NumberViewModel(entity);
            model.HasDecimals = entity.HasDecimals;
            model.DecimalPlaces = entity.DecimalPlaces.ToString();
            return model;
        }

    }
}