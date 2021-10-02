using System;

namespace Presentation.ViewModels
{
    public class DCAppStructureViewModel : BaseViewModel
    {
        public DCAppStructureViewModel(string id) : base(id)
        {
            Users = new DCAppUserCollectionViewModel(Guid.NewGuid().ToString());
            Roles = new DCAppRoleCollectionViewModel(Guid.NewGuid().ToString());
            Hub = new DCAppHubViewModel(Guid.NewGuid().ToString());
        }

        public DCAppUserCollectionViewModel Users { get; set; }
        public DCAppRoleCollectionViewModel Roles { get; set; }
        public DCAppHubViewModel Hub { get; set; }
    }
}