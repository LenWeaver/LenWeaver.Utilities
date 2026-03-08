using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    [ValueConversion( typeof(double), typeof(double), ParameterType = typeof(string) )]
    public class DoubleMathConverter : IValueConverter {

        /// <summary>
        /// Performs simple mathematical operations and returns a result.
        /// </summary>
        /// <param name="value">The value to be manipulated.</param>
        /// <param name="targetType">Ignored</param>
        /// <param name="parameter">Comma delimited list of parameters.  First token should indicate
        /// the mathematical operation to be performed and should be one of the following:  +,
        /// -, * or /.  The second parameter should a value.</param>
        /// <param name="culture">Ignored</param>
        /// <returns>The result as a System.Double.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            double      asDouble    = System.Convert.ToDouble( value );
            double      other;
            double      result      = Double.NaN;

            string[]    tokens;


            if( value != null && parameter != null ) {
                tokens = (parameter.ToString() ?? ",").Split( ',' );
                if( tokens.Length == 2 ) {
                    other = Double.Parse( tokens[1] );
                    switch( tokens[0] ) {
                        case "+":   result = asDouble + other; break;
                        case "-":   result = asDouble - other; break;
                        case "*":   result = asDouble * other; break;
                        case "/" when asDouble != 0 && other != 0: result = asDouble / other; break;
                        default:
                            if( tokens.Length == 3 ) {
                                result = Double.Parse( tokens[2] );
                            }
                            else {
                                throw new ArgumentException( "Unknown arithmetic operation or divide by zero." );
                            }

                            break;
                    }
                }
            }

            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}