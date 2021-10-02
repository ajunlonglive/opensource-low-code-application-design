using Syncfusion.EJ2.Base;
using System;
using System.Security.Principal;

namespace Presentation.ViewModels
{
    public class DataFieldAssistanceViewModel : DataManagerRequest
    {
        public DataFieldAssistanceViewModel()
        {
        }

        public Guid StructureId { get; set; }
        public Guid HubId { get; set; }

        public Guid DataModelId { get; set; }

        public Guid DataFieldId { get; set; }
        public IPrincipal Principal { get; set; }
    }
}