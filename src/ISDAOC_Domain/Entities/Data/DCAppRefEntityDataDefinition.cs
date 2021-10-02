using System;
using System.Collections.Generic;

namespace Domain.Entities.Data
{
    public class DCAppRefEntityDataDefinition : DCAppDataDefinitionBase
    {
        public DCAppRefEntityDataDefinition(Guid id) : base(id)
        {
        }

        public Guid RefDataModelId { get; set; }
        public DCAppDataModel RefDataModel { get; set; }
        public bool IsSystemBuilt {get;set;}

        public string SystemTableName { get; set; }
        public bool IsSingleRecord { get; set; }
    }
}