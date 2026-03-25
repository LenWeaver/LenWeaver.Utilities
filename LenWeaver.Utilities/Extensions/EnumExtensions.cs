using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LenWeaver.Utilities {

    public static class EnumExtensions {

        extension( Enum e ) {

        }

        public static bool IsValid<TEnum>( this TEnum value ) where TEnum : struct, Enum {

            bool        result;

            ulong       bits    = 0;
            ulong       raw;

            Type        type;


            type                = typeof(TEnum);

            if( type.IsDefined( typeof(FlagsAttribute), false ) ) {
                foreach( object v in Enum.GetValues( type ) ) {
                    bits        |= Convert.ToUInt64( v );
                }

                raw             = Convert.ToUInt64( value );
                result          = (raw & ~bits) == 0;
            }
            else {
                result          = Enum.IsDefined( type, value );
            }

            return result;
        }
        public static string ToDisplayString<TEnum>( this TEnum value ) where TEnum : struct, Enum {

            return value.ToString().ToDisplayString();
        }
    }
}