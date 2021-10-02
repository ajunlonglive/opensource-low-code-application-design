using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppStructureOwner : Entity
    {
        public DCAppStructureOwner(Guid id) : base(id)
        {
            Structures = new HashSet<DCAppStructure>();
        }

        public ICollection<DCAppStructure> Structures { get; set; }
    }
}