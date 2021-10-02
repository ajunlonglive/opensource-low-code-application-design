using Syncfusion.EJ2.Base;
using System;
using System.Security.Principal;

namespace Presentation.ViewModels
{
    public class DataModelAssistanceViewModel : GroupAssistanceViewModel
    {
        public DataModelAssistanceViewModel()
        {
        }
       
        public Guid DataModelId { get; set; }

        public Guid DataFieldId { get; set; }

        public bool IsCapability { get; set; }
    }
}