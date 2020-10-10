using System;
using System.Data;

namespace TimeTable.ViewModel
{
    public class ScheduleViewModel : SensitiveViewModel<Entity.Schedule>
    {
        public ScheduleViewModel(Storage.Database db) : base(db)
        {
            foreach (DataRow row in database.GetSchedules().Rows)
            {
                Model.Add(
                    new Entity.Schedule {
                        Id = (uint)(long)row["Id"],
                        DayOfWeek = (DayOfWeek)(long)row["DayOfWeek"],
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

        protected override void Assign(Entity.Schedule lhs, Entity.Schedule rhs)
        {
            lhs.DayOfWeek = rhs.DayOfWeek;
            lhs.LessonTime = rhs.LessonTime;
            lhs.StudyGroup = rhs.StudyGroup;
            lhs.Lesson = rhs.Lesson;
            lhs.Classroom = rhs.Classroom;
            lhs.Teacher = rhs.Teacher;
            lhs.AdditionalInfo = rhs.AdditionalInfo;
        }
    }
}
