using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LenWeaver.Utilities {

    public static class PseudoEnum<TPseudoEnum> {

        private static readonly Dictionary<string, TPseudoEnum> _map;

        static PseudoEnum() {

            Type?   providerType;


            // Find the static class that contains the named instances
            providerType = typeof(TPseudoEnum).Assembly
                .GetTypes()
                .FirstOrDefault( t =>
                    t.IsClass &&
                    t.IsAbstract &&
                    t.IsSealed && // static class
                    t.GetProperties( BindingFlags.Public | BindingFlags.Static )
                     .Any( p => p.PropertyType == typeof(TPseudoEnum) ) );

            if( providerType == null ) {
                throw new InvalidOperationException( $"PseudoEnum provider not found for {typeof(TPseudoEnum).Name}." );
            }

            _map = providerType
                .GetProperties( BindingFlags.Public | BindingFlags.Static )
                .Where( p => p.PropertyType == typeof(TPseudoEnum) )
                .ToDictionary(
                    p => p.Name,
                    p => (TPseudoEnum)p.GetValue( null )!,
                    StringComparer.OrdinalIgnoreCase
                );
        }

        public static bool TryParse( string s, [NotNullWhen(true)] out TPseudoEnum value ) => _map.TryGetValue( s, out value );
        public static bool Validate( string s ) => _map.ContainsKey( s );

        public static IReadOnlyDictionary<string, TPseudoEnum> Values => _map;
    }
}