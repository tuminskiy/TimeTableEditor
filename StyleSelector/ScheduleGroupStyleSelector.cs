using System.Windows;
using System.Windows.Data;

namespace TimeTable.StyleSelector
{
    public class ScheduleGroupStyleSelector : System.Windows.Controls.StyleSelector
    {
        public Style StudyGroupStyle { get; set; }
        public Style DayOfWeekStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var group = item as CollectionViewGroup;

            if (group.IsBottomLevel)
                return DayOfWeekStyle;
            else
                return StudyGroupStyle;
        }
    }
    
}
