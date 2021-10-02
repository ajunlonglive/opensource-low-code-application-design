using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppStructureListViewModel
    {
        public DCAppStructureListViewModel()
        {
            Structures = new List<DCAppStructureViewModel>();
        }

        public ICollection<DCAppStructureViewModel> Structures { get; set; }
    }
}