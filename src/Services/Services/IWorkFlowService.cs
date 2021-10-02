using Presentation.ViewModels;
using System;

namespace Presentation.Services
{
    public interface IWorkFlowService
    {
        void AddWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel);

        void RemoveWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel);

        void UpdateWorkFlowToFeature(DCAppWorkFlowViewModel WorkFlowModel);

        DCAppServiceMessage GetWorkFlow(Guid WorkFlowId, Guid DataRowId);

        DCAppServiceMessage LoadGUIPageFromDatavalue(Guid pageId, Guid DataRowId);
    }
}