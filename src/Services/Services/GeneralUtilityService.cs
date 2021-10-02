using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Presentation.Services
{
    public class GeneralUtilityService : IGeneralUtilityService
    {
        public object RemoveSpacesForValues(object valueObject)
        {
            foreach (PropertyInfo prop in valueObject.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(string))
                {
                    if (prop.GetValue(valueObject, null) != null)
                    {
                        var oldValue = prop.GetValue(valueObject, null).ToString();
                        if (!string.IsNullOrEmpty(oldValue))
                            prop.SetValue(valueObject, oldValue.Trim());
                    }
                }
            }
            return valueObject;
        }

        public string ReplaceSpaceWithUnderscore(string raw)
        {
            if (!string.IsNullOrEmpty(raw))
                raw = raw.Replace(' ', '_');
            return raw;
        }
    }
}
