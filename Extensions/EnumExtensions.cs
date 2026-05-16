using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LenWeaver.Utilities {

    public static class EnumExtensions {

        extension( Enum e ) {

        }

        /// <summary>Determines whether the specified enumeration value is defined for its type or, for flags enumerations,
        /// contains only valid flag combinations.</summary>
        /// <remarks>For enumerations marked with the FlagsAttribute, the method returns true if the value
        /// contains only bits corresponding to defined flags, even if the combination itself is not explicitly defined.
        /// For non-flags enumerations, the method returns true only if the value is a named constant defined in the
        /// enumeration.</remarks>
        /// <typeparam name="TEnum">The enumeration type to validate. Must be a value type that derives from Enum.</typeparam>
        /// <param name="value">The enumeration value to validate.</param>
        /// <returns>true if the value is a defined member of the enumeration type, or if it is a valid combination of defined
        /// flags for a flags enumeration; otherwise, false.</returns>
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