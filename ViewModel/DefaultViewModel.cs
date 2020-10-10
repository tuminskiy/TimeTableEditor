using System.Collections.ObjectModel;

namespace TimeTable.ViewModel
{
    public abstract class DefaultViewModel<T> : AbstractViewModel<T, ObservableCollection<T>>
        where T : Entity.AbstractEntity
    {
        public DefaultViewModel(Storage.Database db) : base(db)
        { }
    }
}
