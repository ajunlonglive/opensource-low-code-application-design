using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;

namespace Presentation.Services
{
    public interface IDataModelService
    {
        DCAppServiceMessage GetDataModel(Guid dataModelId);
        
        DCAppServiceMessage GetDataModelsList(Guid groupId);

        IEnumerable<SelectListItem> GetDataModelListAsSelectListItems(string modelToSkipId=null);

        IEnumerable<SelectListItem> GetDataFieldListAsSelectListItems(string dataModelId);

        DCAppServiceMessage CreateDataField(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataField(DCAppDataFieldViewModel model);

        DCAppServiceMessage RemoveDataField(DCAppDataFieldViewModel model);

        DCAppServiceMessage CreateDataModel(DCAppDataModelViewModel model);
        
        DCAppServiceMessage UpdateDataModel(DCAppDataModelViewModel model);

        DCAppServiceMessage RemoveDataModel(DCAppDataModelViewModel model);

        DCAppServiceMessage GetDataProperties(string dataFieldId);

        DCAppServiceMessage UpdateDataProperties_String(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_Number(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_Bool(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_DateTime(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_File(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_Choices(DCAppDataFieldViewModel model);

        DCAppServiceMessage UpdateDataProperties_EntityReference(DCAppDataFieldViewModel model);

        DCAppServiceMessage CreateDataFieldChoiceItem(DCAppDataChoiceItemViewModel model);

        DCAppServiceMessage UpdateDataFieldChoiceItem(DCAppDataChoiceItemViewModel model);

        DCAppServiceMessage RemoveDataFieldChoiceItem(DCAppDataChoiceItemViewModel model);

        DCAppServiceMessage GetDataFieldChoiceItems(DCAppDataFieldViewModel model);
    }
}