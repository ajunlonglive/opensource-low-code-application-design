using System.Collections.Generic;
using System.Linq;

namespace Domain.ValueObjects
{
    public class DataDefinitionDataType : Enumeration<string>
    {
        public DataDefinitionDataType(string value, string displayName) : base(value, displayName)
        {
        }

        public static DataDefinitionDataType GetByValue(string value)
        {
            return GetAll().FirstOrDefault(x => x.Value == value);
        }

        public static IEnumerable<DataDefinitionDataType> GetAllDataTypes()
        {
            return GetAll();
        }

        public static IEnumerable<DataDefinitionDataType> GetAll()
        {
            yield return Unknown;
            yield return String;
            yield return Bool;
            yield return Number;
            yield return DateTime;
            yield return Choices;
            yield return File;
            yield return EntityReference;
        }

        public static readonly DataDefinitionDataType Unknown = new DataDefinitionDataType("Unknown", "Unknown");
        public static readonly DataDefinitionDataType String = new DataDefinitionDataType("String", "String"); // length, Multiline
        public static readonly DataDefinitionDataType Bool = new DataDefinitionDataType("Bool", "Bool");
        public static readonly DataDefinitionDataType Number = new DataDefinitionDataType("Number", "Number");// decimal
        public static readonly DataDefinitionDataType DateTime = new DataDefinitionDataType("DateTime", "DateTime");// decimal
        public static readonly DataDefinitionDataType Choices = new DataDefinitionDataType("Choices", "Choices"); // AllowMultipleSelection
        public static readonly DataDefinitionDataType File = new DataDefinitionDataType("File", "File");
        public static readonly DataDefinitionDataType EntityReference = new DataDefinitionDataType("EntityReference", "EntityReference");
    }
}