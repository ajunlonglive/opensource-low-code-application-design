using System;

namespace Domain.Entities
{
    public class DCAppInternalRole : DCAppRole
    {
        public DCAppInternalRole(Guid id) : base(id)
        {
            IsInternal = true;
        }
    }
}