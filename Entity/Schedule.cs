using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeTable.Entity
{
    public class Schedule : AbstractEntity, Utility.ICategoryFilter, INotifyPropertyChanged
    {
        private DayOfWeek dayOfWeek;
        private LessonTime lessonTime;
        private StudyGroup studyGroup;
        private Lesson lesson;
        private Classroom classroom;
        private Teacher teacher;
        private string additionalInfo;

        public DayOfWeek DayOfWeek 
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; OnPropertyChanged(); }
        }

        public LessonTime LessonTime
        {
            get { return lessonTime; }
            set { lessonTime = value; OnPropertyChanged(); }
        }

        public StudyGroup StudyGroup
        {
            get { return studyGroup; }
            set { studyGroup = value; OnPropertyChanged(); }
        }

        public Lesson Lesson
        {
            get { return lesson; }
            set { lesson = value; OnPropertyChanged(); }
        }

        public Classroom Classroom
        {
            get { return classroom; }
            set { classroom = value; OnPropertyChanged(); }
        }

        public Teacher Teacher
        {
            get { return teacher; }
            set { teacher = value; OnPropertyChanged(); }
        }

        public string AdditionalInfo
        {
            get { return additionalInfo; }
            set { additionalInfo = value; OnPropertyChanged(); }
        }

        public string StudyGroupName { get { return StudyGroup.Name; } }
        public string ClassroomName { get { return Classroom.Name; } }
        public string TeacherName { get { return Teacher.Name; } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
