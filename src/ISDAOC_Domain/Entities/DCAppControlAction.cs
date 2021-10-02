using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppControlAction : Entity
    {
        public DCAppControlAction(Guid id) : base(id)
        {
            Capabilities = new HashSet<DCAppCapability>();
        }

        public ICollection<DCAppCapability> Capabilities { get; set; }
    }
}