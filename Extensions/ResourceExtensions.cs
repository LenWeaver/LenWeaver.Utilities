using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LenWeaver.Utilities {

    public static class ResourceExtensions {

        static ResourceExtensions() {}


        public static Style? FindStyle( this ResourceDictionary rd, string styleKey ) {

            Style?      result      = null;


            foreach( object key in rd.Keys ) {
                if( String.Compare( key.ToString(), styleKey, ignoreCase: true ) == 0 ) {
                    if( rd[key] is Style ) {
                        result = (Style)rd[key];

                        break;
                    }
                }

                if( rd[key] is ResourceDictionary resDict ) {
                    result = resDict.FindStyle( styleKey );

                    if( result is not null ) break;
                }
            }

            if( result is null ) {
                foreach( ResourceDictionary dict in rd.MergedDictionaries ) {
                    result = dict.FindStyle( styleKey );

                    if( result is not null ) break;
                }
            }

            return result;
        }

        public static string ToExtendedPathMarkup( this IEnumerable<GeometryDrawingDescriptor> descriptors ) {

            return String.Join( '@', descriptors );
        }
    }
}