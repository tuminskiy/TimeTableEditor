using System.Data;

namespace TimeTable.ViewModel
{
    public class ClassroomViewModel : DefaultViewModel<Entity.Classroom>
    {
        public ClassroomViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetClassrooms().Rows)
            {
                Model.Add(
                    new Entity.Classroom {
                        Id = (uint)(long)row["Id"],
                        Name = row["Name"].ToString()
                    }
                );
            }
        }
    }
}
