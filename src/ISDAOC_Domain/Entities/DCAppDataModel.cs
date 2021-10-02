using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppDataModel : Entity
    {
        public DCAppDataModel(Guid id) : base(id)
        {
            DataFields = new HashSet<DCAppDataField>();
        }
        public bool IsSystemBased { get; set; }
        public bool IsInternal { get; set; }
        public Guid? GroupId { get; set; }

        public DCAppGroup Group { get; set; }

        public bool IsSingleRecord { get; set; }
        public ICollection<DCAppDataField> DataFields { get; set; }
    }
}