using System;

namespace TimeTable.Entity
{
    public class LessonTime : AbstractEntity
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public override string ToString()
        {
            return String.Format("{0:hh\\:mm} - {1:hh\\:mm}", Start, End);
        }

        public static LessonTime Parse(string str)
        {
            var times = str.Split('-');

            return new LessonTime
            {
                Start = TimeSpan.Parse(times[0]),
                End = TimeSpan.Parse(times[1])
            };
        }

        public static bool TryParse(string str, out LessonTime obj)
        {
            try
            {
                obj = Parse(str);
            }
            catch (Exception)
            {
                obj = null;
                return false;
            }

            return true;
        }
    }
}
