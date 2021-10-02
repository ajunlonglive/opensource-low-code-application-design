using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DCAppControl : Entity
    {
        public DCAppControl(Guid id) : base(id)
        {
            Actions = new HashSet<DCAppControlAction>();
            Properties = new HashSet<DCAppControlProperty>();
        }

        public DCAppPage DCAppPage { get; set; }

        public Guid DCAppPageId { get; set; }

        public ICollection<DCAppControlAction> Actions { get; set; }

        public DCAppDataField DataField { get; set; }

        public ICollection<DCAppControlProperty> Properties { get; set; }

        [NotMapped]
        public string Value { get; set; }
    }
}