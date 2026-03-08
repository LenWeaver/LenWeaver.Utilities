using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    public class NullOrEmptyToVisibilityConverter : IMultiValueConverter {

        public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture ) {

            bool        canClear;

            string      text;


            if( values is null )        throw new ArgumentNullException();
            if( values.Length != 2 )    throw new ArgumentException( $"{nameof(values)} array parameter must have two elements." );


            canClear    = (bool)values[1];
            text        = values[0] as string ?? String.Empty;

            return canClear && text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}