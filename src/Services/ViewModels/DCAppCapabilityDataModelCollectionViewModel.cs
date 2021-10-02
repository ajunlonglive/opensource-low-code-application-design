using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppCapabilityDataModelCollectionViewModel
    {
        public DCAppCapabilityDataModelCollectionViewModel()
        {
            CollectionViewModels = new List<DCAppCapabilityDataModelViewModel>();
        }

        public ICollection<DCAppCapabilityDataModelViewModel> CollectionViewModels { get; set; }
    }
}