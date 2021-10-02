using Domain.Abstractions;
using Domain.Common;
using System;

namespace Domain.Entities
{
    public class DCAppCapability : Entity
    {
        public DCAppCapability(Guid id) : base(id)
        {
        }

        public DCAppCapabilityType CapabilityType { get; set; }

        public DCAppCapabilityDataModel Data { get; set; }

        /* public DCAppCapabilityActionResult PerformAction() // to be defined in child classes
         {
         }
         */
    }
}