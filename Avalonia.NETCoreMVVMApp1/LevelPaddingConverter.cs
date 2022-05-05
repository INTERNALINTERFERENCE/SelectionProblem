﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Avalonia.NETCoreMVVMApp1
{
    public class LevelPaddingConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value is int level )
            {
                return new Thickness( 10 * level + 10, 0, 5, 5 );
            }
            return new Thickness();
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}