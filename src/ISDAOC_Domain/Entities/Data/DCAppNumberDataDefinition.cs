using System;

namespace Domain.Entities.Data
{
    public class DCAppNumberDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppNumberDataDefinition(Guid id) : base(id)
        {
            DecimalPlaces = 2;
        }

        public bool HasDecimals { get; set; }

        public short DecimalPlaces { get; set; }
    }
}