using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppFeatureViewModel : BaseViewModel
    {
        public DCAppFeatureViewModel()
        {
            WorkFlows = new List<DCAppWorkFlowViewModel>();
        }
        public DCAppFeatureViewModel(Entity entity) : base(entity)
        {
            WorkFlows = new List<DCAppWorkFlowViewModel>();
        }

        public ICollection<DCAppWorkFlowViewModel> WorkFlows { get; set; }

        public int TotalItemsCount => WorkFlows.Count;

        
        public Guid GroupId { get; set; }
    }
}