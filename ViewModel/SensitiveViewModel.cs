using System.ComponentModel;
using System.Linq;

namespace TimeTable.ViewModel
{
    public abstract class SensitiveViewModel<T> : AbstractViewModel<T, Utility.SensitiveObservableCollection<T>>
        where T : Entity.AbstractEntity, INotifyPropertyChanged
    {
        public SensitiveViewModel(Storage.Database db) : base(db)
        { }

        protected abstract void Assign(T lhs, T rhs);

        public bool Update(T item)
        {
            if (item is null)
                return false;

            if (database.Update((dynamic)item))
            {
                var value = Model.First(o => o.Id == item.Id);
                Assign(value, item);
                return true;
            }

            return false;
        }
    }
}
