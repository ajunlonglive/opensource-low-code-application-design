using System;

namespace Domain.Entities.Data
{
    public class DCAppStringDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppStringDataDefinition(Guid id) : base(id)
        {
            Length = 100;
        }

        public short Length { get; set; }
        public bool IsMultiLine { get; set; }

        public string Format { get; set; }
        public bool IsRegularExpressionFormat { get; set; }
    }
}