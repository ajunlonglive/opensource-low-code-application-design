using Domain.Abstractions;
using Domain.ValueObjects;
using System;

namespace Domain.Entities
{
    public class DCAppRoleRule : Entity
    {
        public DCAppRoleRule(Guid id) : base(id)
        {
        }

        public RoleRuleType RuleType { get; set; }

        public DCAppRole ParentRole { get; set; }

        public DCAppRole ChildRole { get; set; }
    }
}