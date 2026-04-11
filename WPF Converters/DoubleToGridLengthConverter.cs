using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    public class DoubleToGridLengthConverter : IValueConverter {


        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            return new GridLength( System.Convert.ToDouble( value ) );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}