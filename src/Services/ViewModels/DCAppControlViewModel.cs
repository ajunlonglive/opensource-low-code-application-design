using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Mozilla;

namespace Presentation.ViewModels
{
    public class DCAppControlViewModel : BaseViewModel
    {
        public DCAppControlViewModel()
        {
            ControlActions = new List<DCAppControlActionViewModel>();
            ControlValidations = new List<DCAppControlValidationViewModel>();
            Pages = new List<DCAppPageViewModel>();
        }
        public DCAppControlViewModel(Entity entity) : base(entity)
        {
            ControlActions = new List<DCAppControlActionViewModel>();
            ControlValidations = new List<DCAppControlValidationViewModel>();
            Pages = new List<DCAppPageViewModel>();
        }

        public ICollection<DCAppControlActionViewModel> ControlActions { get; set; }

        public ICollection<DCAppControlValidationViewModel> ControlValidations { get; set; }

        public List<SelectListItem> GUIControlTypes { get; set; }
        public List<SelectListItem> GUIControlActionTypes { get; set; }

        public List<SelectListItem> GUIControlValidationTypes { get; set; }
        public string Selected_GUI_Control_Type { get; set; }
        public string Label_Text { get; set; }

        public short GUI_Position_Index { get; set; }

        public string Base_Data_Field { get; set; }

        public bool GUI_IsRequired { get; set; }

        public string SelectedBaseDataField { get; set; }
        public string ControlValue { get; set; }

        public List<SelectListItem> ControlChoiceList { get; set; }

        public DCAppPageViewModel AddPage { get; set; }

        public ICollection<DCAppPageViewModel> Pages { get; set; }

        public Guid DataRowId { get; set; }
    }
}