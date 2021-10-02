using Domain.Abstractions;
using System;

namespace Domain.Entities
{
    public class DCAppStructureInfo : Entity
    {
        public DCAppStructureInfo(Guid id) : base(id)
        {
        }

        public string Logo { get; set; }
        public string Payment { get; set; } //TODO
    }
}