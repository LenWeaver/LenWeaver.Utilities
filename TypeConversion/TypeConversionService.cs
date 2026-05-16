using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;

namespace LenWeaver.Utilities {

    public sealed class TypeConversionService : ITypeConversionService {

        private readonly Dictionary<Type,ITypeHandler>      _byCLR                  = new();
        private readonly Dictionary<string,ITypeHandler>    _byName                 = new( StringComparer.Ordinal );

        private readonly Lock                               _sync                   = new();


        public TypeConversionService( IEnumerable<ITypeHandler> handlers ) {

            foreach( ITypeHandler? h in handlers ) Register( h );
        }
        public TypeConversionService() : this( Enumerable.Empty<ITypeHandler>() ) {}


        public void             Register                    ( ITypeHandler handler ) {

            lock(_sync) {
                _byCLR[handler.CLRType]         = handler;
                _byName[handler.TypeName]       = handler;
            }
        }

        public bool             CanHandle                   ( object clrType )      => CanHandle( clrType.GetType() );
        public bool             CanHandle                   ( Type clrType ) {

            lock(_sync) return _byCLR.ContainsKey( clrType );
        }

        public bool             CanHandle                   ( string typeName ) {

            bool        result  = false;

            Type?       t;


            ArgumentException.ThrowIfNullOrWhiteSpace( typeName );

            lock(_sync) {
                if( _byName.ContainsKey( typeName ) ) {
                    result = true;
                }
                else {
                    t = Type.GetType( typeName );
                    if( t is null ) throw ExceptionBuilder.Create<InvalidCastException>( "Specified type name did not convert to an actual type." )
                                                          .AddData( nameof(typeName), typeName );

                    result = CanHandle( t );
                }
            }
            
            return result;
        }

        public int              Count                       => _byCLR.Count;

        public ITypeHandler     GetHandler                  ( object clrType )      => GetHandler( clrType.GetType() );
        public ITypeHandler     GetHandler                  ( Type clrType ) {

            ITypeHandler        handler;

            Type?               def;
            Type?               enumHandlerType;
            Type?               handlerType;


            lock(_sync) {
                // 1. Already created?
                if( _byCLR.TryGetValue( clrType, out var existing ) ) return existing;


                // 2. Enum?
                if( clrType.IsEnum ) {
                    enumHandlerType = typeof(EnumHandler<>).MakeGenericType( clrType );
                    handler         = (ITypeHandler)Activator.CreateInstance( enumHandlerType, this )!;

                    Register( handler );

                    return handler;
                }

                // 3. ValueTuple?
                if( IsValueTuple( clrType ) ) {
                    // Create closed generic ValueTupleHandler<T>
                    handlerType     = typeof(ValueTupleHandler<>).MakeGenericType( clrType );
                    handler         = (ITypeHandler)Activator.CreateInstance( handlerType, this )!;

                    Register( handler );

                    return handler;
                }
                
                // 4. Nullable<T>?
                if( clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(Nullable<>) ) {
                    handlerType     = typeof(NullableHandler<>).MakeGenericType( clrType );
                    handler         = (ITypeHandler)Activator.CreateInstance( handlerType, this )!;

                    Register( handler );

                    return handler;
                }

                //TODO: Make other generic types available for type conversion.
                // 5. Other generic types (List<T>, Dictionary<K,V>, etc.)
                //if( clrType.IsGenericType ) {
                //    def = clrType.GetGenericTypeDefinition();

                //    if( _genericHandlerFactories.TryGetValue( def, out var factory ) ) {
                //        handler     = factory( clrType );

                //        Register( handler );

                //        return handler;
                //    }
                //}

                throw new InvalidOperationException( $"No handler registered for type {clrType.FullName}." );
            }
        }
        public ITypeHandler     GetHandler                  ( string typeName ) {

            ITypeHandler    handler;

            Type?           t;


            ArgumentException.ThrowIfNullOrWhiteSpace( typeName );

            lock(_sync) {
                if( _byName.TryGetValue( typeName, out ITypeHandler? h ) ) {
                    handler = h;
                }
                else {
                    t = Type.GetType( typeName, true );
                    if( t is null ) throw ExceptionBuilder.Create<InvalidCastException>( "Specified type name did not convert to an actual type." )
                                                          .AddData( nameof(typeName), typeName );

                    handler = GetHandler( t );
                }
            }

            return handler;
        }

        public T?               ConvertTo<T>                ( object? dbValue ) {

            ITypeHandler? handler = GetHandler( typeof(T) );
            object? value = handler.FromDbValue( dbValue );

            return (T?)value;
        }
        public object?          ConvertTo                   ( Type targetType, object? dbValue ) {

            ITypeHandler? handler = GetHandler( targetType );

            return handler.FromDbValue( dbValue );
        }

        public string           GetTypeName                 ( Type clrType )        => GetHandler( clrType ).TypeName;
        public Type?            ResolveType                 ( string typeName ) {

            lock(_sync) {
                if( _byName.TryGetValue( typeName, out var h ) ) return h.CLRType;
            }

            return null;
        }


        public static bool      IsValueTuple                ( Type t ) {

            if( !t.IsGenericType ) return false;

            Type? def = t.GetGenericTypeDefinition();


            return def == typeof(ValueTuple<>) ||
                   def == typeof(ValueTuple<,>) ||
                   def == typeof(ValueTuple<,,>) ||
                   def == typeof(ValueTuple<,,,>) ||
                   def == typeof(ValueTuple<,,,,>) ||
                   def == typeof(ValueTuple<,,,,,>) ||
                   def == typeof(ValueTuple<,,,,,,>) ||
                   def == typeof(ValueTuple<,,,,,,,>);
        }

        public static byte[]    Serialize<T>                ( T value )             => JsonSerializer.SerializeToUtf8Bytes( value );
        public static T         Deserialize<T>              ( byte[] data )         => JsonSerializer.Deserialize<T>( data )!;

        public static object    Create                      ( Type tupleType, object?[] values ) {

            Type[]          argTypes;
            object?[]?      fixedValues;


            ArgumentNullException.ThrowIfNull( tupleType );
            ArgumentNullException.ThrowIfNull( values );


            argTypes        = tupleType.GetGenericArguments();
            fixedValues     = new object?[values.Length];

            for( int i = 0; i < values.Length; i++ ) {
                if( values[i] is JsonElement je )
                    fixedValues[i] = ConvertJsonElement( je, argTypes[i] );
                else
                    fixedValues[i] = values[i];
            }

            var method = typeof(ValueTuple)
                .GetMethods( BindingFlags.Public | BindingFlags.Static )
                .Where( m => m.Name == "Create" )
                .Where( m => m.GetParameters().Length == argTypes.Length )
                .First()
                .MakeGenericMethod( argTypes );

            return method.Invoke( null, fixedValues )!;
        }
        public static T         Create<T>                   ( object?[] values ) {

            return (T)Create( typeof(T), values );
        }

        public static string[]  ExtractTupleNames           ( Type t ) {

            string[]                names           = Array.Empty<string>();

            CustomAttributeData?    namesAttr;
                
                
            namesAttr   = t.CustomAttributes.FirstOrDefault( a => a.AttributeType.FullName == "System.Runtime.CompilerServices.TupleElementNamesAttribute" );

            if( namesAttr != null ) {
                names   = ((IReadOnlyList<string>)namesAttr.ConstructorArguments[0].Value!).ToArray<string>();
            }

            return names;
        }

        private static object?  ConvertJsonElement          ( JsonElement elem, Type targetType ) {

            return JsonSerializer.Deserialize( elem.GetRawText(), targetType );
        }
    }
}