using Domain.Abstractions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DCAppWorkFlow : Entity
    {
        public DCAppWorkFlow(Guid id) : base(id)
        {
            Pages = new HashSet<DCAppPage>();
        }
        public Guid DCAppFeatureId { get; set; }

        public DCAppFeature DCAppFeature { get; set; }

        public ICollection<DCAppPage> Pages { get; set; }

        public ICollection<DCAppRolePermission> RoleAccessPermissions { get; set; }

        public ICollection<DCAppRoleAccessGroup> AccessGroups { get; set; }

        public bool IsSingleRecord { get; set; }

        public DataRowAccessType RowAccessType { get; set; }

        [NotMapped]
        public ICollection<DCAppDataRow> DataRows { get; set; }

        public Guid DCAppDataModelId { get; set; }
    }
}