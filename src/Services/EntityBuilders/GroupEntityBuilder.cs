using Domain.Entities;
using Presentation.ViewModels;

namespace Presentation.EntityBuilders
{
    public static class GroupEntityBuilder
    {
        public static DCAppGroup BuildCreatedEntity(DCAppGroup entity, DCAppGroupViewModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
             return entity;
        }

        public static DCAppGroupViewModel BuildViewModel(DCAppGroup entity)
        {
            var model = new DCAppGroupViewModel(entity);

            return model;
        }
    }
}