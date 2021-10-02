using Domain.Abstractions;
using System;

namespace Domain.Entities
{
    public class DCAppDataDefinitionBase : Entity
    {
        public DCAppDataDefinitionBase(Guid id) : base(id)
        {
        }

        public bool AllowNullValue { get; set; }
        public bool AllowOnlyUniqueValue { get; set; }
    }
}