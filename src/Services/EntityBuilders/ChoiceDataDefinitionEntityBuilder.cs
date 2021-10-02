using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.ViewModels;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.EntityBuilders
{
    public static class ChoiceDataDefinitionEntityBuilder
    {
        public static DCAppChoiceDataDefinition BuildCreatedEntity(DCAppChoiceDataDefinition entity, DCAppDataTypeDefinition_ChoiceViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.AllowNullValue = model.AllowNullValue;
            entity.AllowOnlyUniqueValue = model.AllowOnlyUniqueValue;
            entity.AllowMultipleSelection = model.IsAllowMultipleSelection;
            
            return entity;
        }

        public static DCAppDataTypeDefinition_ChoiceViewModel BuildViewModel(DCAppChoiceDataDefinition entity)
        {

            var model = new DCAppDataTypeDefinition_ChoiceViewModel(entity)
            {
                IsAllowMultipleSelection = entity.AllowMultipleSelection
            };

            return model;
        }

    }
}