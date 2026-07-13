using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public static class EnumerableExtensions {

        public static object? FirstOrNull( this IEnumerable list ) {

            object?     result      = null;


            foreach( object o in list ) {
                result = o;
                break;
            }

            return result;
        }
        public static IEnumerable<T> RemoveIf<T>( this IEnumerable<T> list, Predicate<T> remove ) {

            List<T>     result      = new();


            foreach( T t in list ) {
                if( !remove( t ) ) {
                    result.Add( t );
                }
            }

            return result;
        }
    }
}