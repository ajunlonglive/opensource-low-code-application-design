using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppStructure : Entity
    {
        public DCAppStructure(Guid id) : base(id)
        {
            URLs = new HashSet<DCAppCanonicalURL>();
            Internal = new HashSet<DCAppInternalUser>();
            External = new HashSet<DCAppExternalUser>();
        }

        public DateTime Validity { get; set; }

        public ICollection<DCAppCanonicalURL> URLs { get; set; }

        
        public DCAppInternalGroup InternalGroup { get; set; }
        public DCAppExternalGroup ExternalGroup { get; set; }

        public ICollection<DCAppInternalUser> Internal { get; set; }
        public ICollection<DCAppExternalUser> External { get; set; }
    }
}