using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    [ValueConversion( typeof(bool), typeof(Visibility) )]
    [ValueConversion( typeof(bool?), typeof(Visibility) )]
    public class BooleanToVisibilityConverter : IValueConverter {

        public BooleanToVisibilityConverter() {}

        /// <summary>
        /// parameter should either be 'Hidden' or 'Collapse' and is used to define the
        /// the return value when the value parameter is false.  The default is 'Hidden'.
        /// </summary>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            
            bool            asBool;

            string          asString;

            Visibility      result;


            if( targetType == typeof(bool) ) {
                asBool = (bool)value;
            }
            else if( targetType == typeof(bool?) ) {
                asBool = ((bool?)value) ?? false;
            }
            else {
                throw new ArgumentException( "value parameter must either be a boolean or a nullable boolean." );
            }

            if( asBool ) {
                result  = Visibility.Visible;
            }
            else {
                if( parameter != null ) {
                    if( String.Compare( parameter.ToString(), "Hidden", ignoreCase: true ) == 0 ) {
                        asString    = "Hidden";
                    }
                    else {
                        asString    = "Collapse";
                    }
                }
                else {
                    asString = "Hidden";
                }

                result = String.Compare( asString, "Hidden" ) == 0 ? Visibility.Hidden : Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}