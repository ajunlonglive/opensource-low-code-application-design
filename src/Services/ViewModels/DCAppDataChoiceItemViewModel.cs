using Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class DCAppDataChoiceItemViewModel : BaseViewModel
    {
        public DCAppDataChoiceItemViewModel()
        {

        }
        public DCAppDataChoiceItemViewModel(Entity entity) :base(entity)
        {
        }

        public string DataFieldId { get; set; }
        public string ChoiceGroup { get; set; }
    }
}