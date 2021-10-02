using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppDataModelViewModel : BaseViewModel
    {
        public DCAppDataModelViewModel()
        {
            DataFields = new List<DCAppDataFieldViewModel>();
        }
        public DCAppDataModelViewModel(Entity entity):base(entity)
        {
            DataFields = new List<DCAppDataFieldViewModel>();
        }

        public ICollection<DCAppDataFieldViewModel> DataFields { get; set; }

        public int TotalItemsCount => DataFields.Count;

        public Guid GroupId { get; set; }
    }
}