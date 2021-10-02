using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppRoleCollection : Entity
    {
        public DCAppRoleCollection(Guid id) : base(id)
        {
            Internal = new HashSet<DCAppInternalRole>();
            External = new HashSet<DCAppExternalRole>();
        }

        public ICollection<DCAppInternalRole> Internal { get; set; }
        public ICollection<DCAppExternalRole> External { get; set; }
    }
}