using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public static class EFStringConstants
    {
        // Common

        public const string NameField = "Name";

        public const string DescriptionField = "Description";

        // Groups
        public const string InternalGroup = "Internal";

        public const string ExternalGroup = "External";

        public const string SystemPlatformGroup = "sys_Group_Platform";

        public const string  GroupsTable = "Platform-Groups";

        public const string Reference_Group_Field = "Reference Group";        

        // Features
        public const string FeaturesTable = "GUI-Features";

        public const string WorkFlowsTable = "GUI-WorkFlows";

        public const string WorkFlows_Field = "GUI-WorkFlows";

        public const string PagesTable = "GUI-Pages";

        public const string GUIControlsTable = "GUI-Controls";

        public const string GUIControlActionsTable = "GUI-ControlActions";

        public const string GUIControlValidationsTable = "GUI-ControlValidations";

        public const string IsExternalField = "IsExternal";

        // WorkFlows
        public const string GUIPages_Field = "GUI-Pages";
        public const string GUIFeature_Field = "GUI-Feature";

        // Page
        public const string GUIPageField = "GUI-Page";
        public const string WorkFlow_Field = "GUI-WorkFlow";
        public const string GUI_Controls_Field = "GUI-Controls";

        // Role Access Groups

        public const string Data_RoleAccessGroup_Field = "Data-RoleAccessGroup";


        // Controls

        public const string LabelTextField = "Label text";
        public const string GUI_Position_Index_Field = "GUI Position Index";
        public const string GUI_IsRequired = "GUI-IsRequired";
        public const string GUIControlType = "GUI-Control Type";
        public const string GUIControl_Label = "Label";
        public const string GUIControl_TextBox = "Text Box";
        public const string GUIControl_SingleSelect_ComboBox = "Combo Box";
        public const string GUIControl_MultiSelect_ListBox = "List Box";
        public const string GUIControl_Button = "Button";
        public const string GUIControl_Base_Data_Field = "Base-Data-Field";
        public const string GUIControl_Base_Data_Table = "Base-Data-Table";
        public const string GUI_Add_Page_Field = "Add-Page";
        public const string GUI_Edit_Page_Field = "Edit-Page";

        // control actions

        public const string GUIControlActions_Field = "GUI-ControlActions";

        public const string GUI_ActionType_Field = "Action Type";

        public const string GUI_Parameters_Field = "Parameters";

        public const string GUI_ActionType_DataStorage_Create = "21016f79-c602-4aee-8cbd-5fec946e88e2";

        // control validations

        public const string GUI_ValidationType_Field = "Validation Type";
        public const string GUIControlValidations_Field = "GUI-ControlValidations";

    }
}
