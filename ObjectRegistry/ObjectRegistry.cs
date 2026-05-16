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

    public class ObjectRegistry {

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


        private readonly    List<TypeDiscoveryEntry>    lookFor                                 = new List<TypeDiscoveryEntry>();

        public              ExcludedAssemblyCollection  ExcludedAssemblies      { get; }        = new ExcludedAssemblyCollection();
        public              FolderList                  SearchFolders           { get; }        = new FolderList();

        // TODO: Allow ObjectRegister to lookfor other Types (not just interfaces).
        public ObjectRegistry() {}


        public void                                     LookFor                     ( Type t, LoadingStrategies flags ) {

            ArgumentNullException.ThrowIfNull( t );
            UnknownEnumValueException<LoadingStrategies>.ThrowIfUndefined( flags );
            
            if( !t.IsInterface ) throw new ArgumentException( "ObjectRegistry can only look for interfaces at this time." );

            if( lookFor.Find( d => d.SoughtAfterType == t ) != null ) {
                throw new ArgumentException( $"Provided Type: {t.FullName} already exists in list." );
            }

            lookFor.Add( new TypeDiscoveryEntry( t, flags ) );
        }

        public IEnumerable<Type>                        OfType<T>                   () {

            foreach( TypeDiscoveryEntry tde in lookFor ) {
                if( tde.SoughtAfterType == typeof(T) ) {
                    return tde.MatchingTypes.ToArray();
                }
            }

            throw new ArgumentException( $"There are no type entries for type {typeof(T).FullName}." );
        }


        public void                                     Invoke                      () {

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


        private void                                    ScanAssemblyForMatches      ( Assembly ass, TypeDiscoveryEntry tde ) {

            foreach( Type t in GetTypesFromAssembly( ass ) ) {
                if( tde.SoughtAfterType.IsAssignableFrom( t ) )
                    tde.MatchingTypes.Add( t );
            }
        }
        private void                                    SearchReferencedAssemblies  ( TypeDiscoveryEntry tde ) {

            Assembly targetAssembly = tde.SoughtAfterType.Assembly;
            AssemblyName targetAsmName = targetAssembly.GetName();

            // 1. Always scan the interface’s own assembly
            ScanAssemblyForMatches( targetAssembly, tde );

            // 2. Scan assemblies that reference the interface’s assembly
            foreach( Assembly ass in AppDomain.CurrentDomain.GetAssemblies() ) {
                if( ass == targetAssembly )
                    continue;

                if( ShouldSkipAssembly( ass ) )
                    continue;

                bool referencesTarget =
                    ass.GetReferencedAssemblies()
                       .Any( r => r.FullName == targetAsmName.FullName );

                if( !referencesTarget )
                    continue;

                ScanAssemblyForMatches( ass, tde );
            }
        }


        private bool                                    IsUnderConstruction         ( Type t ) {

            return !Debugger.IsAttached && t.GetCustomAttribute<UnderConstructionAttribute>() != null;
        }
        private bool                                    ShouldSkipAssembly          ( Assembly ass ) {

            if( ass.IsDynamic )
                return true;

            if( ass.IsCollectible )
                return true;

            if( String.IsNullOrEmpty( ass.Location ) )
                return true;

            // Skip assemblies explicitly excluded by the user
            foreach( string prefix in ExcludedAssemblies ) {
                if( ass.FullName != null &&
                    ass.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase ) )
                    return true;
            }

            return false;
        }

        private IEnumerable<Type>                       GetTypesFromAssembly        ( Assembly ass ) {

            Type[] types;

            try {
                types = ass.GetTypes();
            }
            catch( ReflectionTypeLoadException ex ) {
                types = ex.Types.Where( t => t != null ).ToArray()!;
            }

            foreach( Type t in types ) {
                if( t != null && !t.IsAbstract && !IsUnderConstruction( t ) )
                    yield return t;
            }
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