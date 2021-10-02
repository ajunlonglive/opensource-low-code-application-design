using Domain.Entities;
using Domain.Entities.Data;
using System;

namespace Domain.Factories
{
    public static class EntityFactory
    {
        public static DCAppDataField CreateNewDataField()
        {
            return new DCAppDataField(Guid.NewGuid());
        }

        public static DCAppDataModel CreateNewDataModel()
        {
            return new DCAppDataModel(Guid.NewGuid());
        }
        public static DCAppGroup CreateNewGroup()
        {
            return new DCAppGroup(Guid.NewGuid());
        }

        public static DCAppDataChoiceItem CreateNewDataChoiceItemModel()
        {
            return new DCAppDataChoiceItem(Guid.NewGuid());
        }
    }
}