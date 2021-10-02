using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class NodeData
    {
        public string NodeId { get; set; }
        public string NodeType { get; set; }
        public string StructureId { get; set; }

        public string HubId { get; set; }

        public bool IsAdd { get; set; }
        public bool IsDelete { get; set; }

        public bool IsInternal { get; set; }
    }

    public class NodeViewModel
    {
        public NodeViewModel()
        {
            HtmlAttributes = new Dictionary<string, string>();
        }

        public string NodeId;
        public string NodeText;
        public string Icon;
        public bool Expanded;
        public bool Selected;
        public string NodeType;

        public Dictionary<string, string> HtmlAttributes;
    }

    public class ParentNodeViewModel : NodeViewModel
    {
        public ParentNodeViewModel()
        {
            child = new List<ChilditemViewModel>();
        }

        public List<ChilditemViewModel> child;
    }

    public class ChilditemViewModel : NodeViewModel
    {
        public ChilditemViewModel()
        {
            child = new List<ChilditemViewModel>();
        }

        public List<ChilditemViewModel> child;
    }
}