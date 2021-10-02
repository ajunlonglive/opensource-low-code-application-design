using Domain.Entities;
using Presentation.ViewModels;
using Presentation.Utility;

namespace Presentation.EntityBuilders
{
    public static class DataModelEntityBuilder
    {
        public static DCAppDataModel BuildCreatedEntity(DCAppDataModel entity, DCAppDataModelViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.GroupId = model.GroupId;
            return entity;
        }

        public static DCAppDataModelViewModel BuildViewModel(DCAppDataModel entity)
        {
            var model = new DCAppDataModelViewModel(entity);
            model.GroupId = entity.GroupId.ToGuid();
            return model;
        }
    }
}