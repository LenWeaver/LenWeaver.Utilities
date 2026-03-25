using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace LenWeaver.Utilities {

    public static class ResourceHelpers {

        public static string[]?  GetResourceNames( Assembly source, string id, bool includeFolderName ) {

            ResourceReader?     rr      = null;
            Stream?             s       = null;

            string[]?           result  = null;


            try {
                id              = $"{id.ToLower()}/";

                s               = source.GetManifestResourceStream( $"{source.GetName().Name}.g.resources" );
                if( s != null ) { 
                    rr          = new ResourceReader( s );

                    result      = (from entry in rr.OfType<DictionaryEntry>()
                                    let item = entry.Key.ToString()
                                  where item.StartsWith( id )
                                 select includeFolderName ? item : item.Substring( id.Length )).ToArray();

                    Array.Sort( result );
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to retrieve specified resources.", ex );
            }
            finally {
                s?.Dispose();
                rr?.Dispose();
            }

            return result;
        }
        public static string[]?  GetResourceNames( string id, bool includeFolderName ) {

            return GetResourceNames( typeof(ResourceHelpers).Assembly, id, includeFolderName );
        }

        
    }
}