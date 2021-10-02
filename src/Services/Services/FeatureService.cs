using Infrastructure.Data;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Presentation.EntityBuilders;
using Syncfusion.EJ2.Spreadsheet;

namespace Presentation.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly StructureDBContext _structureDBContext;

        public FeatureService(StructureDBContext structureDBContext)
        {
            _structureDBContext = structureDBContext;
        }
        public void AddFeatureToStructure(DCAppFeatureViewModel featureModel)
        {
            throw new NotImplementedException();
        }

        public DCAppFeatureViewModel GetFeature(Guid FeatureId, Guid StructureId)
        {
            throw new NotImplementedException();
        }

        public DCAppServiceMessage GetFeatureList(Guid groupId)
        {
            try
            {
                if (_structureDBContext.DAppDataModels
                    .FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.FeaturesTable)) != null)
                {
                    var featureTable = _structureDBContext.DAppDataModels.Include(y => y.DataFields)
                        .FirstOrDefault(x => (x.IsInternal && x.IsSystemBased && x.Name == EFStringConstants.FeaturesTable));

                    var nameDataValues = _structureDBContext.DAppDataValues.Include(d => d.DataField)
                        .Where(x => x.DataField.DCAppDataModelId == featureTable.Id && x.DataField.Name == EFStringConstants.NameField).ToList();

                    var items = new List<DCAppFeatureViewModel>();
                    foreach (var nameDataValue in nameDataValues)
                    {
                        var dataValuesInRow = _structureDBContext.DAppDataValues.Include(d => d.DataField)
                            .Where(x => x.RowId == nameDataValue.RowId && x.Id != nameDataValue.Id).ToList();

                        var featureViewModel = new DCAppFeatureViewModel();
                        featureViewModel.Name = nameDataValue.Value;

                        foreach (var dataValueItem in dataValuesInRow)
                        {
                            switch(dataValueItem.DataField.Name)
                            {
                                case EFStringConstants.NameField:
                                    featureViewModel.Name = dataValueItem.Value;
                                    break;
                                case EFStringConstants.DescriptionField:
                                    featureViewModel.Description = dataValueItem.Value;
                                    break;

                                case EFStringConstants.Reference_Group_Field:
                                    featureViewModel.GroupId = dataValueItem.SingleReferenceRowId;
                                    break;
                                case EFStringConstants.WorkFlows_Field:
                                    foreach (var rowId in dataValueItem.MultipleReferenceRowIds)
                                    {
                                        var dataValue = _structureDBContext.DAppDataValues.Where(x => x.RowId == rowId && x.DataField.Name == EFStringConstants.NameField).FirstOrDefault();
                                        featureViewModel.WorkFlows.Add(new DCAppWorkFlowViewModel() { Id = rowId.ToString(), Name =  dataValue.Value, DataRowId = nameDataValue.RowId});
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        items.Add(featureViewModel);
                    }

                    return new DCAppServiceMessage(string.Empty, DCAppServiceResult.suceeded, items);

                }
            }
            catch(Exception exception)
            {
                return new DCAppServiceMessage(exception.Message, DCAppServiceResult.failed, new List<DCAppFeatureViewModel>());
            }

                return new DCAppServiceMessage(string.Empty, DCAppServiceResult.failed, new List<DCAppFeatureViewModel>());
          }

        public void RemoveFeatureToStructure(DCAppFeatureViewModel featureModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateFeatureToStructure(DCAppFeatureViewModel featureModel)
        {
            throw new NotImplementedException();
        }
    }
}