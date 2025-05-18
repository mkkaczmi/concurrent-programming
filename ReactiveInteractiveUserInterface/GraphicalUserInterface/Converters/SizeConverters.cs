using System;
using System.Globalization;
using System.Windows.Data;

namespace TP.ConcurrentProgramming.GraphicalUserInterface.Converters
{
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                // Leave some margin for the border and other UI elements
                return Math.Max(100, height - 100);
            }
            return 400; // Default height
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width)
            {
                // Leave some margin for the border
                return Math.Max(100, width - 40);
            }
            return 400; // Default width
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 