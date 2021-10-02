using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppHubViewModel : BaseViewModel
    {
        public DCAppHubViewModel(string id) : base(id)
        {
            Name = "Hub";
            DataModels = new List<DCAppDataModelViewModel>();
            CapabilityDataModels = new List<DCAppCapabilityDataModelViewModel>();
            Features = new List<DCAppFeatureViewModel>();
            Groups = new List<DCAppGroupViewModel>();
            AccessPermissions = new DCAppPermissionViewModel(Guid.NewGuid().ToString());
        }

        public List<DCAppDataModelViewModel> DataModels { get; set; }
        public List<DCAppCapabilityDataModelViewModel> CapabilityDataModels { get; set; }
        public List<DCAppFeatureViewModel> Features { get; set; }
        public DCAppPermissionViewModel AccessPermissions { get; set; }
        public List<DCAppGroupViewModel> Groups { get; set; }

        public bool HasChildGroups
        {
            get
            {
                if (Groups != null && Groups.Count > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}