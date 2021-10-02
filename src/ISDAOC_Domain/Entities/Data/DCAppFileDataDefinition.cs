using System;

namespace Domain.Entities.Data
{
    public class DCAppFileDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppFileDataDefinition(Guid id) : base(id)
        {
        }
        public string Location { get; set; }
    }
}