using Domain.Abstractions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppRolePermission : Entity
    {
        public DCAppRolePermission(Guid id) : base(id)
        { }

        public AccessLevelType AccessLevel { get; set; }

        public ICollection<DCAppRoleRule> RoleRules { get; set; }

        public Guid WorkFlowId { get; set; }
        public DCAppWorkFlow WorkFlow { get; set; }

        public ICollection<DCAppDataField> RestrictAccessFields { get; set; }
    }
}