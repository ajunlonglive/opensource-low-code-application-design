using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppPage : Entity
    {
        public DCAppPage(Guid id) : base(id)
        {
            Controls = new HashSet<DCAppControl>();
        }

        public DCAppWorkFlow DCAppWorkFlow { get; set; }
        public Guid DCAppWorkFlowId { get; set; }
        public ICollection<DCAppControl> Controls { get; set; }
    }
}