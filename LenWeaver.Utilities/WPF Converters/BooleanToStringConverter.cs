using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    /// <summary>
    /// Converts a boolean value into one of two possible strings.
    /// </summary>
    public class BooleanToStringConverter : IValueConverter {

        public BooleanToStringConverter() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">The Boolean value to convert to a string.</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">A comma delimited list of possible return values.  First token will be
        /// returned if 'value' is true, otherwise the second token will be returned.  Default is 'True,False'</param>
        /// <param name="culture">Ignored</param>
        /// <returns></returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            bool        asBoolean   = Boolean.Parse( value?.ToString() ?? Boolean.FalseString );

            string[]    tokens      = (parameter?.ToString() ?? "True,False").Split( ',' ) ;


            if( tokens.Length != 2 ) throw new ArgumentException( "Argument 'parameter' must be a comma delimited string containing " +
                                                                  "two tokens." );

            return asBoolean ? tokens[0] : tokens[1];
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            throw new NotImplementedException();
        }
    }
}