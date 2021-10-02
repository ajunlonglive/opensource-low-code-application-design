using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppRoleCollectionViewModel : BaseViewModel
    {
        public DCAppRoleCollectionViewModel(string id) : base(id)
        {
            Name = "Roles";
            Internal = new List<DCAppRoleViewModel>();
            External = new List<DCAppRoleViewModel>();
        }

        public ICollection<DCAppRoleViewModel> Internal { get; set; }
        public ICollection<DCAppRoleViewModel> External { get; set; }
    }
}