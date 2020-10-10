using System.Data;

namespace TimeTable.ViewModel
{
    public class TeacherViewModel : DefaultViewModel<Entity.Teacher>
    {
        public TeacherViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetTeachers().Rows)
            {
                Model.Add(
                    new Entity.Teacher {
                        Id = (uint)(long)row["Id"],
                        Name = row["Name"].ToString()
                    }
                );
            }
        }
    }
}
