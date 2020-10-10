using System;
using System.ComponentModel;
using System.Windows.Data;

namespace TimeTable.Utility
{
    public class DescriptSetter
    {
        public static void AddGroup(object source, params string[] groupsName)
        {
            var groupDescriptions = GetDefautCollectionView(source).GroupDescriptions;

            foreach (var groupName in groupsName)
            {
                groupDescriptions.Add(new PropertyGroupDescription(groupName));
            }
        }

        public static void AddSort(object source, params string[] propertiesName)
        {
            var sortDescriptions = GetDefautCollectionView(source).SortDescriptions;

            foreach (var propertyName in propertiesName)
            {
                sortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Ascending));
            }
        }

        public static void AddFilter(object source, Predicate<object> filter)
        {
            GetDefautCollectionView(source).Filter += filter;
        }

        private static CollectionView GetDefautCollectionView(object source)
        {
            return CollectionViewSource.GetDefaultView(source) as CollectionView;
        }
    }
}
