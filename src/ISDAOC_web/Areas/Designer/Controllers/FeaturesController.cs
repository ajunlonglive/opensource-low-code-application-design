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
using Presentation.Utility;

namespace ISDAOC_Web.Designer.Controllers
{
    [Area("Designer")]
    [Authorize]
    public class FeaturesController : Controller
    {
        private readonly IStructureService _structureService;
        private readonly IFeatureService _featureService;
        private readonly StructureDBContext _dBContext;
        private readonly IGeneralUtilityService _generalUtilityService;
        private readonly IWorkFlowService _workFlowService;
        public FeaturesController(StructureDBContext context,
            IFeatureService featureService, 
            IGeneralUtilityService generalUtilityService,
            IWorkFlowService workFlowService)
        {
            _dBContext = context;
            _featureService = featureService;
            _generalUtilityService = generalUtilityService;
            _workFlowService = workFlowService;
        }

        #region Features
        [HttpPost]
        public PartialViewResult GetFeatureList([FromBody] FeatureAssistanceViewModel model)
        {
            var serviceMessage = _featureService.GetFeatureList(model.GroupId);

            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                IEnumerable<DCAppFeatureViewModel> DataSource = ((IEnumerable<DCAppFeatureViewModel>)serviceMessage.Data).ToList();
                model.Features = DataSource.ToList();
                return PartialView("_FeaturesList", model);
            }

            return PartialView("_FeaturesList", model);

        }

        #endregion

        #region WorkFlow
        [HttpPost]
        public PartialViewResult GetWorkFlowData([FromBody] FeatureAssistanceViewModel model)
        {
            var serviceMessage = _workFlowService.GetWorkFlow(model.WorkFlowId, model.DataRowId);

            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                var result = serviceMessage.Data as DCAppWorkFlowViewModel;
                return PartialView("_WorkFlow", result);
            }

            return PartialView("_FeaturesList", model);

        }
      

        #endregion

        #region Page
        [HttpPost]
        public PartialViewResult AddNewPage([FromBody] FeatureAssistanceViewModel model)
        {
            var serviceMessage = _workFlowService.LoadGUIPageFromDatavalue(model.AddNewPageId, Guid.Empty);

            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                var result = serviceMessage.Data as DCAppPageViewModel;            
                return PartialView("_GUIPage", result);
            }

            return PartialView("_GUIPage", new DCAppPageViewModel());

        }


        #endregion

    }
}