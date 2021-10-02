using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DCAppExternalGroup : DCAppGroup 
    {
        public DCAppExternalGroup(Guid id) : base(id)
        {
        }
    }
}