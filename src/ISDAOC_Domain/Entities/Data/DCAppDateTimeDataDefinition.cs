using System;

namespace Domain.Entities.Data
{
    public class DCAppDateTimeDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppDateTimeDataDefinition(Guid id) : base(id)
        {
            Format = "DD/MMM/YY";
        }

        public string Format { get; set; }
    }
}