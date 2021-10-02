using System;
using System.Collections.Generic;
using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppWorkFlowViewModel : BaseViewModel
    {
        public DCAppWorkFlowViewModel()
        {
            Pages = new List<DCAppPageViewModel>();
        }
        public DCAppWorkFlowViewModel(Entity entity) : base(entity)
        {
            Pages = new List<DCAppPageViewModel>();
        }

        public ICollection<DCAppPageViewModel> Pages { get; set; }

        public DCAppRoleAccessGroupViewModel RAG { get; set; }

        public Guid FeatureId { get; set; }

        public Guid DataRowId { get; set; }
        public string BreadCrumb { get; set; }

      
    }
}