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
    public class WorkFlowsController : Controller
    {
        private readonly IStructureService _structureService;
        private readonly IDataModelService _dataModelService;
        private readonly StructureDBContext _dBContext;
        private IGeneralUtilityService _generalUtilityService;

        public WorkFlowsController(StructureDBContext context,
            IDataModelService dataModelService, IGeneralUtilityService generalUtilityService)
        {
            _dBContext = context;
            _dataModelService = dataModelService;
            _generalUtilityService = generalUtilityService;
        }


        #region DataModels

        public PartialViewResult GetDataModelData([FromBody] NodeData model)
        {
            //var dataViewModel = new DataModelAssistanceViewModel
            //{
            //    DataModelId = Guid.Parse("41117abd-801c-4ddf-8395-431e0f358c89"/*model.NodeId*/),
            //    StructureId = Guid.Parse(model.StructureId),
            //    HubId = Guid.Parse(model.HubId)
            //};

            DCAppDataModel dataModel = _dBContext.DAppDataModels.Include(y => y.Group).FirstOrDefault(x => x.Id.ToString() == model.NodeId);
            DataModelAssistanceViewModel dataViewModel = new DataModelAssistanceViewModel
            {
                DataModelId = dataModel.Id,
                Name = dataModel.Name,
                Description = dataModel.Description,
                GroupId = dataModel.GroupId.ToGuid(),
            };

            return PartialView("_DataModelData", dataViewModel);
        }

        private bool DCAppDataModelExists(Guid id)
        {
            return _dBContext.DAppDataModels.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult GetDataModelList([FromBody]DataModelAssistanceViewModel model)
        {
            model.Principal = User;
            var serviceMessage = _dataModelService.GetDataModelsList(model.GroupId);
            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                IEnumerable<DCAppDataModelViewModel> DataSource = ((IEnumerable<DCAppDataModelViewModel>)serviceMessage.Data).ToList();
                if (DataSource.ToList().Count > 0)
                {
                    DataOperations operation = new DataOperations();
                    if (model.Sorted != null && model.Sorted.Count > 0) //Sorting
                    {
                        DataSource = operation.PerformSorting(DataSource, model.Sorted);
                    }
                    if (model.Where != null && model.Where.Count > 0) //Filtering
                    {
                        DataSource = operation.PerformFiltering(DataSource, model.Where, model.Where[0].Operator);
                    }
                }
                return Json(new { result = DataSource, count = DataSource.ToList().Count });
            }
            else // failed case
            {
                return Json(new { result = "", count = 0 });
            }
        }

        [HttpPost]
        public IActionResult UpdateDataModel([FromBody]CRUDModel<DCAppDataModelViewModel> value)
        {
            var data = value.Value;
            
            var message = _dataModelService.UpdateDataModel(data);

            if (message.Result == DCAppServiceResult.suceeded)
            {
                value.Value = (DCAppDataModelViewModel)message.Data;

            }

            return Json(value.Value);
        }

        [HttpPost]
        public IActionResult DialogUpdateDataModel([FromBody] CRUDModel<DCAppDataModelViewModel> value)
        {
            return PartialView("_DialogUpdateDataModel", value.Value);
        }

        [HttpPost]
        public IActionResult DialogCreateDataModel([FromBody] CRUDModel<DCAppDataModelViewModel> value)
        {
            return PartialView("_DialogCreateDataModel");
        }

        [HttpPost]
        public IActionResult ProcessWorkFlowData([FromForm]DCAppWorkFlowViewModel value)
        {
            var data = value;
            
            //var message = _dataModelService.CreateDataModel(data);

           // if (message.Result == DCAppServiceResult.suceeded)
            {
               // value.Value = (DCAppDataModelViewModel)message.Data;

            }

            return Json(value);
        }

        [HttpPost]
        public IActionResult RemoveDataModel([FromBody]CRUDModel<DCAppDataModelViewModel> value)
        {
            var data = new DCAppDataModelViewModel { Id = value.Key.ToString() };
            var message = _dataModelService.RemoveDataModel(data);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                value.Value = (DCAppDataModelViewModel)message.Data;
            }
            return Json(value.Value);
        }

        #endregion

        #region DataFields

        [HttpPost]
        public IActionResult GetDataFields([FromBody]DataModelAssistanceViewModel model)
        {
            model.Principal = User;
            var serviceMessage = _dataModelService.GetDataModel(model.DataModelId);
            if (serviceMessage.Result == DCAppServiceResult.suceeded)
            {
                var data = ((DCAppDataModelViewModel)serviceMessage.Data);
                IEnumerable DataSource = data.DataFields;
                if (((DCAppDataModelViewModel)serviceMessage.Data).DataFields.Count > 0)
                {
                    DataOperations operation = new DataOperations();
                    if (model.Sorted != null && model.Sorted.Count > 0) //Sorting
                    {
                        DataSource = operation.PerformSorting(DataSource, model.Sorted);
                    }
                    if (model.Where != null && model.Where.Count > 0) //Filtering
                    {
                        DataSource = operation.PerformFiltering(DataSource, model.Where, model.Where[0].Operator);
                    }
                }
                return Json(new { result = DataSource, count = data.TotalItemsCount });
            }
            else // failed case
            {
                return Json(new { result = "", count = 0 });
            }
        }


        public PartialViewResult GetDataFieldData([FromBody] NodeData model)
        {
            //var dataViewModel = new DataModelAssistanceViewModel
            //{
            //    DataModelId = Guid.Parse("41117abd-801c-4ddf-8395-431e0f358c89"/*model.NodeId*/),
            //    StructureId = Guid.Parse(model.StructureId),
            //    HubId = Guid.Parse(model.HubId)
            //};

            DCAppDataField dataField = _dBContext.DAppDataFields.Include(y => y.DCAppDataModel).FirstOrDefault(x => x.Id.ToString() == model.NodeId);
            DCAppDataFieldViewModel dataFieldViewModel = DataFieldEntityBuilder.BuildViewModel(dataField);

            return PartialView("_DataFieldData", dataFieldViewModel);
        }

        [HttpPost]
        public IActionResult DialogUpdateDataField([FromBody] CRUDModel<DCAppDataFieldViewModel> value)
        {
            return PartialView("_DialogUpdateDataField", value.Value);
        }

        [HttpPost]
        public IActionResult DialogCreateDataField([FromBody] CRUDModel<DCAppDataFieldViewModel> value)
        {
            return PartialView("_DialogCreateDataField");
        }

        [HttpPost]
        public IActionResult CreateDataField([FromBody]CRUDModel<DCAppDataFieldViewModel> value)
        {
            string dataModelId = value.Params["DataModelId"].ToString();
            var data = value.Value;
            
            data.DataModelId = dataModelId;
            var message = _dataModelService.CreateDataField(data);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var newData = (DCAppDataFieldViewModel)message.Data;
                data.Id = newData.Id;
            }

            return Json(value.Value);
        }

        [HttpPost]
        public IActionResult UpdateDataField([FromBody]CRUDModel<DCAppDataFieldViewModel> value)
        {
            string dataModelId = value.Params["DataModelId"].ToString();
            var data = value.Value;
            
            data.DataModelId = dataModelId;
            var message = _dataModelService.UpdateDataField(data);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var newData = (DCAppDataFieldViewModel)message.Data;
                data.Id = newData.Id;
            }

            return Json(value.Value);
        }

        [HttpPost]
        public void RemoveDataField([FromBody]CRUDModel<DCAppDataFieldViewModel> value)
        {
            string dataModelId = value.Params["DataModelId"].ToString();
            var data = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(value.Key.ToString())));
            data.DataModelId = dataModelId;
            var message = _dataModelService.RemoveDataField(data);
            if (message.Result == DCAppServiceResult.suceeded)
            {
            }
        }

        #endregion

        #region DataFieldsConfiguration
               
        [HttpPost]
        public ActionResult GetDataPropertiesData(string dataFieldId)
        {
            var message = _dataModelService.GetDataProperties(dataFieldId);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                if (dataField.DataDefinition != null)
                {
                    dataField.DataDefinition.DataFieldId = dataField.Id;
                    dataField.DataDefinition.DataModelId = dataField.DataModelId;

                    if (dataField.DataDefinition is DCAppDataTypeDefinition_StringViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_String", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_EntityReferenceViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_EntityReference", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_BoolViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_Bool", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_NumberViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_Number", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_DateTimeViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_DateTime", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_ChoiceViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_Choices", dataField.DataDefinition);
                    }
                    else if (dataField.DataDefinition is DCAppDataTypeDefinition_FileViewModel)
                    {
                        return PartialView("_UpdateDataTypeProperities_File", dataField.DataDefinition);
                    }
                }
                else
                {
                    return PartialView("_UpdateDataTypeProperities_Unknown");
                }
            }

            return PartialView("_LoadInternalError", message.Message);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_String([FromBody]DCAppDataTypeDefinition_StringViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_String(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_String", (DCAppDataTypeDefinition_StringViewModel)dataField.DataDefinition);
            }

            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_String", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_Number([FromBody]DCAppDataTypeDefinition_NumberViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_Number(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_Number", (DCAppDataTypeDefinition_NumberViewModel)dataField.DataDefinition);
            }

            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_Number", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_DateTime([FromBody]DCAppDataTypeDefinition_DateTimeViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_DateTime(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_DateTime", (DCAppDataTypeDefinition_DateTimeViewModel)dataField.DataDefinition);
            }
            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_DateTime", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_Choices([FromBody]DCAppDataTypeDefinition_ChoiceViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_Choices(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_Choices",
                    (DCAppDataTypeDefinition_ChoiceViewModel)dataField.DataDefinition);
            }
            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_Choices", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_Bool([FromBody]DCAppDataTypeDefinition_BoolViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_Bool(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_Bool",
                    (DCAppDataTypeDefinition_BoolViewModel)dataField.DataDefinition);
            }
            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_Bool", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_File([FromBody]DCAppDataTypeDefinition_FileViewModel model)
        {
            var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
            dataFieldViewModel.DataDefinition = model;
            var message = _dataModelService.UpdateDataProperties_File(dataFieldViewModel);
            if (message.Result == DCAppServiceResult.suceeded)
            {
                var dataField = (DCAppDataFieldViewModel)message.Data;
                dataField.DataDefinition.DisplayMessage = message.Message;
                return PartialView("_UpdateDataTypeProperities_File",
                    (DCAppDataTypeDefinition_FileViewModel)dataField.DataDefinition);
            }
            model.ErrorMessage = message.Message;
            return PartialView("_UpdateDataTypeProperities_File", model);
        }

        [HttpPost]
        public ActionResult UpdatePropertiesData_EntityReference([FromBody]DCAppDataTypeDefinition_EntityReferenceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dataFieldViewModel = new DCAppDataFieldViewModel(new DCAppDataField(Guid.Parse(model.DataFieldId)));
                dataFieldViewModel.DataDefinition = model;
                var message = _dataModelService.UpdateDataProperties_EntityReference(dataFieldViewModel);
                if (message.Result == DCAppServiceResult.suceeded)
                {
                    var dataField = (DCAppDataFieldViewModel)message.Data;
                    dataField.DataDefinition.DisplayMessage = message.Message;
                    return PartialView("_UpdateDataTypeProperities_EntityReference",
                        (DCAppDataTypeDefinition_EntityReferenceViewModel)dataField.DataDefinition);
                }
                model.ErrorMessage = message.Message;
            }
            else
            {
                model.ErrorMessage = string.Join(Environment.NewLine, ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return PartialView("_UpdateDataTypeProperities_EntityReference", model);
        }
        #endregion
    }
}