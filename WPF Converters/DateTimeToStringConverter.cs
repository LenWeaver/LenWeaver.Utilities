using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    [ValueConversion( typeof(DateTime), typeof(string), ParameterType = typeof(string) )]
    public class DateTimeToStringConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            DateTime    valueAsDateTime;

            string?     paramAsString;
            string      result;


            if( value != null ) {
                paramAsString   = parameter != null ? parameter.ToString() : "d";

                if( paramAsString!.Contains( "ShortDate", StringComparison.OrdinalIgnoreCase ) ) {
                    paramAsString = paramAsString.Replace( "ShortDate", DateTimeHelpers.ShortDateFormat );
                }

                if( paramAsString!.Contains( "LongDate", StringComparison.OrdinalIgnoreCase ) ) {
                    paramAsString = paramAsString.Replace( "LongDate", DateTimeHelpers.LongDateFormat );
                }

                if( paramAsString!.Contains( "ShortTime", StringComparison.OrdinalIgnoreCase ) ) {
                    paramAsString = paramAsString.Replace( "ShortTime", DateTimeHelpers.ShortTimeFormat );
                }

                if( paramAsString!.Contains( "LongTime", StringComparison.OrdinalIgnoreCase ) ) {
                    paramAsString = paramAsString.Replace( "LongTime", DateTimeHelpers.LongTimeFormat );
                }

                valueAsDateTime = (DateTime)value;
                result          = valueAsDateTime.ToString( paramAsString );
            }
            else {
                result          = String.Empty;
            }

            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}