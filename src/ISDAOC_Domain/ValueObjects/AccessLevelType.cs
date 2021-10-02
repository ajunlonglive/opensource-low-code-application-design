using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class AccessLevelType : Enumeration<string>
    {
        public AccessLevelType(string value, string displayName) : base(value, displayName)
        {
        }

        public static AccessLevelType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<AccessLevelType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<AccessLevelType> GetAll()
        {
            yield return Unknown;
            yield return Full;
            yield return Read;
            yield return Write;
            yield return Create;
            yield return Delete;
        }

        public static readonly AccessLevelType Unknown = new AccessLevelType("Unknown", "Unknown");
        public static readonly AccessLevelType Full = new AccessLevelType("Full", "Full");
        public static readonly AccessLevelType Read = new AccessLevelType("Read", "Read");
        public static readonly AccessLevelType Write = new AccessLevelType("Write", "Write");
        public static readonly AccessLevelType Create = new AccessLevelType("Create", "Create");
        public static readonly AccessLevelType Delete = new AccessLevelType("Delete", "Delete");
    }
}