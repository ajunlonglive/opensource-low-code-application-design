using System;
using System.Collections.Generic;

namespace Domain.Entities.Data
{
    public class DCAppChoiceDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppChoiceDataDefinition(Guid id) : base(id)
        {
            Choices = new HashSet<DCAppDataChoiceItem>();
        }

        public ICollection<DCAppDataChoiceItem> Choices { get; set; }

        public bool AllowMultipleSelection { get; set; }
    }
}