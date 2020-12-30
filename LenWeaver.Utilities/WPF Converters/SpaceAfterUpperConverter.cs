using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    public class PascalCaseToDisplayStringConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            string  valueAsString   = value?.ToString() ?? "";


            return StringHelpers.PascalCaseToDisplayString( valueAsString );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException();
        }
    }
}
