using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using System;

namespace Presentation.EntityBuilders
{
    public static class StringDataDefinitionEntityBuilder
    {
        public static DCAppStringDataDefinition BuildCreatedEntity(DCAppStringDataDefinition entity, DCAppDataTypeDefinition_StringViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.Length = string.IsNullOrEmpty(model.Length)?(short)100 : short.Parse(model.Length);
           entity.IsMultiLine = model.IsMultiLine;
           entity.IsRegularExpressionFormat = model.IsRegularExpression;
            entity.Format = model.Format;
            return entity;
        }

        public static DCAppDataTypeDefinition_StringViewModel BuildViewModel(DCAppStringDataDefinition entity)
        {
            var model = new DCAppDataTypeDefinition_StringViewModel(entity);
            model.Length = entity.Length.ToString();
            model.IsMultiLine = entity.IsMultiLine;
            model.IsRegularExpression = entity.IsRegularExpressionFormat;
            model.Format = entity.Format;
            return model;
        }

    }
}