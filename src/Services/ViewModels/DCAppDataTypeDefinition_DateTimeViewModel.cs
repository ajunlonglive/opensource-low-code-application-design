using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_DateTimeViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_DateTimeViewModel()
        {

        }
        public DCAppDataTypeDefinition_DateTimeViewModel(Entity entity) : base(entity)
        {
        }
        public string Format { get; set; }
    }
}