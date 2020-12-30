using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    [ValueConversion( typeof(DateTime), typeof(string), ParameterType = typeof(string) )]
    public class StringFormatConverter : IValueConverter {


        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            return String.Format( parameter?.ToString() ?? "", value.ToString() );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}