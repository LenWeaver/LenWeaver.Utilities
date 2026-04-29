using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LenWeaver.Utilities {

    public sealed class TypeConversionService : ITypeConversionService {

        private readonly Dictionary<Type,ITypeHandler>      _byCLR      = new();
        private readonly Dictionary<string,ITypeHandler>    _byName     = new( StringComparer.Ordinal );

        private readonly Lock _sync = new();


        public TypeConversionService( IEnumerable<ITypeHandler> handlers ) {

            foreach( ITypeHandler? h in handlers ) Register(h);
        }
        public TypeConversionService() : this( Enumerable.Empty<ITypeHandler>() ) {}


        public int              Count           => _byCLR.Count;

        public void             Register        ( ITypeHandler handler ) {

            lock(_sync) {
                _byCLR[handler.CLRType]     = handler;
                _byName[handler.TypeName]   = handler;
            }
        }

        public bool             CanHandle       ( Type clrType ) {
            lock(_sync) return _byCLR.ContainsKey( clrType );
        }
        public bool             CanHandle       ( string typeName ) {

            lock(_sync) return _byName.ContainsKey( typeName );
        }

        public ITypeHandler     GetHandler      ( Type clrType ) {

            lock(_sync) {
                if( _byCLR.TryGetValue( clrType, out var h ) ) return h;

                if( clrType.IsEnum ) {
                    Type? enumHandlerType = typeof(EnumHandler<>).MakeGenericType( clrType );
                    ITypeHandler? handler = (ITypeHandler)Activator.CreateInstance( enumHandlerType )!;
                    Register( handler );

                    return handler;
                }

                throw new InvalidOperationException( $"No handler registered for type {clrType.FullName}." );
            }
        }
        public ITypeHandler     GetHandler      ( string typeName ) {

            lock(_sync) {
                if( _byName.TryGetValue( typeName, out var h ) ) return h;
                throw new InvalidOperationException( $"No handler registered for type name '{typeName}'." );
            }
        }

        public T?               ConvertTo<T>    ( object? dbValue ) {

            ITypeHandler? handler = GetHandler( typeof(T) );
            object? value = handler.FromDbValue( dbValue );

            return (T?)value;
        }
        public object?          ConvertTo       ( Type targetType, object? dbValue ) {

            ITypeHandler? handler = GetHandler( targetType );

            return handler.FromDbValue( dbValue );
        }

        public string           GetTypeName     ( Type clrType ) => GetHandler( clrType ).TypeName;
        public Type?            ResolveType     ( string typeName ) {

            lock(_sync) {
                if( _byName.TryGetValue( typeName, out var h ) ) return h.CLRType;
            }

            return null;
        }
    }
}