using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LenWeaver.Utilities {

    public static class StringHelpers {

        private static  readonly string[][]      englishDigitMatrix;


        static StringHelpers() {

            englishDigitMatrix =    new string[][] {
                                    new string[] { "Zero",      "Zeroth",   ""          },
                                    new string[] { "One",       "First",    ""          },
                                    new string[] { "Two",       "Second",   "Twenty"    },
                                    new string[] { "Three",     "Third",    "Thirty"    },
                                    new string[] { "Four",      "Fourth",   "Forty"     },
                                    new string[] { "Five",      "Fifth",    "Fifty"     },
                                    new string[] { "Six",       "Sixth",    "Sixty"     },
                                    new string[] { "Seven",     "Seventh",  "Seventy"   },
                                    new string[] { "Eight",     "Eighth",   "Eighty"    },
                                    new string[] { "Nine",      "Ninth",    "Ninety"    },
                                    new string[] { "Ten",       "",         ""          },
                                    new string[] { "Eleven",    "",         ""          },
                                    new string[] { "Twelve",    "",         ""          },
                                    new string[] { "Thirteen",  "",         ""          },
                                    new string[] { "Fourteen",  "",         ""          },
                                    new string[] { "Fifteen",   "",         ""          },
                                    new string[] { "Sixteen",   "",         ""          },
                                    new string[] { "Seventeen", "",         ""          },
                                    new string[] { "Eighteen",  "",         ""          },
                                    new string[] { "Nineteen",  "",         ""          } };
        }

        /// <summary>
        /// Searches target string for searchSpec.  searchOrigin determines the where the search
        /// will begin.  The search will start at target[searchOrigin] and proceed by checking
        /// one character before searchOrigin, one character after, two characters before, etc.
        /// </summary>
        /// <param name="target">The string to be searched.</param>
        /// <param name="searchOrigin">The zero-based location with target to begin the search.</param>
        /// <param name="searchSpec">The character to search for.</param>
        /// <returns></returns>
        public static int FindNear( string target, int searchOrigin, char searchSpec ) {

            int after       = searchOrigin;
            int before      = searchOrigin;
            int result      = -1;


            if( target[searchOrigin] == searchSpec ) {
                result = searchSpec;
            }
            else {
                while( before > 0 && after < target.Length && result == -1 ) {
                    if( target[before] == searchSpec ) {
                        result = before;
                    }
                    else {
                        before--;

                        if( target[after] == searchSpec ) {
                            result = after;
                        }
                        else {
                            after++;
                        }
                    }
                }
            }

            return result;
        }
        
        /// <summary>
        /// Inserts spaces before every upper case character.
        /// </summary>
        /// <param name="pascalCase">A string of text in PascalCase without spaces.</param>
        /// <returns>A version of pascalCase with a space inserted before each upper case character.</returns>
        public static string PascalCaseToDisplayString( string pascalCase ) {

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
        public static string ToDisplayString( int number, bool order ) {
        
            bool            single          = false;
            bool            teen            = false;
            bool            tens            = false;
            
            int             countPlace      = 0;
            int             sign            = 1;
            int             tempNumber      = number;
            
            string          returnString    = String.Empty;

            StringBuilder   sb              = new StringBuilder();

            
            if( number < 0 ) {
                sign *= -1;
                sb.Append( "negative" );
            }
            
            //count the billions (int max is over two billion)
            countPlace = (tempNumber / 1_000_000_000) * sign;
            if( countPlace > 0 ) {
                sb.Append( ToDisplayString( countPlace, false ) );
                sb.Append( " Billion " );
            }
            
            tempNumber -= (1_000_000_000 * countPlace) * sign;
            
            //count the millions
            countPlace = (tempNumber / 1_000_000) * sign;
            if( countPlace > 0 ) {
                sb.Append( ToDisplayString( countPlace, false ) );
                sb.Append( " Million " );
            }
            
            tempNumber -= (1_000_000 * countPlace) * sign;
            
            //count the thousands
            countPlace = (tempNumber / 1_000) * sign;
            if( countPlace > 0 ) {
                sb.Append( ToDisplayString( countPlace, false ) );
                sb.Append( " Thousand " );
            }
            
            tempNumber -= (1_000 * countPlace) * sign;
            
            //any recursion falls in here - in English, the main number
            //groupings are in the hundreds - hundreds, hundreds of thousands
            //hundreds of millions, etc.
            countPlace = (tempNumber / 100) * sign;
            if( countPlace > 0 ) {
                sb.Append( ToDisplayString( countPlace, false ) );
                sb.Append( " Hundred " );
            }
            
            tempNumber -= (100 * countPlace) * sign;

            //count the 10's places
            countPlace = (tempNumber / 10) * sign;
            if( countPlace > 0 ) {
                tens = true;
                if( countPlace == 1 ) {
                    teen = true;
                }
                else {
                    sb.Append( englishDigitMatrix[countPlace][2] );
                    sb.Append( ' ' );
                }
            }
            
            tempNumber -= (10 * countPlace) * sign;

            //when working with single digits, and also
            //teens, the rules change a bit.
            tempNumber *= sign; //for the singles, read positives
            if( tempNumber >= 0 ) {
                //catch if we have any single digits
                if( tempNumber == 0 ) {
                    single = false;
                }
                else {
                    single = true;
                }
                
                //catch the teens, and the number ten as well
                if( teen ) {
                    sb.Append( englishDigitMatrix[10 + tempNumber][0] );
                    //catch the position order
                    if( order ) {
                        sb.Append( "th" );
                    }
                }
                else if( tempNumber > 0 ) {
                    //catch the position order for single digits
                    if( order ) {
                        sb.Append( englishDigitMatrix[tempNumber][1] );
                    }
                    else {
                        sb.Append( englishDigitMatrix[tempNumber][0] );
                    }
                }
                else if( tempNumber == 0 && sb.Length == 0 ) {
                    //need to catch the solitary number 0
                    //nothing will have been caught before this,
                    //so returnString will be empty.
                    if( order ) {
                        sb.Append( englishDigitMatrix[tempNumber][1] );
                    }
                    else {
                        sb.Append( englishDigitMatrix[tempNumber][0] );
                    }                
                }
            }
            
            sb = new StringBuilder( sb.ToString().Trim() );
            
            //check if it ended on a signifier greater than or
            //equal to the hundreds - it won't have any order
            //qualifiers, we need to add them
            if( order ) {
                returnString = sb.ToString().Trim();

                if( returnString.EndsWith( "Billion" ) ||
                    returnString.EndsWith( "Million" ) ||
                    returnString.EndsWith( "Thousand" ) ||
                    returnString.EndsWith( "Hundred" ) ) {
                    returnString += "th";
                }
                else if( !single && !teen && tens ) {
                    //must be multiple of 10, greater than or equal to 20
                    //less than one hundred
                    returnString = returnString.Substring( 0, returnString.Length - 1 ) + "ieth";
                }
            }
            
            return returnString; 
        }
        public static string ToDisplayString( int number ) {

            return ToDisplayString( number, order: false );
        }
    }
}