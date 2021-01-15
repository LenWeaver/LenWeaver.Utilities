using System;
using System.IO;
using System.Reflection;

namespace LenWeaver.Utilities {

    public static class Helpers {

        static Helpers() {}


        public static T         Max<T>( T a, T b ) where T : IComparable<T> {

            return a.CompareTo( b ) > 0 ? a : b;
        }
        public static T         Max<T>( params T[] items ) where T : IComparable<T> {

            T       result;


            if( items == null || items.Length == 0 ) {
                throw new ArgumentException( "items parameter must not be null and must contain at least one item." );
            }

            result = items[0];

            for( int index = 1; index < items.Length; index++ ) {
                if( result.CompareTo( items[index] ) < 0 ) {
                    result = items[index];
                }
            }

            return result;
        }
        public static T         Min<T>( T a, T b ) where T : IComparable<T> {

            return a.CompareTo( b ) < 0 ? a : b;
        }
        public static T         Min<T>( params T[] items ) where T : IComparable<T> {

            T       result;


            if( items == null || items.Length == 0 ) {
                throw new ArgumentException( "items parameter must not be null and must contain at least one item." );
            }

            result = items[0];

            for( int index = 1; index < items.Length; index++ ) {
                if( result.CompareTo( items[index] ) > 0 ) {
                    result = items[index];
                }
            }

            return result;
        }
        public static T         RangeCheck<T>( T value, T lower, T upper ) where T : IComparable<T> {

            T       result  = value;


            if( lower.CompareTo( upper ) > 0 ) throw new ArgumentException( "The 'lower' parameter is greater than the 'upper' parameter." );

            if( value.CompareTo( lower ) < 0 ) result = lower;
            if( value.CompareTo( upper ) > 0 ) result = upper;

            return result;
        }

        public static T?        FirstNonNull<T>( params T[] items ) where T : class {

            T?   result  = null;


            for( int i = 0; i < items.Length; i++ ) {
                if( items[i] != null ) {
                    result = items[i];
                    break;
                }
            }

            return result;
        }
        public static T?        FirstNonNull<T>( params T?[] items ) where T : struct {

            T?  result  = null;


            for( int i = 0; i < items.Length; i++ ) {
                if( items[i] != null ) {
                    result = items[i];
                    break;
                }
            }

            return result;
        }

        public static string    FileSizeToString( long fileSize ) {

            return FileSizeToString( (double)fileSize );
        }
        public static string    FileSizeToString( double fileSize ) {

            string      result;


            if( fileSize < 1024d ) {
                result = $"{fileSize} B";
            }
            else if( fileSize < 1_000_000d ) {
                result = $"{fileSize / 1_024d:N1} KB";
            }
            else if( fileSize < 1_000_000_000d ) {
                result = $"{fileSize / 1_000_000d:N1} MB";
            }
            else if( fileSize < 1_000_000_000_000d ) {
                result = $"{fileSize / 1_000_000_000d:N1} GB";
            }
            else {
                result = $"{fileSize / 1_000_000_000_000d:N1} TB";
            }

            return result;
        }
        public static string    GetSettingsFilename( string extension ) {

            string                  assemblyFilename;
            string                  result;

            Assembly?               entryAssembly;


            entryAssembly           = Assembly.GetEntryAssembly();

            assemblyFilename        = entryAssembly?.Location ?? String.Empty;

            if( !String.IsNullOrEmpty( assemblyFilename ) ) {
                result              = $"{Path.GetDirectoryName( assemblyFilename )}" +
                                      $"{Path.DirectorySeparatorChar}" +
                                      $"{Path.GetFileNameWithoutExtension( assemblyFilename )}" +
                                      $"{extension}";
            }
            else {
                throw new InvalidOperationException( "Unable to determine entry assembly filename." );
            }

            return result;
        }
    }
}