using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppRoleViewModel : BaseViewModel
    {
        public DCAppRoleViewModel(string guid) : base(guid)
        {
            ChildRoles = new List<DCAppRoleViewModel>();
            Users = new List<DCAppUserViewModel>();
        }

        public List<DCAppRoleViewModel> ChildRoles { get; set; }

        public bool HasChildRoles
        {
            get
            {
                if (ChildRoles != null && ChildRoles.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public List<DCAppUserViewModel> Users { get; set; }
    }
}