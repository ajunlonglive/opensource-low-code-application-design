using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppUserCollection : Entity
    {
        public DCAppUserCollection(Guid id) : base(id)
        {
            Internal = new HashSet<DCAppInternalUser>();
            External = new HashSet<DCAppExternalUser>();
        }

        public ICollection<DCAppInternalUser> Internal { get; set; }
        public ICollection<DCAppExternalUser> External { get; set; }
    }
}