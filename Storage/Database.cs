using System.Data;

namespace TimeTable.Storage
{
    public abstract class Database 
    {
        protected DatabaseConfiguration Configuration { get; set; }
        protected IDbConnection Connection { get; set; }

        public Database(DatabaseConfiguration config)
        {
            Configuration = config;
        }

        public abstract DataTable GetLessonTimes();
        public abstract DataTable GetStudyGroups();
        public abstract DataTable GetLessons();
        public abstract DataTable GetClassrooms();
        public abstract DataTable GetTeachers();
        public abstract DataTable GetSchedules();
        public abstract DataTable GetChanges();

        public abstract Entity.LessonTime GetLessonTime(uint id);
        public abstract Entity.StudyGroup GetStudyGroup(uint id);
        public abstract Entity.Lesson GetLesson(uint id);
        public abstract Entity.Classroom GetClassroom(uint id);
        public abstract Entity.Teacher GetTeacher(uint id);

        public abstract uint Insert(Entity.LessonTime item);
        public abstract uint Insert(Entity.StudyGroup item);
        public abstract uint Insert(Entity.Lesson item);
        public abstract uint Insert(Entity.Classroom item);
        public abstract uint Insert(Entity.Teacher item);
        public abstract uint Insert(Entity.Schedule item);
        public abstract uint Insert(Entity.Change item);

        public abstract bool Delete(Entity.Schedule item);
        public abstract bool Delete(Entity.Change item);

        public abstract bool Update(Entity.Schedule item);
        public abstract bool Update(Entity.Change item);
    }
}
