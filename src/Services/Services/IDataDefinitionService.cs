using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Presentation.Services
{
    public interface IDataDefinitionService
    {
        IEnumerable<SelectListItem> GetDataTypesAsSelectListItems();
    }
}