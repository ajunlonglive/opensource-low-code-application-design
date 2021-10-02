using Domain.Entities;
using Presentation.ViewModels;
using Presentation.Utility;

namespace Presentation.EntityBuilders
{
    public static class FeatureEntityBuilder
    {
        public static DCAppFeature BuildCreatedEntity(DCAppFeature entity, DCAppFeatureViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.GroupId = model.GroupId;
            return entity;
        }

        public static DCAppFeatureViewModel BuildViewModel(DCAppFeature entity)
        {
            var model = new DCAppFeatureViewModel(entity);
            model.GroupId = entity.GroupId.ToGuid();
            return model;
        }
    }
}