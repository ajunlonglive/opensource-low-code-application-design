using System;
using System.Collections.Generic;
using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppRoleAccessGroupViewModel : BaseViewModel
    {
        public DCAppRoleAccessGroupViewModel()
        {
            Pages = new List<DCAppPageViewModel>();
        }
        public DCAppRoleAccessGroupViewModel(Entity entity) : base(entity)
        {
            Pages = new List<DCAppPageViewModel>();
        }

        public ICollection<DCAppPageViewModel> Pages { get; set; }


        public Guid FeatureId { get; set; }
    }
}