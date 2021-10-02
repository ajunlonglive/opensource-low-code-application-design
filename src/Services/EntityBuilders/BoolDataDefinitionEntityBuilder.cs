using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Presentation.ViewModels;
using Syncfusion.Pdf.Lists;
using System;

namespace Presentation.EntityBuilders
{
    public static class BoolDataDefinitionEntityBuilder
    {
        public static DCAppBoolDataDefinition BuildCreatedEntity(DCAppBoolDataDefinition entity, DCAppDataTypeDefinition_BoolViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            return entity;
        }

        public static DCAppDataTypeDefinition_BoolViewModel BuildViewModel(DCAppBoolDataDefinition entity)
        {
            return new DCAppDataTypeDefinition_BoolViewModel(entity)
            {
            };
        }

    }
}