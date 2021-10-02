using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Data
{
    public class DCAppDataChoiceItem : Entity
    {
        public DCAppDataChoiceItem(Guid id) : base(id)
        {
        }

        public DCAppChoiceDataDefinition ChoiceParent { get; set; }
        public Guid ChoiceParentId { get; set; }

        public string ChoiceGroup { get; set; }
    }
}
