using System.Data;

namespace TimeTable.ViewModel
{
    public class LessonViewModel : DefaultViewModel<Entity.Lesson>
    {
        public LessonViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetLessons().Rows)
            {
                Model.Add(
                    new Entity.Lesson {
                        Id = (uint)(long)row["Id"],
                        Name = row["Name"].ToString()
                    }
                );
            }
        }
    }
}
