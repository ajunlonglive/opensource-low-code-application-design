using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppGroup : Entity
    {
        public DCAppGroup(Guid id) : base(id)
        {
            ChildGroups = new HashSet<DCAppGroup>();
            Features = new HashSet<DCAppFeature>();
            Roles = new HashSet<DCAppRole>();
            DataModels = new HashSet<DCAppDataModel>();
            CapabilityDataModels = new HashSet<DCAppCapabilityDataModel>();
        }
        public bool IsInternal { get; set; }
        public DCAppGroup ParentGroup { get; set; }
        public Guid? ParentGroupId { get; set; }
        public ICollection<DCAppGroup> ChildGroups { get; set; }
        public bool HasChildGroups
        {
            get
            {
                if (ChildGroups != null && ChildGroups.Count > 0)
                    return true;
                else
                    return false;
            }
        }
        public ICollection<DCAppRole> Roles { get; set; }
      
        public ICollection<DCAppFeature> Features { get; set; }

        public ICollection<DCAppDataModel> DataModels { get; set; }
        public ICollection<DCAppCapabilityDataModel> CapabilityDataModels { get; set; }
    }
}