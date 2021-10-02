using Domain.Entities;
using Domain.Entities.Data;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.EntityBuilders;
using Presentation.Services;
using Presentation.ViewModels;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISDAOC_Web.Designer.Controllers
{
    [Area("Designer")]
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IStructureService _structureService;
        private readonly IGroupService _groupService;
        private readonly StructureDBContext _dBContext;
        private IGeneralUtilityService _generalUtilityService;

        public GroupsController(StructureDBContext context,
            IGroupService groupService, IGeneralUtilityService generalUtilityService)
        {
            _dBContext = context;
            _groupService = groupService;
            _generalUtilityService = generalUtilityService;
        }


        #region DataModels

      
        [HttpPost]
        public object GetGroupList([FromBody]DataModelAssistanceViewModel model)
        {
            model.Principal = User;
            var serviceMessage = _groupService.GetGroupList(model.IsInternal);
            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                IEnumerable<DCAppGroupViewModel> DataSource = ((IEnumerable<DCAppGroupViewModel>)serviceMessage.Data).ToList();
                return Json(DataSource);
            }   
            else // failed case
            {
                return Json(new { result = "", count = 0 });
            }
        }

        [HttpPost]
        public IActionResult UpdateGroup([FromBody]DCAppGroupViewModel data)
        {
            var message = _groupService.UpdateGroup(data);

            if (message.Result == DCAppServiceResult.suceeded)
            {
                data = (DCAppGroupViewModel)message.Data;

            }

            return Json(data);
        }

        [HttpPost]
        public IActionResult DialogUpdateGroup([FromBody] DCAppGroupViewModel value)
        {
            return PartialView("_DialogUpdateDataModel", value);
        }

        [HttpPost]
        public IActionResult DialogCreateDataModel([FromBody] DCAppDataModelViewModel value)
        {
            return PartialView("_DialogCreateDataModel");
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody]DCAppGroupViewModel value)
        {
          
            var message = _groupService.CreateGroup(value);

            if (message.Result == DCAppServiceResult.suceeded)
            {
                value = (DCAppGroupViewModel)message.Data;

            }

            return Json(value);
        }

        [HttpPost]
        public IActionResult RemoveGroup([FromBody] DCAppGroupViewModel value)
        {
            var message = _groupService.RemoveGroup(value);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                value  = (DCAppGroupViewModel)message.Data;
            }
            return Json(value);
        }

        #endregion
      
    }
}