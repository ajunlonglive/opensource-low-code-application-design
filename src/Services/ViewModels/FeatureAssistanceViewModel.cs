using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Presentation.ViewModels
{
    public class FeatureAssistanceViewModel : GroupAssistanceViewModel
    {
        public FeatureAssistanceViewModel()
        {
        }

        public Guid FeatureId { get; set; }

        public Guid DataRowId { get; set; }
        public Guid WorkFlowId { get; set; }

        public DCAppWorkFlowViewModel WorkFlow { get; set; }
        public ICollection<DCAppFeatureViewModel> Features { get; set; }
        public Guid AddNewPageId { get; set; }
    }
}