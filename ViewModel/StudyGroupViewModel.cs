using System.Data;

namespace TimeTable.ViewModel
{
    public class StudyGroupViewModel : DefaultViewModel<Entity.StudyGroup>
    {
        public StudyGroupViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetStudyGroups().Rows)
            {
                Model.Add(
                    new Entity.StudyGroup {
                        Id = (uint)(long)row["Id"],
                        Name = row["Name"].ToString()
                    }
                ); ;
            }
        }
    }
}
