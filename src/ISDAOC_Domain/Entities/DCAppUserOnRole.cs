using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppUserOnRole : Entity
    {
        public DCAppUserOnRole(Guid id) : base(id)
        {
            ReportsTo = new List<DCAppUserRole>();
        }
        public Guid UserId { get; set; }

        public DCAppUserRole User { get; set; }

        public ICollection<DCAppUserRole> ReportsTo { get; set; }
    }
}