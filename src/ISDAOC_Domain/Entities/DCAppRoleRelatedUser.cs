using Domain.Abstractions;
using Domain.ValueObjects;
using System;

namespace Domain.Entities
{
    public class DCAppRoleRelatedUser : Entity
    {
        public DCAppRoleRelatedUser(Guid id) : base(id)
        {
        }

        public DCAppUser Player { get; set; }
        public Guid PlayerId { get; set; }

        public DCAppUser RelatedUser { get; set; }

        public RoleRelationType Relation {get;set;}
    }
}