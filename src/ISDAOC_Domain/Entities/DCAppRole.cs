using Domain.Abstractions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppRole : Entity
    {
        public DCAppRole(Guid id) : base(id)
        {
        }
        public bool IsInternal { get; set; }
        public DCAppGroup Group { get; set; }
        public Guid? GroupId { get; set; }
        public bool IsSystemDefined { get; set; }

        public SystemRoleType SystemRole { get; set; }
    }
}