using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LenWeaver.Utilities {

    public static class StringExtensions {

        public const string     AllLowerAlpha       = "abcdefghijklmnopqrstuvwxyz";
        public const string     AllUpperAlpha       = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string     AllNumeric          = "0123456789";
        public const string     AllPunctuation      = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
        public const string     AllWhiteSpace       = " \t\n\r\v\f";

        public const string     AllAlpha            = AllLowerAlpha + AllUpperAlpha;
        public const string     AllAlphaNumeric     = AllAlpha + AllNumeric;


        public static bool      IsAlphaOnly         ( this string s ) {

            bool    result  = true;


            foreach( char c in s ) {
                if( !Char.IsLetter( c ) ) {
                    result = false;
                    break;
                }
            }

            return result;
        }
        public static bool      ContainsOnly        ( this string s, string permittedCharacters ) {

            HashSet<char>       permitted;


            if( s is null )                                     return false;
            if( String.IsNullOrEmpty( s ) )                     return true;
            if( String.IsNullOrEmpty( permittedCharacters ) )   return false;

            permitted = new HashSet<char>( permittedCharacters );

            foreach( char c in s ) {
                if( !permitted.Contains( c ) ) {
                    return false;
                }
            }

            return true;
        }

        public static string    ComputeHash         ( this string input, string salt ) {


            ArgumentException.ThrowIfNullOrWhiteSpace( input );

            byte[] bytes    = SHA256.HashData( Encoding.UTF8.GetBytes( input + salt ) );

            return Convert.ToHexString( bytes );
        }
        public static string    ComputeHash         ( this string input ) => ComputeHash( input, "" );

        /// <summary>Increments a string according to the specified mask, supporting alphanumeric and numeric patterns with
        /// carry-over logic.</summary>
        /// <remarks>Wildcard characters in the mask determine the increment behavior: '@' increments A-Z,
        /// '!' increments 0-9 then A-Z, and '#' increments 0-9. Carry-over is handled according to the mask
        /// pattern.</remarks>
        /// <param name="s">The string to increment. Must be empty or the same length as the mask.</param>
        /// <param name="mask">A mask string that defines the increment pattern. Must contain at least one wildcard character: '@' for
        /// uppercase letters, '!' for alphanumeric, or '#' for digits.</param>
        /// <returns>A new string representing the incremented value based on the mask. If the input string is empty or
        /// whitespace, returns the initial value as defined by the mask.</returns>
        /// <exception cref="ArgumentException">Thrown if mask is null, empty, or contains no wildcard characters; or if s is not empty and its length does
        /// not match the mask.</exception>
        public static string    Increment           ( this string s, string mask ) {

            bool        carry           = false;

            char        c;
            char[]      chars;

            string      result;


            ArgumentException.ThrowIfNullOrWhiteSpace( mask );

            if( mask.IndexOfAny( ['@', '!', '#'] ) == -1 ) throw new ArgumentException( "Specified mask contains no wildcard characters." );
            if( s.Length != mask.Length && s.Length != 0 ) throw ExceptionBuilder.Create<ArgumentException>( "The string being acted upon must either " +
                                                                                 "be empty or the same length as the mask parameter." )
                                                                                 .AddData( "s",         s )
                                                                                 .AddData( "mask",   mask );

            if( String.IsNullOrWhiteSpace( s ) ) {
                //This is the first use of .Increment.  Create a zero state result.

                result      = mask.Replace( '@', 'A' )      //Alpha characters - All uppercase.
                                  .Replace( '!', '0' )      //AlphaNumeric characters.
                                  .Replace( '#', '0' );     //Numeric characters.
            }
            else {
                chars       = s.ToCharArray();

                for( int i = chars.Length - 1; i >= 0; i-- ) {
                    carry   = false;
                    c       = chars[i];

                    switch( mask[i] ) {
                        case '!':
                            if( c >= '0' && c <= '8' ) c++;
                            else if( c == '9' ) c = 'A';
                            else if( c >= 'A' && c <= 'Y' ) c++;
                            else if( c == 'Z' ) {
                                c = '0';
                                carry = true;
                            }
                            break;

                        case '@':
                            if( c < 'Z' ) {
                                c++;
                            }
                            else {
                                c       = 'A';
                                carry   = true;
                            }
                            break;

                        case '#':
                            if( c < '9' ) {
                                c++;
                            }
                            else {
                                c       = '0';
                                carry   = true;
                            }
                            break;

                        default:
                            carry       = true;
                            break;
                    }

                    chars[i] = c;
                    if( !carry ) break;
                }

                result = new String( chars );
            }

            return result;
        }
        public static string    KeepOnly            ( this string s, string toKeep ) {

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
        public static string    RemoveAny           ( this string s, string toRemove ) {

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
        public static string    ToDisplayString     ( this string pascalCase ) {

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
        public static string    Unquote             ( this string s ) {

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