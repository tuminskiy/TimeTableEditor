using System;
using System.Data;

namespace TimeTable.ViewModel
{
    public class LessonTimeViewModel : DefaultViewModel<Entity.LessonTime>
    {
        public LessonTimeViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetLessonTimes().Rows)
            {
                Model.Add(
                    new Entity.LessonTime {
                        Id = (uint)(long)row["Id"],
                        Start = ((DateTime)row["Start"]).TimeOfDay,
                        End = ((DateTime)row["End"]).TimeOfDay
                    }
                );
            }
        }
    }
}
