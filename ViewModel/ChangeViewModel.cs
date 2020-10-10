using System;
using System.Data;

namespace TimeTable.ViewModel
{
    public class ChangeViewModel : SensitiveViewModel<Entity.Change>
    {
        public ChangeViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetChanges().Rows)
            {
                Model.Add(
                    new Entity.Change
                    {
                        Id = (uint)(long)row["Id"],
                        Date = DateTime.Parse((string)row["Date"]),
                        LessonTime = database.GetLessonTime((uint)(long)row["LessonTimeId"]),
                        StudyGroup = database.GetStudyGroup((uint)(long)row["StudyGroupId"]),
                        Lesson = database.GetLesson((uint)(long)row["LessonId"]),
                        Classroom = database.GetClassroom((uint)(long)row["ClassroomId"]),
                        Teacher = database.GetTeacher((uint)(long)row["TeacherId"]),
                        AdditionalInfo = row["AdditionalInfo"].ToString()
                    }
                );
            }
        }

        protected override void Assign(Entity.Change lhs, Entity.Change rhs)
        {
            lhs.Date = rhs.Date;
            lhs.LessonTime = rhs.LessonTime;
            lhs.StudyGroup = rhs.StudyGroup;
            lhs.Lesson = rhs.Lesson;
            lhs.Classroom = rhs.Classroom;
            lhs.Teacher = rhs.Teacher;
            lhs.AdditionalInfo = rhs.AdditionalInfo;
        }
    }
}
