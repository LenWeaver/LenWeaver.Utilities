using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LenWeaver.Utilities {

    /// <summary>
    /// Provides a value converter that converts 32-bit integer values to Boolean values for use in data binding
    /// scenarios.
    /// </summary>
    /// <remarks>This converter interprets any nonzero integer as <see langword="true"/>, and zero as <see
    /// langword="false"/>. The converter does not support conversion from Boolean back to integer values; calling
    /// <c>ConvertBack</c> will throw a <see cref="NotImplementedException"/>.</remarks>
    [ValueConversion( typeof(int), typeof(bool) )]
    public class Int32ToBooleanConverter : IValueConverter {

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

            return (int)value != 0;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

            throw new NotImplementedException( nameof(ConvertBack) );
        }
    }
}