using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Utilities;

namespace Presentation.ViewModels
{
    public class DCAppControlValidationViewModel : BaseViewModel
    {
        public DCAppControlValidationViewModel()
        {
        }
        public DCAppControlValidationViewModel(Entity entity) : base(entity)
        {
        }

        public List<SelectListItem> Options { get; set; }
        public string SelectedValue { get; set; }
        public string Parameters { get; set; }

    }
}