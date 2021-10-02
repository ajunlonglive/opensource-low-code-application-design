using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppUserRole : IAssociationEntity
    {
        public DCAppUserRole()//(Guid Id) : base(Id)
        {
            ReportsTo = new HashSet<DCAppUserRole>();
    }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public DCAppUser User { get; set; }
        public DCAppRole Role { get; set; }

        public ICollection<DCAppUserRole> ReportsTo { get; set; }
    }
}