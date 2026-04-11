using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace LenWeaver.Utilities {


    public class ToStringConverter : IValueConverter {


        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            

            return value?.ToString() ?? (parameter?.ToString() ?? "");
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}