using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class RoleRuleType : Enumeration<string>
    {
        public RoleRuleType(string value, string displayName) : base(value, displayName)
        {
        }

        public static RoleRuleType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<RoleRuleType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<RoleRuleType> GetAll()
        {
            yield return Unknown;
            yield return All;
            yield return AllAndChildType;
            yield return Every;
            yield return EveryAndChildTogetherType;
            yield return EveryAndChildSeparatelyType; // Only two of them sort of combination - ex. Performance report
        }

        public static readonly RoleRuleType Unknown = new RoleRuleType("Unknown", "Unknown");
        public static readonly RoleRuleType All = new RoleRuleType("All", "All");
        public static readonly RoleRuleType AllAndChildType = new RoleRuleType("AllAndChildType", "AllAndChildType");
        public static readonly RoleRuleType Every = new RoleRuleType("Every", "Every");
        public static readonly RoleRuleType EveryAndChildTogetherType = new RoleRuleType("EveryAndChildTogetherType", "EveryAndChildTogetherType");
        public static readonly RoleRuleType EveryAndChildSeparatelyType = new RoleRuleType("EveryAndChildSeparatelyType", "EveryAndChildSeparatelyType");
    }
}