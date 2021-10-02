using Domain.Abstractions;
using Newtonsoft.Json;
using System;

namespace Domain.Entities
{
    public class DCAppDataField : Entity
    {
        public DCAppDataField(Guid id) : base(id)
        {
        }

        public string DataType { get; set; }
        public DCAppDataDefinitionBase DataDefinition { get; set; }
        public Guid DCAppDataModelId { get; set; }

        
        public DCAppDataModel DCAppDataModel { get; set; }
    }
}