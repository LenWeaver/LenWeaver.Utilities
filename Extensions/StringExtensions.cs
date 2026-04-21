using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public static class StringExtensions {

        public const string AllLowerAlpha   = "abcdefghijklmnopqrstuvwxyz";
        public const string AllUpperAlpha   = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string AllNumeric      = "0123456789";
        public const string AllPunctuation  = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
        public const string AllWhiteSpace   = " \t\n\r\v\f";

        public const string AllAlpha        = AllLowerAlpha + AllUpperAlpha;
        public const string AllAlphaNumeric = AllAlpha + AllNumeric;


        public static string KeepOnly( this string s, string toKeep ) {

            HashSet<char>   keepers;
            StringBuilder   sb;


            if( s is null )                         return null!;
            if( String.IsNullOrEmpty( toKeep ) )    return String.Empty;

            keepers = new HashSet<char>( toKeep );
            sb      = new StringBuilder( s.Length );

            foreach( char c in s ) {
                if( keepers.Contains( c ) ) sb.Append( c );
            }

            return sb.ToString();
        }
        public static string RemoveAny( this string s, string toRemove ) {

            HashSet<char>   losers;
            StringBuilder   sb;


            if( s is null )                         return null!;
            if( String.IsNullOrEmpty( toRemove ) )  return String.Empty;

            losers = new HashSet<char>( toRemove );
            sb      = new StringBuilder( s.Length );

            foreach( char c in s ) {
                if( !losers.Contains( c ) ) sb.Append( c );
            }

            return sb.ToString();
        }

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

        /// <summary>Removes matching leading and trailing quote characters from the specified string, if present.</summary>
        /// <remarks>If the input string is less than two characters long or does not have matching quote
        /// characters at both ends, the original string is returned unchanged.</remarks>
        /// <param name="s">The string to remove quotes from. May be enclosed in double quotes (", e.g., "value"), square brackets
        /// ([value]), or backticks (`value`) or apostrophes ('value').</param>
        /// <returns>A string with matching leading and trailing quotes removed if present; otherwise, the original string.</returns>
        public static string Unquote( this string s ) {

            if( s.Length >= 2 ) {
                if( ( s.StartsWith( '"' ) && s.EndsWith( '"' ) ) ||
                    ( s.StartsWith( '[' ) && s.EndsWith( ']' ) ) ||
                    ( s.StartsWith( '`' ) && s.EndsWith( '`' ) ) ||
                    ( s.StartsWith( '\'' ) && s.EndsWith( '\'' ) ) ) {

                    return s.Substring( 1, s.Length - 2 );
                }
            }

            return s;
        }
    }
}