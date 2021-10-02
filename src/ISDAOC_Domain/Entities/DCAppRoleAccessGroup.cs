using Domain.Abstractions;
using System;
using System.Dynamic;

namespace Domain.Entities
{
    public class DCAppRoleAccessGroup : Entity
    {
        public DCAppRoleAccessGroup(Guid id) : base(id)
        {
        }

        public DCAppRole ParentRole { get; set; }
        public DCAppUser ParentUser { get; set; }
        public DCAppWorkFlow WorkFlow { get; set; }
        public Guid WorkFlowId { get; set; }

        public bool IsSource { get; set; }

    }
}