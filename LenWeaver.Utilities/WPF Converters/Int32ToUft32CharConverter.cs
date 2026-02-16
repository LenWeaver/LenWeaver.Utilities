using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    public class Int32ToUft32CharConverter : IValueConverter {


        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            return Char.ConvertFromUtf32( System.Convert.ToInt32( value ) );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}