using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppGroupViewModel : BaseViewModel
    {
        public DCAppGroupViewModel() 
        {
            ChildGroups = new List<DCAppGroupViewModel>();
        }

        public DCAppGroupViewModel(Entity entity) : base(entity)
        {
            ChildGroups = new List<DCAppGroupViewModel>();
        }


        public Guid? ParentGroupId { get; set; }

        public List<DCAppGroupViewModel> ChildGroups { get; set; }

        public short RoleCount { get; set; }
        public short FeatureCount { get; set; }

        public short DataModelCount { get; set; }
        public short CapabilityDataModelCount { get; set; }

        public bool HasChildren { get; set; }
    }
}