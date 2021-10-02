using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_StringViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_StringViewModel()
        {

        }
        public DCAppDataTypeDefinition_StringViewModel(Entity entity) : base(entity)
        {
        }

        public string Length { get; set; }
        public bool IsMultiLine { get; set; }

        public string Format { get; set; }
        public bool IsRegularExpression { get; set; }
    }
}