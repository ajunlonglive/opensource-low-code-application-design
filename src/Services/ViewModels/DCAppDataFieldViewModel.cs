using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppDataFieldViewModel : BaseViewModel
    {
        public DCAppDataFieldViewModel(Entity entity):base(entity)
        {
        }

        public DCAppDataFieldViewModel() 
        {
        }

        public string DataType { get; set; }

        public string DataModelId { get; set; }

        public DCAppDataDefinitionBaseViewModel DataDefinition { get; set; }
    }
}