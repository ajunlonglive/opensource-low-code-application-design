using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Messaging
{
    public class StringInterpolator
    {
        private readonly Dictionary<string, string> _replacementLookupTable;

        public StringInterpolator()
        {
            _replacementLookupTable = new Dictionary<string, string>();
        }

        public void Add(string variableName, string replacementValue)
        {
            var searchString = variableName.StartsWith("@") ? variableName : $"@{variableName}";
            _replacementLookupTable.Add(searchString, replacementValue);
        }

        public string Interpolate(string source)
        {
            var builder = new StringBuilder(source);

            foreach (var key in _replacementLookupTable.Keys)
            {
                builder.Replace(key, _replacementLookupTable[key]);
            }

            return builder.ToString();
        }
    }
}