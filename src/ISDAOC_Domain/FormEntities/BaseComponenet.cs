using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.FormEntities
{
    public class Components
    {
        [JsonProperty("components")]
        public List<BaseComponent> ComponentList { get; set; }

        public Components()
        {
            ComponentList = new List<BaseComponent>();
        }
    }

    public class FormData
    {
        [JsonProperty("data")]
        public Dictionary<string, string> DataValueList { get; set; }

        public FormData()
        {
            DataValueList = new Dictionary<string, string>();
        }
    }

    public class DataGridData
    {
        [JsonProperty("data")]
        public DataGridDataChildren DataValueList { get; set; }

        public DataGridData()
        {
            DataValueList = new DataGridDataChildren();
        }
    }

    public class DataGridDataChildren
    {
        [JsonProperty("children")]
        public List<Dictionary<string, string>> DataValueList { get; set; }

        public DataGridDataChildren()
        {
            DataValueList = new List<Dictionary<string, string>>();
        }
    }

    public class Component
    {
        public string type { get; set; }
        public bool alwaysEnabled { get; set; }
        //public bool  { get; set; }
        //public bool  { get; set; }
    }

    public class BaseComponent : Component
    {
        public bool input { get; set; }
        public bool unique { get; set; }
        public bool persistent { get; set; }
        public bool hidden { get; set; }
        public bool clearOnHide { get; set; }
        public bool tableView { get; set; }
        public bool dataGridLabel { get; set; }
        public bool hideLabel { get; set; }

        public bool disabled { get; set; }
        public bool autofocus { get; set; }
        public bool dbIndex { get; set; }

        public bool allowCalculateOverride { get; set; }
        public bool clearOnRefresh { get; set; }

        public string defaultValue { get; set; }
        public string key { get; set; }
        public string placeholder { get; set; }
        public string prefix { get; set; }
        public string customClass { get; set; }
        public string suffix { get; set; }
        public string labelPosition { get; set; }
        public string description { get; set; }
        public string label { get; set; }
        public string errorLabel { get; set; }
        public string tooltip { get; set; }
        public string tabindex { get; set; }
        public string customDefaultValue { get; set; }
        public string calculateValue { get; set; }
        public string refreshOn { get; set; }
        public string validateOn { get; set; }
        public Validate validate { get; set; }
        public Conditional conditional { get; set; }

        public int labelWidth { get; set; }
        public int labelMargin { get; set; }
    }

    public class Validate
    {
        public bool required { get; set; }
        public int minLength { get; set; }
        public string customMessage { get; set; }
        public string json { get; set; }
    }

    public class Conditional
    {
        public string show { get; set; }
        public string when { get; set; }
        public string json { get; set; }
    }

    public class DataGridComponentWithData
    {
        public DataGridComponent components { get; set; }
        public DataGridData data { get; set; }
    }

    public class DataGridComponent
    {
        public DataGridComponent()
        {
            DataGridContainer = new List<DataGridContainer>();
        }

        [JsonProperty("components")]
        public List<DataGridContainer> DataGridContainer { get; set; }
    }

    public class DataGridContainer //: BaseComponent
    {
        //extracomponents
        public DataGridContainer()
        {
            type = "datagrid";
            label = "Children";
            key = "children";
            input = true;
            reorder = true;
            search = true;
            Validate = new newValidate { minLength = 1, maxLength = 4 };
            ComponentList = new List<BaseComponent>();
        }

        public bool reorder { get; set; }
        public newValidate Validate { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string key { get; set; }
        public bool input { get; set; }
        public bool search { get; set; }

        public class newValidate : Validate
        {
            public int maxLength { get; set; }
        }

        [JsonProperty("components")]
        public List<BaseComponent> ComponentList { get; set; }
    }
}