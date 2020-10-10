namespace TimeTable.Utility
{
    public interface ICategoryFilter
    {
        string StudyGroupName { get; }
        string TeacherName { get; }
        string ClassroomName { get; }
    }
}
