using Domain.Entities;
using Infrastructure.Data;
using ISDAOC_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Services;
using Presentation.ViewModels;
using Presentation.Utility;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ISDAOC_Web.Designer.Controllers
{
    [Area("Designer")]
    [Authorize]
    public class StructureController : Controller
    {
        private readonly IStructureService _structureService;
        private readonly IDataModelService _dataModelService;
        private readonly ViewRenderService _viewRenderService;
        private readonly StructureDBContext _dBContext;
        private readonly IGroupService _groupService;
        private readonly UserManager<DCAppUser> _userManager;

        public StructureController
            (StructureDBContext dBContext,
            IDataModelService dataModelService,
            IStructureService structureService,
            IGroupService groupService,
            ViewRenderService viewRenderService,
            UserManager<DCAppUser> userManager)
        {
            _structureService = structureService;
            _dataModelService = dataModelService;
            _groupService = groupService;
            _viewRenderService = viewRenderService;
            _dBContext = dBContext;
            _userManager = userManager;
        }

       
        // GET: Structure
        [HttpPost]
        public IActionResult Index()
        {
            var user = _userManager.GetUserName(User);
            if (user == "johnmangam@gmail.com" || user == "emmyjstephen@gmail.com" || user.EndsWith("iemoutreach.org"))
            {
                var structureListModel = new DCAppStructureListViewModel();
                return View("Index", structureListModel);
            }
            else
                return View("AccessDenied");
           
        }

        //// GET: Structure
        //[HttpGet]
        //[HttpPost]
        //[ActionName("GetAllData")]
        //public IEnumerable<ParentNodeViewModel> GetAllData()
        //{
        //    List<object> menuItems = new List<object>();
        //    menuItems.Add(new
        //    {
        //        text = "Add New Item",
        //    });
        //    menuItems.Add(new
        //    {
        //        text = "Rename Item",
        //    });
        //    menuItems.Add(new
        //    {
        //        text = "Remove Item",
        //    });
        //    ViewData["menuItems"] = menuItems;
        //    return _structureService.GetLightStructuresForUsers(User);
        //}

        [HttpPost]
        public IActionResult GetUsers([FromBody]DataManagerRequest dm)
        {
            ICollection<DCAppUserViewModel> userList =
            new List<DCAppUserViewModel> {
                new DCAppUserViewModel(Guid.NewGuid().ToString()) { LastName = "John", FirstName="John", Email="test@test.co" },
                new DCAppUserViewModel(Guid.NewGuid().ToString()) { LastName = "Tom", FirstName="Johnson", Mobile="0123456789"},
                new DCAppUserViewModel(Guid.NewGuid().ToString()) { LastName = "Rose", FirstName="Lilly" },
                new DCAppUserViewModel(Guid.NewGuid().ToString()) { LastName = "Vick", FirstName="Turner" },
            };
            IEnumerable DataSource = userList;
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<DCAppUserViewModel>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);         //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }

        [HttpPost]
        public IActionResult LoadView([FromBody] NodeData model)
        {
            string action = "Index";
            PartialViewResult result = null;
            switch (model.NodeType)
            {
                // Internal
                case StringConstants.NodeType_Groups:
                    result = Groups(model.IsInternal);
                    break;
                case StringConstants.NodeType_Users:
                    result = Users(model.IsInternal);
                    break;
                case StringConstants.NodeType_Roles:
                    result = Roles(model.IsInternal);
                    break;
                case StringConstants.NodeType_Features:
                    result = Features(model.IsInternal);
                    break;
                case StringConstants.NodeType_DataModels:
                    result = DataModels(model.IsInternal, false);
                    break;
                case StringConstants.NodeType_CapabilityDataModels:
                    result = DataModels(model.IsInternal, true);
                    break;

                // External


                default:
                    break;
            }

            var user = _userManager.GetUserName(User);
            if (user == "johnmangam@gmail.com" || user == "emmyjstephen@gmail.com" || user.EndsWith("iemoutreach.org"))
            {
                if(result!=null)
                {
                    return result;
                }
                return RedirectToAction(action, model);
            }
          
                return View("AccessDenied");
        }

        public PartialViewResult StructureData(NodeData model)
        {
            var structure = new DCAppStructureViewModel(model.NodeId);
            structure.Name = "Indian Evangelical Mission";
            return PartialView("_Structure", structure);
        }

        public PartialViewResult Groups(bool isInternal)
        {
            GroupAssistanceViewModel groupViewModel = new GroupAssistanceViewModel();

            if (isInternal)
            {
                groupViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }
            else
            {
                groupViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            groupViewModel.IsInternal = isInternal;

            return PartialView("_Groups", groupViewModel);
        }

        public PartialViewResult Roles(bool isInternal)
        {
            GroupAssistanceViewModel roleViewModel = new GroupAssistanceViewModel();

            if (isInternal)
            {
                roleViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }
            else
            {
                roleViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            roleViewModel.IsInternal = isInternal;
            return PartialView("_Roles", roleViewModel);
        }

        public PartialViewResult Users(bool isInternal)
        {
            UserAssistanceViewModel userViewModel = new UserAssistanceViewModel();

            if (isInternal)
            {
                userViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }
            else
            {
                userViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            userViewModel.IsInternal = isInternal;
            return PartialView("_Users", userViewModel);
        }

        public PartialViewResult DataModels(bool isInternal, bool isCapability)
        {
            DataModelAssistanceViewModel dataViewModel = new DataModelAssistanceViewModel();

            if(isInternal)
            {
                dataViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }            
            else
            {
                dataViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            LoadViewBagWithGroups(isInternal);

            dataViewModel.IsInternal = isInternal;
            dataViewModel.IsCapability = isCapability;
           
            return PartialView("_DataModels", dataViewModel);
        }

        public PartialViewResult Features(bool isInternal)
        {
            FeatureAssistanceViewModel featureViewModel = new FeatureAssistanceViewModel();

            if (isInternal)
            {
                featureViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.InternalGroup).FirstOrDefault().InternalGroup.Id;
            }
            else
            {
                featureViewModel.GroupId = _dBContext.DAppStructures.Include(x => x.ExternalGroup).FirstOrDefault().ExternalGroup.Id;
            }

            LoadViewBagWithGroups(isInternal);

            featureViewModel.IsInternal = isInternal;
            return PartialView("_Features", featureViewModel);
        }        

        private void LoadViewBagWithGroups(bool isInternal)
        {
            var serviceMessage = _groupService.GetGroupList(isInternal);
            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                IEnumerable<DCAppGroupViewModel> DataSource = ((IEnumerable<DCAppGroupViewModel>)serviceMessage.Data).ToList();
                ViewBag.TreeDatasource = DataSource;
            }
            else // failed case
            {
                ViewBag.TreeDatasource = null;
            }
        }
    }
}