using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppUserCollectionViewModel : BaseViewModel
    {
        public DCAppUserCollectionViewModel(string id) : base(id)
        {
            Name = "Users";
            Internal = new List<DCAppUserViewModel>();
            External = new List<DCAppUserViewModel>();
        }

        public List<DCAppUserViewModel> Internal { get; set; }
        public List<DCAppUserViewModel> External { get; set; }
    }
}