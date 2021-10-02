using Syncfusion.EJ2.Base;
using System;
using System.Security.Principal;

namespace Presentation.ViewModels
{
    public class GroupAssistanceViewModel : DataManagerRequest
    {
        public GroupAssistanceViewModel()
        {
        }

        public IPrincipal Principal { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid GroupId { get; set; }

        public Guid StructureId { get; set; }

        public bool IsInternal { get; set; }
    }
}