using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Avalonia.NETCoreMVVMApp1
{
    public class TreeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var success = int.TryParse(value.ToString(), out var count);

            if (!success)
            {
                return false;
            }


            return count switch
            {
                > 1 => false,
                < 1 => true,
                _ => true
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}