using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.ViewModels
{
    public class DCAppPageViewModel : BaseViewModel
    {

        public DCAppPageViewModel()
        {
            Controls = new List<DCAppControlViewModel>();
        }
        public DCAppPageViewModel(Entity entity) : base(entity)
        {
            Controls = new List<DCAppControlViewModel>();
        }

        public ICollection<DCAppControlViewModel> Controls { get; set; }

        public int TotalItemsCount => Controls.Count;

        public List<SelectListItem> BaseDataTableOptions { get; set; }

        public string SelectedBaseDataTable { get; set; }

        public Guid GroupId { get; set; }

        public Guid DataRowId { get; set; }

        public string FormRole { get; set; }
    }
}