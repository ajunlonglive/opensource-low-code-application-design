using Domain.Abstractions;
using Domain.Entities;

namespace Presentation.ViewModels
{
    public class DCAppDataDefinitionBaseViewModel : BaseViewModel
    {
        public DCAppDataDefinitionBaseViewModel()
        {

        }
        public DCAppDataDefinitionBaseViewModel(Entity entity) : base(entity)
        {
            Id = entity.Id.ToString();
            Name = entity.Name;
            Description = entity.Description;

            if (entity is DCAppDataDefinitionBase)
            {
                AllowNullValue = ((DCAppDataDefinitionBase)entity).AllowNullValue;
                AllowOnlyUniqueValue = ((DCAppDataDefinitionBase)entity).AllowOnlyUniqueValue;
            }
        }

        public bool AllowNullValue { get; set; }
        public bool AllowOnlyUniqueValue { get; set; }
        public string DataFieldId { get; set; }
        public string DataModelId { get; set; }
    }
}