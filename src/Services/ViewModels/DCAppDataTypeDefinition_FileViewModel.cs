using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_FileViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_FileViewModel()
        {

        }
        public DCAppDataTypeDefinition_FileViewModel(Entity entity) : base(entity)
        {
        }
        public string Location { get; set; }
    }
}