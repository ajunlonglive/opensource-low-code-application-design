using Domain.Abstractions;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_ChoiceViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_ChoiceViewModel()
        {
        }
        public DCAppDataTypeDefinition_ChoiceViewModel(Entity entity) :base(entity)
        {
        }

        public bool IsAllowMultipleSelection { get; set; }
        public ClaimsPrincipal Principal { get; set; }
    }

    public class DCAppDataTypeDefinition_ChoiceHelperViewModel : DataManagerRequest
    {
        public DCAppDataTypeDefinition_ChoiceHelperViewModel()
        {
            ChoiceViewModel = new DCAppDataTypeDefinition_ChoiceViewModel();
        }
        public DCAppDataTypeDefinition_ChoiceViewModel ChoiceViewModel {get;set;}
    }

}