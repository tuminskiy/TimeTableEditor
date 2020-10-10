using System;
using System.Globalization;
using System.Windows.Data;

namespace TimeTable.Converter
{
    class WpfDayOfWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return culture.DateTimeFormat.GetDayName((DayOfWeek)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
