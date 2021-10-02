using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class RoleRelationType : Enumeration<string>
    {
        public RoleRelationType(string value, string displayName) : base(value, displayName)
        {
        }

        public static RoleRelationType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<RoleRelationType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<RoleRelationType> GetAll()
        {
            yield return Unknown;
            yield return ReportsTo;
            yield return Reportee;
            //yield return Peer;
        }

        public static readonly RoleRelationType Unknown = new RoleRelationType("Unknown", "Unknown");
        public static readonly RoleRelationType ReportsTo = new RoleRelationType("ReportsTo", "ReportsTo");
        public static readonly RoleRelationType Reportee = new RoleRelationType("Reportee", "Reportee");
    }
}