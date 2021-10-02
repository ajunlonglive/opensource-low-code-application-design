using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Presentation.Services
{
    public interface IGeneralUtilityService
    {
        object RemoveSpacesForValues(object valueObject);
        string ReplaceSpaceWithUnderscore(string raw);
    }
}
