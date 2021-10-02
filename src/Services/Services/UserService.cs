using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Services
{
    public class UserService : IUserService
    {
        private readonly StructureDBContext _structureDBContext;

        public UserService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }
    }
}
