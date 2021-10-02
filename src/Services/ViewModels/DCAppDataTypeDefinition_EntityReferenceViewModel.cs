using Domain.Abstractions;
using Presentation.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_EntityReferenceViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_EntityReferenceViewModel()
        {
        }
        public DCAppDataTypeDefinition_EntityReferenceViewModel(Entity entity) : base(entity)
        {
        }

        [NotDefault("Reference Data-Table is required")]
        public Guid RefDataModelId { get; set; }

        public bool IsSingleRecord { get; set; }
    }
}