using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace TimeTable.Utility
{
    public class SensitiveObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public SensitiveObservableCollection()
        {
            CollectionChanged += SensitiveObservableCollection_CollectionChanged;
        }

        private void SensitiveObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (T item in e.NewItems)
                    item.PropertyChanged += Item_PropertyChanged;
            }
            else if (e.OldItems != null)
            {
                foreach (T item in e.OldItems)
                    item.PropertyChanged -= Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
