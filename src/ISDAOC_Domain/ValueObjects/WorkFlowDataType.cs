using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class DataRowAccessType : Enumeration<string>
    {
        public DataRowAccessType(string value, string displayName) : base(value, displayName)
        {
        }

        public static DataRowAccessType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<DataRowAccessType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<DataRowAccessType> GetAll()
        {
            yield return Unknown;
            yield return Self;
            yield return Role;
            yield return SystemDefinition;
        }

        public static readonly DataRowAccessType Unknown = new DataRowAccessType("Unknown", "Unknown");
        public static readonly DataRowAccessType Self = new DataRowAccessType("Self", "Self");
        public static readonly DataRowAccessType Role = new DataRowAccessType("Role", "Role");
        public static readonly DataRowAccessType SystemDefinition = new DataRowAccessType("SystemDefinition", "SystemDefinition");
    }
}