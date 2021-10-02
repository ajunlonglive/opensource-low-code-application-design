using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class SystemRoleType : Enumeration<string>
    {
        public SystemRoleType(string value, string displayName) : base(value, displayName)
        {
        }

        public static SystemRoleType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<SystemRoleType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<SystemRoleType> GetAll()
        {
            yield return Unknown;
            yield return Platform_Admin;
            yield return Feature_Designer;
            yield return Feature_Manager;
            yield return Data_Manager;
        }

        public static readonly SystemRoleType Unknown = new SystemRoleType("Unknown", "Unknown");
        public static readonly SystemRoleType Platform_Admin = new SystemRoleType("Platform_Admin", "Platform_Admin");
        public static readonly SystemRoleType Feature_Designer = new SystemRoleType("Feature_Designer", "Feature_Designer");
        public static readonly SystemRoleType Feature_Manager = new SystemRoleType("Feature_Manager", "Feature_Manager");
        public static readonly SystemRoleType Data_Manager = new SystemRoleType("Data_Manager", "Data_Manager");
    }
}