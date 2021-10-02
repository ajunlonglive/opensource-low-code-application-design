using Presentation.ViewModels;
using System;
using System.Collections.Generic;

namespace Presentation.Services
{
    public interface IFeatureService
    {
        void AddFeatureToStructure(DCAppFeatureViewModel featureModel);

        void RemoveFeatureToStructure(DCAppFeatureViewModel featureModel);

        void UpdateFeatureToStructure(DCAppFeatureViewModel featureModel);

        DCAppFeatureViewModel GetFeature(Guid FeatureId, Guid structureId);

        DCAppServiceMessage GetFeatureList(Guid groupId);
    }
}