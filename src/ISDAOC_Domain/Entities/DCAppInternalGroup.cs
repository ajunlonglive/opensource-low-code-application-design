using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppInternalGroup: DCAppGroup 
    {
        public DCAppInternalGroup(Guid id) : base(id)
        {
            IsInternal = true;
        }

       
    }
}