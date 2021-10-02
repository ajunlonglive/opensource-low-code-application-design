using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Services
{
    public interface IGroupService
    {
        IEnumerable<SelectListItem> GetGroupsAsSelectListItems();

        DCAppServiceMessage GetGroupList(bool isInternal);

        DCAppServiceMessage CreateGroup(DCAppGroupViewModel model);

        DCAppServiceMessage UpdateGroup(DCAppGroupViewModel model);

        DCAppServiceMessage RemoveGroup(DCAppGroupViewModel model);
    }
}
