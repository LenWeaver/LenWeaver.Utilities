using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    /// <summary>
    /// Converts Boolean or nullable Boolean values to a corresponding TextWrapping value for use in WPF data binding
    /// scenarios.
    /// </summary>
    /// <remarks>This converter is used to convert a boolean, or nullable boolean to a TextWrapping value.  A value
    /// of false will return a NoWrap result.  By default a value of true will return Wrap.  Passing a parameter
    /// of the string "WrapWithOverflow". The ConvertBack method is not implemented and will throw a
    /// NotImplementedException if called.</remarks>
    [ValueConversion( typeof(bool),  typeof(TextWrapping), ParameterType = typeof(string) )]
    [ValueConversion( typeof(bool?), typeof(TextWrapping), ParameterType = typeof(string) )]
    public class BooleanToTextWrappingConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            bool?           bln     = (bool?)value;

            string          str     = (string?)parameter ?? String.Empty;

            TextWrapping    result  = TextWrapping.Wrap;


            if( bln ?? false ) {
                if( String.Compare( str, nameof(TextWrapping.WrapWithOverflow), ignoreCase: true ) == 0 ) {
                    result = TextWrapping.WrapWithOverflow;
                }
                else {
                    result = TextWrapping.Wrap;
                }
            }
            else {
                result = TextWrapping.NoWrap;
            }

            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}