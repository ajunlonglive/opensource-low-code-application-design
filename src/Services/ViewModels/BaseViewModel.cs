using Domain.Abstractions;
using Syncfusion.EJ2.Base;
using Syncfusion.Pdf.Lists;

namespace Presentation.ViewModels
{
    public abstract class BaseViewModel
    {
        protected BaseViewModel()
        {

        }

        protected BaseViewModel(string guid)
        {
            Id = guid;
        }

        protected BaseViewModel(Entity entity)
        {
            Id = entity.Id.ToString();
            Name = string.IsNullOrEmpty(entity.Name)?string.Empty : entity.Name ;
            Description = string.IsNullOrEmpty(entity.Description)? string.Empty : entity.Description;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}