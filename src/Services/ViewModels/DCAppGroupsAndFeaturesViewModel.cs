using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppGroupsAndFeaturesViewModel
    {
        public DCAppGroupsAndFeaturesViewModel()
        {
            ChildGroups = new List<DCAppGroupViewModel>();
            Features = new List<DCAppFeatureViewModel>();
        }

        public List<DCAppGroupViewModel> ChildGroups { get; set; }

        public bool HasChildGroups
        {
            get
            {
                if (ChildGroups != null && ChildGroups.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public List<DCAppFeatureViewModel> Features { get; set; }
    }
}