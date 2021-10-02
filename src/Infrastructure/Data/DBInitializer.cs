using Domain.Entities;
using Domain.Entities.Data;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StructureDBContext context)
        {
            context.Database.EnsureCreated();

            if (!context.DAppStructures.Any())
            {
                var structure = new DCAppStructure(Guid.NewGuid());
                structure.Name = "My organization";

                var internalGroup = new DCAppInternalGroup(Guid.NewGuid()) { Name = EFStringConstants.InternalGroup };

                var sys_PlatformGroup = new DCAppInternalGroup(Guid.NewGuid()) { Name = EFStringConstants.SystemPlatformGroup };
                sys_PlatformGroup.ParentGroup = internalGroup;
                internalGroup.ChildGroups.Add(sys_PlatformGroup);

                // begin for testing
                var internal21Group = new DCAppInternalGroup(Guid.NewGuid()) { Name = "HRM" };
                var internal22Group = new DCAppInternalGroup(Guid.NewGuid()) { Name = "Finance" };
                internal21Group.ParentGroup = sys_PlatformGroup;
                internal22Group.ParentGroup = sys_PlatformGroup;
                sys_PlatformGroup.ChildGroups.Add(internal21Group);
                sys_PlatformGroup.ChildGroups.Add(internal22Group);
                // end for testing

                var externalGroup = new DCAppExternalGroup(Guid.NewGuid()) { Name = EFStringConstants.ExternalGroup };
                structure.InternalGroup = internalGroup;
                structure.ExternalGroup = externalGroup;

                context.DAppGroups.Add(internalGroup);
                context.DAppGroups.Add(sys_PlatformGroup);

                // begin for testing
                context.DAppGroups.Add(internal21Group);
                context.DAppGroups.Add(internal22Group);
                // end for testing

                context.DAppStructures.Add(structure);
            }


            // TO be changed *****

            if (context.DAppDataModels.FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.FeaturesTable)) != null)
            {
                if (!context.DAppDataValues.Any())
                {
                    // Add base tables

                    // Add base Values
                    InitializeWithBaseValues(context);
                }
            }

            var saved = false;
            while (!saved)
            {
                try
                {
                    // Attempt to save changes to the database


                    context.SaveChanges();
                    saved = true;


                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is DCAppPage)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        private static void InitializeWithBaseValues(StructureDBContext context)
        {
            var systemPlatformGroupID = context.DAppGroups.FirstOrDefault(x => x.Name == EFStringConstants.SystemPlatformGroup).Id;
            var sysDMGroupRowID = Guid.NewGuid();
            // Groups 
            var groupTable = context.DAppDataModels
           .Include(x => x.DataFields)
           .ThenInclude(y => y.DataDefinition)
           .FirstOrDefault(t => t.Name == EFStringConstants.GroupsTable);

            var systemGroupRowID = Guid.NewGuid();

            // Group
            foreach (var dataField in groupTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.Reference_Group_Field:
                        var groupDataValue = new DCAppDataValue(new Guid());
                        groupDataValue.Name = EFStringConstants.SystemPlatformGroup;
                        groupDataValue.RowId = sysDMGroupRowID;
                        groupDataValue.DataField = dataField;
                        groupDataValue.SingleReferenceRowId = systemPlatformGroupID;
                        context.DAppDataValues.Add(groupDataValue);
                        break;

                    case EFStringConstants.NameField:
                        var groupNameDataValue = new DCAppDataValue(new Guid());
                        groupNameDataValue.Name = EFStringConstants.SystemPlatformGroup;
                        groupNameDataValue.RowId = sysDMGroupRowID;
                        groupNameDataValue.DataField = dataField;
                        groupNameDataValue.Value = EFStringConstants.SystemPlatformGroup;
                        context.DAppDataValues.Add(groupNameDataValue);
                        break;

                    default:
                        // code block
                        break;
                }

            }

            // ControlAction
            var controlActionsTable = context.DAppDataModels
               .Include(x => x.DataFields)
               .ThenInclude(y => y.DataDefinition)
               .FirstOrDefault(t => t.Name == EFStringConstants.GUIControlActionsTable);

            var controlValidationsTable = context.DAppDataModels
               .Include(x => x.DataFields)
               .ThenInclude(y => y.DataDefinition)
               .FirstOrDefault(t => t.Name == EFStringConstants.GUIControlValidationsTable);

            var dB_Item_CreateRowID = Guid.NewGuid();

            var pagesTable = context.DAppDataModels
             .Include(x => x.DataFields)
             .ThenInclude(y => y.DataDefinition)
             .FirstOrDefault(t => t.Name == EFStringConstants.PagesTable);

            var workFlowsTable = context.DAppDataModels
          .Include(x => x.DataFields)
          .ThenInclude(y => y.DataDefinition)
          .FirstOrDefault(t => t.Name == EFStringConstants.WorkFlowsTable);

            var controlsTable = context.DAppDataModels
             .Include(x => x.DataFields)
             .ThenInclude(y => y.DataDefinition)
             .FirstOrDefault(t => t.Name == EFStringConstants.GUIControlsTable);

            var featuresTable = context.DAppDataModels
               .Include(x => x.DataFields)
               .ThenInclude(y => y.DataDefinition)
               .FirstOrDefault(t => t.Name == EFStringConstants.FeaturesTable);

            var workFlowRowID = Guid.NewGuid();

            var addFeaturePageRowID = Guid.NewGuid();
            var addWorkFlowPageRowID = Guid.NewGuid();
            var addRAGPageRowID = Guid.NewGuid();
            var addPagePageRowID = Guid.NewGuid();
            var addControlPageRowID = Guid.NewGuid();
            var addControlActionPageRowID = Guid.NewGuid();
            var addControlValidationPageRowID = Guid.NewGuid();


            foreach (var dataField in controlActionsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.GUI_ActionType_Field:
                        var guiControlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        guiControlActionDataValue.Name = controlActionsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        guiControlActionDataValue.RowId = dB_Item_CreateRowID;
                        guiControlActionDataValue.DataField = dataField;
                        guiControlActionDataValue.Value = EFStringConstants.GUI_ActionType_DataStorage_Create;
                        context.DAppDataValues.Add(guiControlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }
            }

            AddDVsOfAddFeaturePage(
            featuresTable, workFlowsTable,
            pagesTable, controlsTable,
            systemGroupRowID,
            addFeaturePageRowID,
            addWorkFlowPageRowID,
            workFlowRowID,
            dB_Item_CreateRowID,
            context);

            AddDVsOfAddWorkFlowPage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           addWorkFlowPageRowID,
           addRAGPageRowID,
           addPagePageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);

            AddDVsOfAddRoleAccessGroupPage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           addRAGPageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);

            AddDVsOfAddPagePage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           addPagePageRowID,
           addControlPageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);

            AddDVsOfAddGUIControlPage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           addControlPageRowID,
           addControlActionPageRowID,
           addControlValidationPageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);

            AddDVsOfAddGUIControlActionPage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           controlActionsTable,
           addControlActionPageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);

            AddDVsOfAddGUIControlValidationPage(
           featuresTable, workFlowsTable,
           pagesTable, controlsTable,
           controlValidationsTable,
           addControlValidationPageRowID,
           workFlowRowID,
           dB_Item_CreateRowID,
           context);


            // WorkFlows

            var featureRowID = Guid.NewGuid();

            foreach (var dataField in workFlowsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = workFlowsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Add Feature";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descriptiondataValue = new DCAppDataValue(Guid.NewGuid());
                        descriptiondataValue.Name = workFlowsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descriptiondataValue.RowId = workFlowRowID;
                        descriptiondataValue.DataField = dataField;
                        descriptiondataValue.Value = "Add a new Feature";
                        context.DAppDataValues.Add(descriptiondataValue);
                        break;

                    case EFStringConstants.GUIPages_Field:
                        var pagesdataValue = new DCAppDataValue(Guid.NewGuid());
                        pagesdataValue.Name = workFlowsTable.Name + " : " + EFStringConstants.GUIPages_Field;
                        pagesdataValue.RowId = workFlowRowID;
                        pagesdataValue.DataField = dataField;
                        pagesdataValue.MultipleReferenceRowIds.Add(addFeaturePageRowID);
                        context.DAppDataValues.Add(pagesdataValue);
                        break;

                    case EFStringConstants.GUIFeature_Field:
                        var featuredataValue = new DCAppDataValue(Guid.NewGuid());
                        featuredataValue.Name = workFlowsTable.Name + " : " + EFStringConstants.GUIFeature_Field;
                        featuredataValue.RowId = workFlowRowID;
                        featuredataValue.DataField = dataField;
                        featuredataValue.SingleReferenceRowId = featureRowID;
                        context.DAppDataValues.Add(featuredataValue);
                        break;

                    default:
                        // code block
                        break;
                }

            }


            // Features

            foreach (var dataField in featuresTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = featuresTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featureRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Management";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = featuresTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featureRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Manage the features.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.Reference_Group_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = featuresTable.Name + " : " + EFStringConstants.Reference_Group_Field;
                        groupdataValue.RowId = featureRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.SingleReferenceRowId = sysDMGroupRowID;
                        context.DAppDataValues.Add(groupdataValue);
                        break;

                    case EFStringConstants.WorkFlows_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = featuresTable.Name + " : " + EFStringConstants.WorkFlows_Field;
                        workflowsdataValue.RowId = featureRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.MultipleReferenceRowIds.Add(workFlowRowID);
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;

                    default:
                        // code block
                        break;
                }

            }

        }

        private static void AddDVsOfAddFeaturePage(
            DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
            DCAppDataModel pagesTable, DCAppDataModel controlsTable,
            Guid systemGroupRowID,
            Guid addFeaturePageRowID,
            Guid addWorkFlowPageRowID,
            Guid workFlowRowID,
            Guid dB_Item_CreateRowID,
            StructureDBContext context)
        {
            // Controls

            var featureNameDataField = featuresTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var featureDesciptionDataField = featuresTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var featureGroupDataField = featuresTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.Reference_Group_Field);
            var featureWorkFlowsDataField = featuresTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);

            var featureNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featureNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featureNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = featureNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Feature Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = featureNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = featureNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = featureNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = featureNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = featureNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addFeaturePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var featuredescriptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featuredescriptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featuredescriptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = featuredescriptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = featuredescriptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = featuredescriptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = featuredescriptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = featureDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = featuredescriptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addFeaturePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var featureGroupControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featureGroupControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Group";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featureGroupControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The group that this feature belongs to";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = featureGroupControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Group";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = featureGroupControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = featureGroupControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = featureGroupControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = featureGroupDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = featureGroupControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addFeaturePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var featureWorkFlowsControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featureWorkFlowsControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The WorkFlows";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featureWorkFlowsControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The WorkFlows of the feature";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = featureWorkFlowsControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The WorkFlows";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = featureWorkFlowsControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_MultiSelect_ListBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = featureWorkFlowsControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = featureWorkFlowsControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = featureWorkFlowsDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = featureWorkFlowsControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addFeaturePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var addPageDataValue = new DCAppDataValue(Guid.NewGuid());
                        addPageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        addPageDataValue.RowId = featureWorkFlowsControlRowId;
                        addPageDataValue.DataField = dataField;
                        addPageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(addPageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var featureCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = featureCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Feature";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = featureCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = featureCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Feature";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = featureCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = featureCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = featureCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addFeaturePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = featureCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addFeaturePageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create a new Feature";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addFeaturePageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addFeaturePageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addFeaturePageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(featureNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(featuredescriptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(featureGroupControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(featureWorkFlowsControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(featureCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addFeaturePageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = featuresTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }

        private static void AddDVsOfAddWorkFlowPage(
           DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
           DCAppDataModel pagesTable, DCAppDataModel controlsTable,
           Guid addWorkFlowPageRowID,
           Guid addRAGPageRowID,
           Guid addPagePageRowID,
           Guid workFlowRowID,
           Guid dB_Item_CreateRowID,
           StructureDBContext context)
        {
            // Controls

            var workFlowNameDataField = workFlowsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var workFlowDesciptionDataField = workFlowsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var workFlowRAGDataField = workFlowsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.Data_RoleAccessGroup_Field);
            var workFlowPagesDataField = workFlowsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUIPages_Field);


            var workFlowNameControlRowID = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowNameControlRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Add Feature";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = workFlowNameControlRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Adds a new Feature";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = workFlowNameControlRowID;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Add Feature";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = workFlowNameControlRowID;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = workFlowNameControlRowID;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = workFlowNameControlRowID;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = workFlowNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = workFlowNameControlRowID;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var workFlowDesciptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowDesciptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Add Feature Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = workFlowDesciptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Adds a new Feature";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = workFlowDesciptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Adds a new Feature";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = workFlowDesciptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = workFlowDesciptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = workFlowDesciptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = workFlowDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = workFlowDesciptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var workFlowRAGControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowRAGControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Role Access Group";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = workFlowRAGControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Assigns the access levels to Roles";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = workFlowRAGControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Assigns the access levels to Roles";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = workFlowRAGControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = workFlowRAGControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = workFlowRAGControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = workFlowRAGDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = workFlowRAGControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        controlActionDataValue.RowId = workFlowRAGControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = addRAGPageRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var workFlowPagesControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowPagesControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The WorkFlows";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = workFlowPagesControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The Pages of this WorkFlow";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = workFlowPagesControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Pages";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = workFlowPagesControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = workFlowPagesControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = workFlowPagesControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = workFlowPagesDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = workFlowPagesControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        controlActionDataValue.RowId = workFlowPagesControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var workFlowCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = workFlowCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create WorkFlow";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = workFlowCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Creates a new WorkFlow";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = workFlowCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create WorkFlow";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = workFlowCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = workFlowCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = workFlowCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addWorkFlowPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = workFlowCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addWorkFlowPageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create a new WorkFlow";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addWorkFlowPageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new WorkFlow";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addWorkFlowPageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var controlsdataValue = new DCAppDataValue(Guid.NewGuid());
                        controlsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        controlsdataValue.RowId = addWorkFlowPageRowID;
                        controlsdataValue.DataField = dataField;
                        controlsdataValue.MultipleReferenceRowIds.Add(workFlowNameControlRowID);
                        controlsdataValue.MultipleReferenceRowIds.Add(workFlowDesciptionControlRowId);
                        controlsdataValue.MultipleReferenceRowIds.Add(workFlowRAGControlRowId);
                        controlsdataValue.MultipleReferenceRowIds.Add(workFlowPagesControlRowId);
                        controlsdataValue.MultipleReferenceRowIds.Add(workFlowCreateButtonControlRowId);
                        context.DAppDataValues.Add(controlsdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addWorkFlowPageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = workFlowsTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }

        private static void AddDVsOfAddPagePage(
           DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
           DCAppDataModel pagesTable, DCAppDataModel controlsTable,
           Guid addPagePageRowID,
           Guid addControlPageRowID,
           Guid workFlowRowID,
           Guid dB_Item_CreateRowID,
           StructureDBContext context)
        {
            // Controls

            var pageNameDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var pageDesciptionDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var pageControlsDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUI_Controls_Field);
            var pageBaseDataTableDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUIControl_Base_Data_Table);

            var pageNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Feature Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageDescriptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageDescriptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageDescriptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageDescriptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageDescriptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageDescriptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageDescriptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageDescriptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageBaseDataTableControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageBaseDataTableControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageBaseDataTableControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageBaseDataTableControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Feature Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageBaseDataTableControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageBaseDataTableControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageBaseDataTableControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageBaseDataTableDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageBaseDataTableControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageControlsControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageControlsControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Controls";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageControlsControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The controls of the page.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageControlsControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Controls";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageControlsControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageControlsControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageControlsControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageControlsDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageControlsControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        controlActionDataValue.RowId = pageControlsControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Page";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the creation of a new Page";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Page";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addPagePageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = pageCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addPagePageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create Page";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addPagePageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Page";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addPagePageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addPagePageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(pageNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageDescriptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageBaseDataTableControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageControlsControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addPagePageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = pagesTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }

        private static void AddDVsOfAddRoleAccessGroupPage(
         DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
         DCAppDataModel pagesTable, DCAppDataModel controlsTable,
         Guid addRAGPageRowID,
         Guid workFlowRowID,
         Guid dB_Item_CreateRowID,
         StructureDBContext context)
        {
            // Controls

            var pageNameDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var pageDesciptionDataField = pagesTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);

            var pageNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Role Access Group Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the Role Access Group";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Role Access Group Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addRAGPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageDescriptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageDescriptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Role Access Group Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageDescriptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the Role Access Group";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageDescriptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Role Access Group Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageDescriptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageDescriptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = pageDescriptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = pageDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageDescriptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addRAGPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Role Access Group";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the creation of a new Role Access Group";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Role Access Group";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addRAGPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = pageCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addRAGPageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create Page";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addRAGPageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Page";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addRAGPageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addRAGPageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(pageNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageDescriptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addRAGPageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = pagesTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }


        private static void AddDVsOfAddGUIControlPage(
           DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
           DCAppDataModel pagesTable, DCAppDataModel controlsTable,
           Guid addControlPageRowID,
           Guid addControlActionPageRowID,
            Guid addControlValidationPageRowID,
           Guid workFlowRowID,
           Guid dB_Item_CreateRowID,
           StructureDBContext context)
        {
            // Controls

            /*
                     Name	The name of the GUI field	String	
                    -Description	Description of the control	String
                    GUI-Validations	Validations	EntityReference	
                    -GUI-Control Type	The respective gui-control	Choices	
                    -Label text	The text that goes with the GUI-Control	String	
                    -GUI Position Index	The positional index of the GUI control within the page	Number	
                    -GUI-ControlActions	The list of actions	EntityReference	
                    -Base-Data-Field	The data-field	EntityReference	
                    -GUI-IsRequired	Flag for required	Bool	
                    -GUI-Page	The owning page of this control	EntityReference
             */

            var controlNameDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var controlDesciptionDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var controlControlTypeDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.Reference_Group_Field);
            var controlActionsDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlValidationsDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlLabelTextDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlPositionalIndexDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlIsRequiredDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlBaseDataFieldDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);
            var controlOwnerPageDataField = controlsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.WorkFlows_Field);

            var controlNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control's Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the control.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control's Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.MultipleReferenceRowIds.Add(addControlPageRowID);
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlDesciptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlDesciptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control's Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlDesciptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the control.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlDesciptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control's Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlDesciptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlDesciptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlDesciptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlDesciptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlControlTypeControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlControlTypeControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control Type Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlControlTypeControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The type of the control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlControlTypeControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The type of the control";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlControlTypeControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlControlTypeControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlControlTypeControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlControlTypeDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlControlTypeControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlActionsControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionsControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Group";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionsControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The group that this feature belongs to.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionsControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Group";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionsControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionsControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlActionsControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionsDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionsControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        controlActionDataValue.RowId = controlActionsControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlValidationsControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationsControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Validations";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationsControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The validations for this control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationsControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Validations";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationsControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationsControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlValidationsControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionsDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationsControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    case EFStringConstants.GUI_Add_Page_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Add_Page_Field;
                        controlActionDataValue.RowId = controlValidationsControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlLabelTextControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlLabelTextControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Label Text";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlLabelTextControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the label text of the control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlLabelTextControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Label Text";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlLabelTextControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlLabelTextControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlLabelTextControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlLabelTextDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlLabelTextControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlPositionalIndexControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlPositionalIndexControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Positional Index";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlPositionalIndexControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the positional index of the control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlPositionalIndexControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Positional Index";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlPositionalIndexControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlPositionalIndexControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlPositionalIndexControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlPositionalIndexDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlPositionalIndexControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlIsRequiredControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlIsRequiredControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Is Required";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlIsRequiredControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the Is Required flag for the control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlIsRequiredControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Is Required";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlIsRequiredControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlIsRequiredControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlIsRequiredControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlIsRequiredDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlIsRequiredControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlBaseDataFieldDataFieldControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Base Data Field";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the Base Data Field of the control";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Base Data Field";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlBaseDataFieldDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlBaseDataFieldDataFieldControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var pageCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = pageCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Feature";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = pageCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the feature.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = pageCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Feature";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = pageCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = pageCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = pageCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = pageCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;

                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addControlPageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create a new Control";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addControlPageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Control.";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addControlPageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addControlPageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(controlNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlDesciptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlControlTypeControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionsControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationsControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlLabelTextControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlPositionalIndexControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlIsRequiredControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlBaseDataFieldDataFieldControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(pageCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addControlPageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = controlsTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }


        private static void AddDVsOfAddGUIControlActionPage(
           DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
           DCAppDataModel pagesTable, DCAppDataModel controlsTable,
           DCAppDataModel controlActionsTable,
           Guid addControlActionPageRowID,
           Guid workFlowRowID,
           Guid dB_Item_CreateRowID,
           StructureDBContext context)
        {
            // Controls

            var controlActionNameDataField = controlActionsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var controlActionDesciptionDataField = controlActionsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var controlActionTypeDataField = controlActionsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUI_ActionType_Field);
            var controlActionParametersDataField = controlActionsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUI_Parameters_Field);

            var controlActionNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control Action Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the Control Action";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control Action Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlActionNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlActionDesciptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionDesciptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control Action Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionDesciptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the Control Action";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionDesciptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control Action Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionDesciptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionDesciptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlActionDesciptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionDesciptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionDesciptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlActionTypeControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionTypeControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control Action Type";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionTypeControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the Control Action Type";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionTypeControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control Action Type";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionTypeControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionTypeControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlActionTypeControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionTypeDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionTypeControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlActionParametersControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionParametersControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Control Action Parameters";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionParametersControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The Control Action Parameters";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionParametersControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Control Action Parameters";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionParametersControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionParametersControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlActionParametersControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlActionParametersDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionParametersControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlActionCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlActionCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Control Action";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlActionCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the creation of Control Action";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlActionCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Control Action";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlActionCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlActionCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlActionCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlActionPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = controlActionCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.MultipleReferenceRowIds.Add(dB_Item_CreateRowID);
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addControlActionPageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create a new Control Action";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addControlActionPageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Control Action";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addControlActionPageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addControlActionPageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionDesciptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionTypeControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionParametersControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlActionCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addControlActionPageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = controlActionsTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }

        private static void AddDVsOfAddGUIControlValidationPage(
           DCAppDataModel featuresTable, DCAppDataModel workFlowsTable,
           DCAppDataModel pagesTable, DCAppDataModel controlsTable,
           DCAppDataModel controlValidationsTable,
           Guid addControlValidationPageRowID,
           Guid workFlowRowID,
           Guid dB_Item_CreateRowID,
           StructureDBContext context)
        {
            // Controls

            var controlValidationNameDataField = controlValidationsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.NameField);
            var controlValidationDescriptionDataField = controlValidationsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.DescriptionField);
            var controlValidationTypeDataField = controlValidationsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUI_ValidationType_Field);
            var controlValidationParametersDataField = controlValidationsTable.DataFields.FirstOrDefault(x => x.Name == EFStringConstants.GUI_Parameters_Field);

            var controlValidationNameControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationNameControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Validation Name";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationNameControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the name of the Validation.";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationNameControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Validation Name";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationNameControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationNameControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "0";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlValidationNameControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlValidationNameDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationNameControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlValidationdescriptionControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationdescriptionControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Validation Description";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationdescriptionControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The description for Validation";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationdescriptionControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Validation Description";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationdescriptionControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationdescriptionControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "1";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlValidationdescriptionControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlValidationDescriptionDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationdescriptionControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlValidationTypeControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationTypeControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Validation Type";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationTypeControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The type of control Validation";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationTypeControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Validation Type";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationTypeControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_SingleSelect_ComboBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationTypeControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "2";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlValidationTypeControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlValidationTypeDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationTypeControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlValidationParametersControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationParametersControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "The Parameters";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationParametersControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "The Parameters for the validation";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationParametersControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "The Parameters";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationParametersControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_TextBox;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationParametersControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "3";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Field:
                        var baseDataFieldDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataFieldDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Field;
                        baseDataFieldDataValue.RowId = controlValidationParametersControlRowId;
                        baseDataFieldDataValue.DataField = dataField;
                        baseDataFieldDataValue.BaseDataField = controlValidationParametersDataField;
                        context.DAppDataValues.Add(baseDataFieldDataValue);
                        break;

                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationParametersControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }

            var controlValidationCreateButtonControlRowId = Guid.NewGuid();

            foreach (var dataField in controlsTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = controlsTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = controlValidationCreateButtonControlRowId;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Create Control Validation";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = controlsTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = controlValidationCreateButtonControlRowId;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "For the description of the Create Control Validation button";
                        context.DAppDataValues.Add(descdataValue);
                        break;

                    case EFStringConstants.LabelTextField:
                        var labelDataValue = new DCAppDataValue(Guid.NewGuid());
                        labelDataValue.Name = controlsTable.Name + " : " + EFStringConstants.LabelTextField;
                        labelDataValue.RowId = controlValidationCreateButtonControlRowId;
                        labelDataValue.DataField = dataField;
                        labelDataValue.Value = "Create Validation";
                        context.DAppDataValues.Add(labelDataValue);
                        break;

                    case EFStringConstants.GUIControlType:
                        var controlTypeDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlTypeDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIControlType;
                        controlTypeDataValue.RowId = controlValidationCreateButtonControlRowId;
                        controlTypeDataValue.DataField = dataField;
                        controlTypeDataValue.Value = EFStringConstants.GUIControl_Button;
                        context.DAppDataValues.Add(controlTypeDataValue);
                        break;
                    case EFStringConstants.GUI_Position_Index_Field:
                        var positionalIndexDataValue = new DCAppDataValue(Guid.NewGuid());
                        positionalIndexDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_Position_Index_Field;
                        positionalIndexDataValue.RowId = controlValidationCreateButtonControlRowId;
                        positionalIndexDataValue.DataField = dataField;
                        positionalIndexDataValue.Value = "4";
                        context.DAppDataValues.Add(positionalIndexDataValue);
                        break;
                    case EFStringConstants.GUIPageField:
                        var pageDataValue = new DCAppDataValue(Guid.NewGuid());
                        pageDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUIPageField;
                        pageDataValue.RowId = controlValidationCreateButtonControlRowId;
                        pageDataValue.DataField = dataField;
                        pageDataValue.SingleReferenceRowId = addControlValidationPageRowID;
                        context.DAppDataValues.Add(pageDataValue);
                        break;

                    case EFStringConstants.GUI_ActionType_Field:
                        var controlActionDataValue = new DCAppDataValue(Guid.NewGuid());
                        controlActionDataValue.Name = controlsTable.Name + " : " + EFStringConstants.GUI_ActionType_Field;
                        controlActionDataValue.RowId = controlValidationCreateButtonControlRowId;
                        controlActionDataValue.DataField = dataField;
                        controlActionDataValue.SingleReferenceRowId = dB_Item_CreateRowID;
                        context.DAppDataValues.Add(controlActionDataValue);
                        break;


                    default:
                        // code block
                        break;
                }

            }

            //groupdataValue.ReferenceRowIds.Add(systemPlatformGroupID);

            // Page


            foreach (var dataField in pagesTable.DataFields)
            {
                switch (dataField.Name)
                {
                    case EFStringConstants.NameField:
                        var namedataValue = new DCAppDataValue(Guid.NewGuid());
                        namedataValue.Name = pagesTable.Name + " : " + EFStringConstants.NameField;
                        namedataValue.RowId = addControlValidationPageRowID;
                        namedataValue.DataField = dataField;
                        namedataValue.Value = "Page: Create Control Validation";
                        context.DAppDataValues.Add(namedataValue);
                        break;

                    case EFStringConstants.DescriptionField:
                        var descdataValue = new DCAppDataValue(Guid.NewGuid());
                        descdataValue.Name = pagesTable.Name + " : " + EFStringConstants.DescriptionField;
                        descdataValue.RowId = addControlValidationPageRowID;
                        descdataValue.DataField = dataField;
                        descdataValue.Value = "Page to create a new Control Validation";
                        context.DAppDataValues.Add(descdataValue);
                        break;
                    case EFStringConstants.WorkFlow_Field:
                        var workflowsdataValue = new DCAppDataValue(Guid.NewGuid());
                        workflowsdataValue.Name = pagesTable.Name + " : " + EFStringConstants.WorkFlow_Field;
                        workflowsdataValue.RowId = addControlValidationPageRowID;
                        workflowsdataValue.DataField = dataField;
                        workflowsdataValue.SingleReferenceRowId = workFlowRowID;
                        context.DAppDataValues.Add(workflowsdataValue);
                        break;
                    case EFStringConstants.GUI_Controls_Field:
                        var groupdataValue = new DCAppDataValue(Guid.NewGuid());
                        groupdataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUI_Controls_Field;
                        groupdataValue.RowId = addControlValidationPageRowID;
                        groupdataValue.DataField = dataField;
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationNameControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationdescriptionControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationTypeControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationParametersControlRowId);
                        groupdataValue.MultipleReferenceRowIds.Add(controlValidationCreateButtonControlRowId);
                        context.DAppDataValues.Add(groupdataValue);
                        break;
                    case EFStringConstants.GUIControl_Base_Data_Table:
                        var baseDataModelDataValue = new DCAppDataValue(Guid.NewGuid());
                        baseDataModelDataValue.Name = pagesTable.Name + " : " + EFStringConstants.GUIControl_Base_Data_Table;
                        baseDataModelDataValue.RowId = addControlValidationPageRowID;
                        baseDataModelDataValue.DataField = dataField;
                        baseDataModelDataValue.BaseDataModel = controlValidationsTable;
                        context.DAppDataValues.Add(baseDataModelDataValue);
                        break;
                    default:
                        // code block
                        break;
                }

            }
        }
    }
}