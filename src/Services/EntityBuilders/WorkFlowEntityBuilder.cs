using Domain.Entities;
using Presentation.ViewModels;
using Presentation.Utility;

namespace Presentation.EntityBuilders
{
    public static class WorkFlowEntityBuilder
    {
        public static DCAppWorkFlow BuildCreatedEntity(DCAppWorkFlow entity, DCAppWorkFlowViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DCAppFeatureId = model.FeatureId;
            return entity;
        }

        public static DCAppWorkFlowViewModel BuildViewModel(DCAppWorkFlow entity)
        {
            var model = new DCAppWorkFlowViewModel(entity);
            model.FeatureId = entity.DCAppFeatureId;
            return model;
        }
    }
}