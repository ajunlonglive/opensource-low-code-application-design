using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppDataModelCollectionViewModel
    {
        public DCAppDataModelCollectionViewModel()
        {
            CollectionViewModels = new List<DCAppDataModelViewModel>();
        }

        public ICollection<DCAppDataModelViewModel> CollectionViewModels { get; set; }
    }
}