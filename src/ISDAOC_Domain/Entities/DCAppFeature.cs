using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppFeature : Entity
    {
        public DCAppFeature(Guid id) : base(id)
        {
            WorkFlows = new HashSet<DCAppWorkFlow>();
        }
        public bool IsInternal { get; set; }
        public DCAppGroup Group { get; set; }
        public Guid? GroupId { get; set; }

        public ICollection<DCAppWorkFlow> WorkFlows { get; set; }
    }
}