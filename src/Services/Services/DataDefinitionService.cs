using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Services
{
    public class DataDefinitionService : IDataDefinitionService
    {
        private readonly StructureDBContext _structureDBContext;

        public DataDefinitionService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }

        public IEnumerable<DataDefinitionDataType> GetAllDataTypes()
        {
            return DataDefinitionDataType.GetAllDataTypes();
        }

        public IEnumerable<SelectListItem> GetDataTypesAsSelectListItems()
        {
            return GetAllDataTypes()
              .Select(c => new SelectListItem { Text = c.DisplayName, Value = c.Value })
              .ToList();
        }      
    }
}