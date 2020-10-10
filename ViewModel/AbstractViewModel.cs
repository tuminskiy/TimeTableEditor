using System.Collections.ObjectModel;

namespace TimeTable.ViewModel
{
    public abstract class AbstractViewModel<T, C>
        where T : Entity.AbstractEntity
        where C : ObservableCollection<T>, new()
    {
        protected Storage.Database database;

        public C Model { get; set; }

        public AbstractViewModel(Storage.Database db)
        {
            database = db;

            Model = new C();
        }

        public void Add(T item)
        {
            if (item is null)
                return;

            item.Id = database.Insert((dynamic)item);
            Model.Add(item);
        }

        public bool Remove(T item)
        {
            if (item is null)
                return false;

            if (database.Delete((dynamic)item))
                return Model.Remove(item);
            
            return false;
        }
    }
}
