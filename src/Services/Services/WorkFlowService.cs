using Domain.Entities.Data;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Services
{
    public class WorkFlowService : IWorkFlowService
    {
        private readonly StructureDBContext _structureDBContext;
        private readonly IDataModelService _dataModelService;
        private readonly IGeneralUtilityService _generalUtilityService;
        private readonly IGroupService _groupService;
        public WorkFlowService(StructureDBContext structureDBContext,
            IDataModelService dataModelService,
            IGeneralUtilityService generalUtilityService,
            IGroupService groupService)
        {
            _structureDBContext = structureDBContext;
            _dataModelService = dataModelService;
            _generalUtilityService = generalUtilityService;
            _groupService = groupService;
        }

        public void AddWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel)
        {
            throw new NotImplementedException();
        }

        public DCAppServiceMessage GetWorkFlow(Guid WorkFlowRowId, Guid DataRowId)
        {
            try
            {
                if (_structureDBContext.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable)) != null)
                {
                    // var workFlowsTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields).FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable));

                    var workFlowDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField).ThenInclude(dm => dm.DCAppDataModel).Where(x => x.RowId == WorkFlowRowId).ToList();

                    if (workFlowDataValues.Count > 0)
                    {
                        var dCAppWorkFlowViewModel = new DCAppWorkFlowViewModel();

                        foreach (var workFlowRowDataValue in workFlowDataValues)
                        {
                            switch (workFlowRowDataValue.DataField.Name)
                            {
                                case EFStringConstants.NameField:
                                    dCAppWorkFlowViewModel.Name = workFlowRowDataValue.Value;
                                    dCAppWorkFlowViewModel.Id = WorkFlowRowId.ToString();
                                    break;
                                case EFStringConstants.DescriptionField:
                                    dCAppWorkFlowViewModel.Description = workFlowRowDataValue.Value;
                                    break;
                                case EFStringConstants.GUIFeature_Field:
                                    dCAppWorkFlowViewModel.BreadCrumb = "**Todo** breadCurmb";
                                    break;
                                case EFStringConstants.Data_RoleAccessGroup_Field:
                                    dCAppWorkFlowViewModel.RAG = new DCAppRoleAccessGroupViewModel() { Id = workFlowRowDataValue.SingleReferenceRowId.ToString() };
                                    break;
                                case EFStringConstants.GUIPages_Field:
                                    foreach (var rowId in workFlowRowDataValue.MultipleReferenceRowIds)
                                    {
                                        var message = LoadGUIPageFromDatavalue(rowId, DataRowId);
                                        if (message.Result == DCAppServiceResult.suceeded)
                                            dCAppWorkFlowViewModel.Pages.Add(message.Data as DCAppPageViewModel);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dCAppWorkFlowViewModel);
                    }

                }
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new DCAppWorkFlowViewModel());
            }

            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new DCAppWorkFlowViewModel());
        }

        public DCAppServiceMessage LoadGUIPageFromDatavalue(Guid pageRowId, Guid dataRowId)
        {

            try
            {
                //if (_structureDBContext.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.PagesTable)) != null)
                {
                    // var workFlowsTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields).FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable));

                    var pageDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField).Where(x => x.RowId == pageRowId).ToList();

                    var dCAppPageViewModel = new DCAppPageViewModel();
                    foreach (var pageRowDataValue in pageDataValues)
                    {
                        switch (pageRowDataValue.DataField.Name)
                        {
                            case EFStringConstants.NameField:
                                dCAppPageViewModel.Name = pageRowDataValue.Value;
                                dCAppPageViewModel.Id = pageRowId.ToString();
                                break;
                            case EFStringConstants.DescriptionField:
                                dCAppPageViewModel.Description = pageRowDataValue.Value;
                                break;

                            case EFStringConstants.GUIControl_Base_Data_Table:
                                {
                                    dCAppPageViewModel.BaseDataTableOptions = _dataModelService.GetDataModelListAsSelectListItems().ToList();
                                    dCAppPageViewModel.SelectedBaseDataTable = pageRowDataValue.SingleReferenceRowId.ToString();
                                }
                                break;
                            case EFStringConstants.GUI_Controls_Field:
                                foreach (var rowId in pageRowDataValue.MultipleReferenceRowIds)
                                {
                                    var baseTable = _structureDBContext.DAppDataValues.Include(d => d.DataField).Include(b => b.BaseDataModel).Where(x => x.RowId == pageRowId && x.DataField.Name == EFStringConstants.GUIControl_Base_Data_Table).FirstOrDefault();
                                    var message = LoadGUIControlFromDatavalue(rowId, baseTable.BaseDataModel.Id.ToString(), dataRowId);
                                    if (message.Result == DCAppServiceResult.suceeded)
                                        dCAppPageViewModel.Controls.Add(message.Data as DCAppControlViewModel);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dCAppPageViewModel);

                }
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new DCAppPageViewModel());
            }
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new DCAppPageViewModel());
        }

        private DCAppServiceMessage LoadGUIControlFromDatavalue(Guid controlRowId, string baseDataModelId, Guid dataRowId)
        {
            try
            {
                if (_structureDBContext.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlsTable)) != null)
                {
                    // var workFlowsTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields).FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable));

                    var controlDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField).ThenInclude(df=>df.DataDefinition).Include(d => d.BaseDataField).ThenInclude(df => df.DataDefinition).Where(x => x.RowId == controlRowId).ToList();

                    var dCAppControlViewModel = new DCAppControlViewModel();
                    foreach (var controlRowDataValue in controlDataValues)
                    {
                        switch (controlRowDataValue.DataField.Name)
                        {
                            case EFStringConstants.NameField:
                                dCAppControlViewModel.Name = _generalUtilityService.ReplaceSpaceWithUnderscore(controlRowDataValue.Value);
                                dCAppControlViewModel.Id = controlRowId.ToString();
                                break;
                            case EFStringConstants.DescriptionField:
                                dCAppControlViewModel.Description = controlRowDataValue.Value;
                                break;
                            case EFStringConstants.LabelTextField:
                                dCAppControlViewModel.Label_Text = controlRowDataValue.Value;
                                break;
                            case EFStringConstants.GUI_Position_Index_Field:
                                dCAppControlViewModel.GUI_Position_Index = controlRowDataValue.Value == null ? (short)0 : short.Parse(controlRowDataValue.Value);
                                break;
                            case EFStringConstants.GUI_IsRequired:
                                dCAppControlViewModel.GUI_IsRequired = controlRowDataValue.Value == null ? false : bool.Parse(controlRowDataValue.Value);
                                break;
                            case EFStringConstants.GUIControlType:
                                {
                                    var choiceDataField = _structureDBContext.DAppDataModels.Include(y => y.DataFields)
                                               .ThenInclude(x => x.DataDefinition).ThenInclude(x => (x as DCAppChoiceDataDefinition).Choices)
                                               .FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlsTable))
                                               .DataFields.Where(df => df.Name == EFStringConstants.GUIControlType).FirstOrDefault();

                                    var choiceItems = (choiceDataField.DataDefinition as DCAppChoiceDataDefinition).Choices.Select(x => new SelectListItem(x.ChoiceGroup + " => " + x.Name, x.Id.ToString()));
                                    dCAppControlViewModel.GUIControlTypes = choiceItems.ToList();
                                    dCAppControlViewModel.Selected_GUI_Control_Type = controlRowDataValue.Value;
                                }
                                break;
                            case EFStringConstants.GUIControlActions_Field:
                                {
                                    var choiceDataField = _structureDBContext.DAppDataModels.Include(y => y.DataFields)
                                      .ThenInclude(x => x.DataDefinition).ThenInclude(x => (x as DCAppChoiceDataDefinition).Choices)
                                      .FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlActionsTable))
                                      .DataFields.Where(df => df.Name == EFStringConstants.GUI_ActionType_Field);

                                    var choiceItems = (choiceDataField as DCAppChoiceDataDefinition).Choices.Select(x => new SelectListItem(x.ChoiceGroup + " => " + x.Name, x.Id.ToString()));
                                    dCAppControlViewModel.GUIControlActionTypes = choiceItems.ToList();

                                    foreach (var rowId in controlRowDataValue.MultipleReferenceRowIds)
                                    {
                                        var message = LoadGUIControlActionFromDatavalue(rowId);
                                        if (message.Result == DCAppServiceResult.suceeded)
                                            dCAppControlViewModel.ControlActions.Add(message.Data as DCAppControlActionViewModel);
                                    }
                                }
                                break;
                            case EFStringConstants.GUIControl_Base_Data_Field:
                                {
                                    // TODO: Fill the control value with the DataRow's DV value for UPDATE Operation

                                    // Groups
                                    if (controlRowDataValue.BaseDataField != null && controlRowDataValue.BaseDataField.DataType == DataDefinitionDataType.EntityReference.Value)
                                    {
                                        var nameDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField)
                                            .Where(x => x.DataField.DCAppDataModelId == ((DCAppRefEntityDataDefinition)controlRowDataValue.BaseDataField.DataDefinition).RefDataModelId && x.DataField.Name == EFStringConstants.NameField).ToList()
                                          .Select(c => new SelectListItem { Text = c.Value, Value = c.RowId.ToString() })
                                          .ToList();
                                        
                                        dCAppControlViewModel.ControlChoiceList = nameDataValues.ToList();
                                    }
                                    else if (false)
                                    {
                                        //else if if (controlRowDataValue.BaseDataField.Name == EFStringConstants.Reference_Group_Field)
                                        var dataFields = _dataModelService.GetDataFieldListAsSelectListItems(baseDataModelId);
                                        dCAppControlViewModel.ControlChoiceList = dataFields.ToList();

                                    }

                                    dCAppControlViewModel.SelectedBaseDataField = controlRowDataValue.SingleReferenceRowId.ToString();
                                }
                                break;
                            case EFStringConstants.GUIControlValidations_Field:
                                {
                                    var choiceDataField = _structureDBContext.DAppDataModels.Include(y => y.DataFields)
                                      .ThenInclude(x => x.DataDefinition).ThenInclude(x => (x as DCAppChoiceDataDefinition).Choices)
                                      .FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlValidationsTable))
                                      .DataFields.Where(df => df.Name == EFStringConstants.GUI_ValidationType_Field);

                                    var choiceItems = (choiceDataField as DCAppChoiceDataDefinition).Choices.Select(x => new SelectListItem(x.ChoiceGroup + " => " + x.Name, x.Id.ToString()));
                                    dCAppControlViewModel.GUIControlValidationTypes = choiceItems.ToList();

                                    foreach (var rowId in controlRowDataValue.MultipleReferenceRowIds)
                                    {
                                        var message = LoadGUIControlValidationFromDatavalue(rowId);
                                        if (message.Result == DCAppServiceResult.suceeded)
                                            dCAppControlViewModel.ControlValidations.Add(message.Data as DCAppControlValidationViewModel);
                                    }
                                }
                                break;
                            case EFStringConstants.GUI_Add_Page_Field:
                                {
                                    var message = LoadGUIPageFromDatavalue(controlRowDataValue.SingleReferenceRowId, dataRowId);
                                    if (message.Result == DCAppServiceResult.suceeded)
                                        dCAppControlViewModel.AddPage = message.Data as DCAppPageViewModel;
                                    break;
                                }
                            default:
                                break;
                        }
                    }

                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dCAppControlViewModel);

                }
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new DCAppPageViewModel());
            }
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new DCAppPageViewModel());
        }


        private DCAppServiceMessage LoadGUIControlActionFromDatavalue(Guid controlActionRowId)
        {
            try
            {
                if (_structureDBContext.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlActionsTable)) != null)
                {
                    List<DCAppControlActionViewModel> dCAppControlActionViewModelList = new List<DCAppControlActionViewModel>();
                    // var workFlowsTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields).FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable));
                    var controlActionDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField).Where(x => x.RowId == controlActionRowId).ToList();

                    var dCAppControlActionViewModel = new DCAppControlActionViewModel();
                    foreach (var controlActionRowDataValue in controlActionDataValues)
                    {
                        switch (controlActionRowDataValue.DataField.Name)
                        {
                            case EFStringConstants.NameField:
                                dCAppControlActionViewModel.Name = controlActionRowDataValue.Value;
                                break;
                            case EFStringConstants.DescriptionField:
                                dCAppControlActionViewModel.Description = controlActionRowDataValue.Value;
                                break;

                            case EFStringConstants.GUI_ActionType_Field:
                                {
                                    dCAppControlActionViewModel.SelectedValue = controlActionRowDataValue.SingleReferenceRowId.ToString();
                                }
                                break;
                            case EFStringConstants.GUI_Parameters_Field:
                                dCAppControlActionViewModel.Parameters = controlActionRowDataValue.Value;
                                break;
                            default:
                                break;
                        }
                    }


                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dCAppControlActionViewModel);

                }
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new DCAppControlActionViewModel());
            }
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new DCAppControlActionViewModel());
        }

        private DCAppServiceMessage LoadGUIControlValidationFromDatavalue(Guid controlValidationRowId)
        {
            try
            {
                if (_structureDBContext.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.GUIControlValidationsTable)) != null)
                {
                    List<DCAppControlValidationViewModel> dCAppControlValidationViewModelList = new List<DCAppControlValidationViewModel>();
                    // var workFlowsTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields).FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.WorkFlowsTable));
                    var controlValidationDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField).Where(x => x.RowId == controlValidationRowId).ToList();

                    var dCAppControlValidationViewModel = new DCAppControlValidationViewModel();
                    foreach (var controlValidationRowDataValue in controlValidationDataValues)
                    {
                        switch (controlValidationRowDataValue.DataField.Name)
                        {
                            case EFStringConstants.NameField:
                                dCAppControlValidationViewModel.Name = controlValidationRowDataValue.Value;
                                break;
                            case EFStringConstants.DescriptionField:
                                dCAppControlValidationViewModel.Description = controlValidationRowDataValue.Value;
                                break;

                            case EFStringConstants.GUI_ValidationType_Field:
                                {
                                    dCAppControlValidationViewModel.SelectedValue = controlValidationRowDataValue.SingleReferenceRowId.ToString();
                                }
                                break;
                            case EFStringConstants.GUI_Parameters_Field:
                                dCAppControlValidationViewModel.Parameters = controlValidationRowDataValue.Value;
                                break;
                            default:
                                break;
                        }
                    }


                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, dCAppControlValidationViewModel);

                }
            }
            catch (Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new DCAppControlValidationViewModel());
            }
            return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new DCAppControlValidationViewModel());
        }

        public void RemoveWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel)
        {
            throw new NotImplementedException();
        }
    }
}