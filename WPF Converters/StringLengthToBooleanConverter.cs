using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    /// <summary>Converts a string value to a Boolean indicating whether the string is not null or empty. Intended for use in
    /// data binding scenarios where a Boolean representation of text presence is required.</summary>
    /// <remarks>This converter is commonly used in WPF or XAML-based applications to enable or disable UI
    /// elements based on whether a bound text value is present. The ConvertBack method is not implemented and will
    /// throw a NotImplementedException if called.</remarks>
    public class StringLengthToBooleanConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            return value is string s && s.Length > 0;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}