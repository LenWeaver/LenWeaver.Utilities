using System;
using System.Threading;
using System.Windows;

namespace LenWeaver.Utilities {

    public static class DateTimeHelpers {

        private static string?  longDateFormat          = null;
        private static string?  longTimeFormat          = null;
        private static string?  shortDateFormat         = null;
        private static string?  shortTimeFormat         = null;

        public const string     ISO8601DateFormat       = "yyyyMMdd";
        public const string     ISO8601DateTimeFormat   = "yyyyMMdd HH:mm:ss";
        public const string     ISO8601TimeFormat       = "HH:mm:ss";


        static DateTimeHelpers() {}

        
        public static string LongDateFormat {
            get {
                if( longDateFormat == null ) {
                    longDateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern;
                }

                return longDateFormat;
            }
        }
        public static string LongTimeFormat {
            get {
                if( longTimeFormat == null ) {
                    longTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern;
                }

                return longTimeFormat;
            }
        }
        public static string ShortDateFormat {
            get {
                if( shortDateFormat == null ) {
                    shortDateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }

                return shortDateFormat;
            }
        }
        public static string ShortTimeFormat {
            get {
                if( shortTimeFormat == null ) {
                    shortTimeFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;
                }

                return shortTimeFormat;
            }
        }

        public static void GetDateRangeYear( int year, out DateTime start, out DateTime end ) {

            start   = new DateTime( year,  1,  1,  0,  0,  0,   0 );
            end     = new DateTime( year, 12, 31, 23, 59, 59, 998 );
        }
        public static void GetDateRangeYear( DateTime date, out DateTime start, out DateTime end ) {

            GetDateRangeYear( date.Year, out start, out end );
        }
        public static void GetDateRangeMonth( DateTime date, out DateTime start, out DateTime end ) {

            start   = new DateTime( date.Year, date.Month, 1, 0, 0, 0, 0 );
            end     = start.AddMonths( 1 ).AddMilliseconds( -2 );
        }
        public static void GetDateRangeWeek( DateTime date, out DateTime start, out DateTime end ) {

            start   = date.Date.AddDays( -(int)date.DayOfWeek );
            end     = start.AddDays( 7 ).AddMilliseconds( -2 );
        }

        public static string TimeSpanToString( TimeSpan ts ) {

            string      result;


            if( ts.TotalMilliseconds < 1000d ) {
                result  = $"{(long)(ts.TotalMilliseconds / 1000d)} ms";
            }
            else if( ts.TotalSeconds < 60d ) {
                result  = $"{(long)(ts.TotalSeconds / 60d)} s";
            }
            else if( ts.TotalMinutes < 60d ) {
                result  = $"{(long)(ts.TotalMinutes / 60d)} m";
            }
            else if( ts.TotalHours < 24d ) {
                result  = $"{(long)(ts.TotalHours / 24d)} h";
            }
            else if( ts.TotalDays < 365d ) {
                result  = $"{(long)(ts.TotalDays / 365d)} days";
            }
            else {
                result  = $"{(long)(ts.TotalDays / 365d)} years";
            }

            return result;
        }

        public static (DateTime start, DateTime end) GetDateRangeYear( DateTime date ) {

            DateTime        end;
            DateTime        start;


            GetDateRangeYear( date, out start, out end );

            return (start, end);
        }
        public static (DateTime start, DateTime end) GetDateRangeMonth( DateTime date ) {

            DateTime        end;
            DateTime        start;


            GetDateRangeMonth( date, out start, out end );

            return (start, end);
        }
        public static (DateTime start, DateTime end) GetDateRangeWeek( DateTime date ) {

            DateTime        end;
            DateTime        start;


            GetDateRangeWeek( date, out start, out end );

            return (start, end);
        }
    }
}