using System;
using System.Data;
using System.Data.SQLite;

using TimeTable.Entity;

namespace TimeTable.Storage
{
    public class SqliteDatabase : Database
    {
        public SqliteDatabase(DatabaseConfiguration config) : base(config)
        {
            Connection = new SQLiteConnection(Configuration.ConnectionString);
        }

        private DataTable Select(string query)
        {
            using (var guard = new ConnectionGuard(Connection))
            {
                var dt = new DataTable();
                var adapter = new SQLiteDataAdapter(query, Connection as SQLiteConnection);
                adapter.Fill(dt);

                return dt;
            }
        }

        public override DataTable GetLessonTimes()
        {
            return Select(Properties.Resources.SelectLessonTime);
        }
        
        public override DataTable GetStudyGroups()
        {
            return Select(Properties.Resources.SelectStudyGroup);
        }

        public override DataTable GetLessons()
        {
            return Select(Properties.Resources.SelectLesson);
        }

        public override DataTable GetClassrooms()
        {
            return Select(Properties.Resources.SelectClassroom);
        }

        public override DataTable GetTeachers()
        {
            return Select(Properties.Resources.SelectTeacher);
        }

        public override DataTable GetSchedules()
        {
            return Select(Properties.Resources.SelectSchedule);
        }

        public override DataTable GetChanges()
        {
            return Select(Properties.Resources.SelectChange);
        }


        public override LessonTime GetLessonTime(uint id)
        {
            var rows = Select(Properties.Resources.SelectLessonTime + $" WHERE Id={id}").Rows;

            if (rows.Count == 0) // Not found
                return null;

            return new LessonTime
            {
                Id = id,
                Start = ((DateTime)rows[0]["Start"]).TimeOfDay,
                End = ((DateTime)rows[0]["End"]).TimeOfDay
            };
        }

        public override StudyGroup GetStudyGroup(uint id)
        {
            var rows = Select(Properties.Resources.SelectStudyGroup + $" WHERE Id={id}").Rows;

            if (rows.Count == 0) // Not found
                return null;

            return new StudyGroup
            {
                Id = id,
                Name = rows[0]["Name"].ToString()
            };
        }

        public override Lesson GetLesson(uint id)
        {
            var rows = Select(Properties.Resources.SelectLesson + $" WHERE Id={id}").Rows;

            if (rows.Count == 0) // Not found
                return null;

            return new Lesson
            {
                Id = id,
                Name = rows[0]["Name"].ToString()
            };
        }

        public override Classroom GetClassroom(uint id)
        {
            var rows = Select(Properties.Resources.SelectClassroom + $" WHERE Id={id}").Rows;

            if (rows.Count == 0) // Not found
                return null;

            return new Classroom
            {
                Id = id,
                Name = rows[0]["Name"].ToString()
            };
        }

        public override Teacher GetTeacher(uint id)
        {
            var rows = Select(Properties.Resources.SelectTeacher + $" WHERE Id={id}").Rows;

            if (rows.Count == 0) // Not found
                return null;

            return new Teacher
            {
                Id = id,
                Name = rows[0]["Name"].ToString()
            };
        }


        private uint Insert(string query, params object[] args)
        {
            using (var guard = new ConnectionGuard(Connection))
            {
                var command = Connection.CreateCommand();
                command.CommandText = String.Format(query, args);
                command.ExecuteNonQuery();

                command.CommandText = "select last_insert_rowid()";

                var id = (uint)(long)command.ExecuteScalar();

                return id;
            }
        }

        public override uint Insert(LessonTime item)
        {
            return Insert(Properties.Resources.InsertLessonTime, item.Start, item.End);
        }

        public override uint Insert(StudyGroup item)
        {
            return Insert(Properties.Resources.InsertStudyGroup, item.Name);
        }

        public override uint Insert(Lesson item)
        {
            return Insert(Properties.Resources.InsertLesson, item.Name);
        }

        public override uint Insert(Classroom item)
        {
            return Insert(Properties.Resources.InsertClassroom, item.Name);
        }

        public override uint Insert(Teacher item)
        {
            return Insert(Properties.Resources.InsertTeacher, item.Name);
        }

        public override uint Insert(Schedule item)
        {
            return Insert(Properties.Resources.InsertSchedule,
                Convert.ToInt32(item.DayOfWeek), item.LessonTime.Id, item.StudyGroup.Id, item.Lesson.Id, item.Classroom.Id, item.Teacher.Id, item.AdditionalInfo);
        }

        public override uint Insert(Change item)
        {
            return Insert(Properties.Resources.InsertChange,
                item.Date.Date, item.LessonTime.Id, item.StudyGroup.Id, item.Lesson.Id, item.Classroom.Id, item.Teacher.Id, item.AdditionalInfo);
        }


        private bool Delete(string query, params object[] args)
        {
            using (var guard = new ConnectionGuard(Connection))
            {
                var command = Connection.CreateCommand();
                command.CommandText = string.Format(query, args);

                var count = command.ExecuteNonQuery();

                return count != 0;
            }            
        }

        public override bool Delete(Schedule item)
        {
            return Delete(Properties.Resources.DeleteSchedule, item.Id);
        }

        public override bool Delete(Change item)
        {
            return Delete(Properties.Resources.DeleteChange, item.Id);
        }


        private bool Update(string query, params object[] args)
        {
            using (var guard = new ConnectionGuard(Connection))
            {
                var command = Connection.CreateCommand();
                command.CommandText = string.Format(query, args);

                var count = command.ExecuteNonQuery();

                return count != 0;
            }
        }

        public override bool Update(Schedule item)
        {
            return Update(Properties.Resources.UpdateSchedule,
                Convert.ToInt32(item.DayOfWeek), item.LessonTime.Id, item.StudyGroup.Id, item.Lesson.Id, item.Classroom.Id, item.Teacher.Id, item.AdditionalInfo, item.Id);
        }

        public override bool Update(Change item)
        {
            return Update(Properties.Resources.UpdateChange,
                item.Date, item.LessonTime.Id, item.StudyGroup.Id, item.Lesson.Id, item.Classroom.Id, item.Teacher.Id, item.AdditionalInfo, item.Id);
        }
    }
}
