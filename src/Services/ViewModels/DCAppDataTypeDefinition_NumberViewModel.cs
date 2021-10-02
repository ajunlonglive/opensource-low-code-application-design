using Domain.Abstractions;

namespace Presentation.ViewModels
{
    public class DCAppDataTypeDefinition_NumberViewModel : DCAppDataDefinitionBaseViewModel
    {
        public DCAppDataTypeDefinition_NumberViewModel()
        {

        }
        public DCAppDataTypeDefinition_NumberViewModel(Entity entity) : base(entity)
        {
        }
        public bool HasDecimals { get; set; }
        public string DecimalPlaces { get; set; }
    }
}