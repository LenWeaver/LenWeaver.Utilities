using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public static class StringExtensions {

        public static string ToDisplayString( this string pascalCase ) {

            Char                c;

            StringBuilder       sb      = new StringBuilder();


            for( int index = 0; index < pascalCase.Length; index++ ) {
                c   = pascalCase[index];

                if( Char.IsUpper( c ) ) {
                    sb.Append( ' ' );
                }

                sb.Append( c );
            }

            return sb.ToString().Trim();
        }
    }
}