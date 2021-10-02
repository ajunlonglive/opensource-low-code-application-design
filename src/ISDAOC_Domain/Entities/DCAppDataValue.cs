using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppDataValue : Entity
    {
        public DCAppDataValue(Guid id) : base(id)
        {
            MultipleReferenceRowIds = new List<Guid>();
        }

        public ICollection<Guid> MultipleReferenceRowIds { get; set; }

        public Guid RowId { get; set; }
        public DCAppDataField DataField { get; set; }

        public Guid SingleReferenceRowId { get; set; }
        public DCAppDataField BaseDataField { get; set; }

        public DCAppDataModel BaseDataModel { get; set; }
        public string Value { get; set; }
    }
}