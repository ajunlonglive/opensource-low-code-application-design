using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Services
{
    public class RoleService: IRoleService
    {
        private readonly StructureDBContext _structureDBContext;

        public RoleService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }
    }
}
