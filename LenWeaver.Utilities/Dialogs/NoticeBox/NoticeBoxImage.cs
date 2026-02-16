using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace LenWeaver.Utilities {

    public enum NoticeBoxImage {
        None,
        Error,
        Stop,
        Question,
        Exclamation,
        Warning,
        Information
    }

    public static class NoticeBoxImageHelpers {


        static NoticeBoxImageHelpers() {}


        public static bool IsDefined( int of ) {

            return of == (int)NoticeBoxImage.None ||
                   of == (int)NoticeBoxImage.Error ||
                   of == (int)NoticeBoxImage.Stop ||
                   of == (int)NoticeBoxImage.Question ||
                   of == (int)NoticeBoxImage.Exclamation ||
                   of == (int)NoticeBoxImage.Warning ||
                   of == (int)NoticeBoxImage.Information;
        }
        public static bool IsDefined( NoticeBoxImage of ) {

            return IsDefined( (int)of );
        }

        public static NoticeBoxImage Parse( int of ) {

            NoticeBoxImage        result;


            if( IsDefined( of ) ) {
                result = (NoticeBoxImage)of;
            }
            else {
                throw new ArgumentException( $"Unknown value for integer {nameof(NoticeBoxImage)}." );
            }

            return result;
        }
        public static NoticeBoxImage Parse( string of ) {

            NoticeBoxImage        result;


            switch( of.Trim().ToUpper() ) {
                case "NONE":            result = NoticeBoxImage.None;           break;
                case "ERROR":           result = NoticeBoxImage.Error;          break;
                case "STOP":            result = NoticeBoxImage.Stop;           break;
                case "QUESTION":        result = NoticeBoxImage.Question;       break;
                case "EXCLAMATION":     result = NoticeBoxImage.Exclamation;    break;
                case "WARNING":         result = NoticeBoxImage.Warning;        break;
                case "INFORMATION":     result = NoticeBoxImage.Information;    break;

                default:
                    throw new ArgumentException( $"Unknown value for string {nameof(NoticeBoxImage)}." );
            }

            return result;
        }

        public static string ToDisplayString( NoticeBoxImage of ) {

            string      result;


            switch( of ) {
                case NoticeBoxImage.None:           result = "None";            break;
                case NoticeBoxImage.Error:          result = "Error";           break;
                case NoticeBoxImage.Stop:           result = "Stop";            break;
                case NoticeBoxImage.Question:       result = "Question";        break;
                case NoticeBoxImage.Exclamation:    result = "Exclamation";     break;
                case NoticeBoxImage.Warning:        result = "Warning";         break;
                case NoticeBoxImage.Information:    result = "Information";     break;

                default:
                    throw new ArgumentOutOfRangeException( $"{nameof(NoticeBoxImage)} parameter contains an invalid value." );
            }

            return result;
        }
        public static string ToString( NoticeBoxImage of ) {

            string      result;


            switch( of ) {
                case NoticeBoxImage.None:           result = "None";            break;
                case NoticeBoxImage.Error:          result = "Error";           break;
                case NoticeBoxImage.Stop:           result = "Stop";            break;
                case NoticeBoxImage.Question:       result = "Question";        break;
                case NoticeBoxImage.Exclamation:    result = "Exclamation";     break;
                case NoticeBoxImage.Warning:        result = "Warning";         break;
                case NoticeBoxImage.Information:    result = "Information";     break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        public static IEnumerable<NoticeBoxImage> ForEach() {

            yield return NoticeBoxImage.None;
            yield return NoticeBoxImage.Error;
            yield return NoticeBoxImage.Stop;
            yield return NoticeBoxImage.Question;
            yield return NoticeBoxImage.Exclamation;
            yield return NoticeBoxImage.Warning;
            yield return NoticeBoxImage.Information;
        }
    }
}