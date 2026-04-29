using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace LenWeaver.Utilities {

    public static class ObjectRegistry {

        #region Assembly groups that may need to be skipped.
        public static readonly  string[]                    WPFAssemblies       = ["PresentationCore","PresentationFramework","System.Xaml",
                                                                                   "UIAutomationProvider","WindowsBase"];
        public static readonly  string[]                    BCLAssemblies       = ["System.Collections","System.ComponentModel.Primitives",
                                                                                   "System.ComponentModel.TypeConverter","System.IO.FileSystem.DriveInfo",
                                                                                   "System.Linq","System.ObjectModel","System.Runtime",
                                                                                   "System.Threading","System.Threading.Thread","System.Xml.ReaderWriter"];
        public static readonly  string[]                    CommonExtensions    = ["System.Drawing.Common","System.Memory","System.Text.Json",
                                                                                   "Microsoft.Win32"];
        public static readonly  string[]                    DatabaseAssemblies  = ["System.Data.Common","System.Data.SQLite","System.Data.SqlClient"];
        #endregion


        private static readonly         List<Type>                  types                                   = new List<Type>();
        private static readonly         List<TypeDiscoveryEntry>    lookFor                                 = new List<TypeDiscoveryEntry>();

        public static                   ExcludedAssemblyCollection  ExcludedAssemblies      { get; }        = new ExcludedAssemblyCollection();
        public static                   FolderList                  SearchFolders           { get; }        = new FolderList();

        // TODO: Allow ObjectRegister to lookfor other Types (not just interfaces).
        static ObjectRegistry() {}


        public static void              LookFor                     ( Type t, LoadingStrategies flags ) {

            ArgumentNullException.ThrowIfNull( t );
            UnknownEnumValueException<LoadingStrategies>.ThrowIfUndefined( flags );
            
            if( !t.IsInterface ) throw new ArgumentException( "ObjectRegistry can only look for interfaces at this time." );

            if( lookFor.Find( d => d.SoughtAfterType == t ) != null ) {
                throw new ArgumentException( $"Provided Type: {t.FullName} already exists in list." );
            }

            lookFor.Add( new TypeDiscoveryEntry( t, flags ) );
        }


        public static void              Invoke                      () {

            foreach( TypeDiscoveryEntry tde in lookFor ) {
                if( (tde.LoadingStrategies & LoadingStrategies.ApplicationFolder) != 0 ) {
                    throw new NotImplementedException();
                }

                if( (tde.LoadingStrategies & LoadingStrategies.LoadedAssemblies) != 0 ) {
                    throw new NotImplementedException();
                }

                if( (tde.LoadingStrategies & LoadingStrategies.ReferencedAssemblies) != 0 ) {
                    SearchReferencedAssemblies( tde );
                }
                
                if( (tde.LoadingStrategies & LoadingStrategies.SpecifiedFolders) != 0 ) {
                    throw new NotImplementedException();
                }
            }
        }


        public static IEnumerable<Type> OfType<T>                   () {

            foreach( TypeDiscoveryEntry tde in lookFor ) {
                if( tde.SoughtAfterType == typeof(T) ) {
                    return tde.MatchingTypes.ToArray();
                }
            }

            throw new ArgumentException( $"There are no type entries for type {typeof(T).FullName}." );
        }

        private static void             SearchReferencedAssemblies  ( TypeDiscoveryEntry tde ) {

            bool                found;


            try {
                foreach( Assembly ass in AppDomain.CurrentDomain.GetAssemblies() ) {
                    found = false;

                    foreach( string s in ExcludedAssemblies ) {
                        if( ass.FullName != null && ass.FullName.StartsWith( s, StringComparison.OrdinalIgnoreCase ) ) {
                            found = true;
                            break;
                        }
                    }

                    if( !found ) {
                        foreach( Type t in GetTypesFromAssembly( ass ) ) {
                            if( tde.SoughtAfterType.IsAssignableFrom( t ) ) {
                                tde.MatchingTypes.Add( t );
                            }
                        }
                    }
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to search referenced assemblies.", ex );
            }
        }

        private static bool             IncludeType                 ( [NotNullWhen(true)]Type? t ) {

#if DEBUG
            bool        isNull      = t == null;
            bool        underCon    = IsUnderConstruction( t );
            bool        isAb        = t.IsAbstract;

            return !isNull && !underCon && !isAb;
#else
            return t != null && !IsUnderConstruction( t ) && !t.IsAbstract;
#endif
        }
        private static bool             IsUnderConstruction         ( Type t ) {

            return !Debugger.IsAttached && t.GetCustomAttribute<UnderConstructionAttribute>() != null;
        }

        private static Type[]           GetTypesFromAssembly        ( Assembly ass ) {

            //List<Type>      loadedTypes;
            

            try {
                types.Clear();

                if( !ass.IsDynamic && !ass.IsCollectible && !String.IsNullOrEmpty( ass.Location ) ) {
                    types.AddRange( ass.GetTypes() );
                }
            }
            catch( ReflectionTypeLoadException ex ) {
                //Some types may have been loaded despite the exception.
                types.AddRange( (IEnumerable<Type>)ex.Types.RemoveIf<Type?>( t => t == null ) );
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to extract Type information from assembly.", ex );
            }

            try {
                //loadedTypes = new List<Type>();

                //foreach( Type t in types ) {
                //    if( IncludeType( t ) ) {
                //        loadedTypes.Add( t );
                //    }
                //}

                for( int i = types.Count - 1; i >= 0; i-- ) {
                    if( !IncludeType( types[i] ) ) {
                        types.RemoveAt( i );
                    }
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to filter Types from list.", ex );
            }

            return types.ToArray();
        }


        #region TypeDiscoveryEntry Inner Class
        private class TypeDiscoveryEntry {

            public  LoadingStrategies   LoadingStrategies   { get; init; }

            public  Type                SoughtAfterType     { get; init; }

            public  List<Type>          MatchingTypes       { get; }            = new List<Type>();


            internal TypeDiscoveryEntry( Type soughtAfterType, LoadingStrategies flags ) {

                LoadingStrategies   = flags;
                SoughtAfterType     = soughtAfterType;
            }


            public override string ToString() => $"Looking For: {SoughtAfterType.FullName}   Matches Found: {MatchingTypes.Count}.";
        }
        #endregion
    }
}