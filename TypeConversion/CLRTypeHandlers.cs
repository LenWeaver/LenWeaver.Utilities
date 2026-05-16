using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace LenWeaver.Utilities {

    public sealed class BooleanHandler          : TypeHandlerBase<bool>     {}
    public sealed class CharHandler             : TypeHandlerBase<char>     {}
    public sealed class ByteHandler             : TypeHandlerBase<byte>     {}
    public sealed class SByteHandler            : TypeHandlerBase<sbyte>    {}
    public sealed class Int16Handler            : TypeHandlerBase<short>    {}
    public sealed class UInt16Handler           : TypeHandlerBase<ushort>   {}
    public sealed class Int32Handler            : TypeHandlerBase<int>      {}
    public sealed class UInt32Handler           : TypeHandlerBase<uint>     {}
    public sealed class Int64Handler            : TypeHandlerBase<long>     {}
    public sealed class UInt64Handler           : TypeHandlerBase<ulong>    {}
    public sealed class SingleHandler           : TypeHandlerBase<float>    {}
    public sealed class DoubleHandler           : TypeHandlerBase<double>   {}
    public sealed class DecimalHandler          : TypeHandlerBase<decimal>  {}
    public sealed class StringHandler           : TypeHandlerBase<string> {

        public override object? FromDbValue ( object? dbValue )     => dbValue is DBNull ? null : dbValue?.ToString();
        public override object? ToDbValue   ( object? clrValue )    => clrValue ?? (object)DBNull.Value;
    }

    public sealed class DateTimeHandler         : TypeHandlerBase<DateTime> {

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(DateTime);
            if( dbValue is DateTime dt ) return dt;

            return DateTime.Parse( dbValue.ToString()!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind );
        }
        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            DateTime dt = (DateTime)Convert.ChangeType( clrValue, typeof(DateTime) );

            return dt;
        }

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            DateTime dt = (DateTime)Convert.ChangeType( clrValue, typeof(DateTime) );

            return dt.ToString( "o", CultureInfo.InvariantCulture );
        }
        public override object? FromText( string? text )
            => string.IsNullOrEmpty( text ) ? default(DateTime) : DateTime.Parse( text, null, DateTimeStyles.RoundtripKind );
    }
    public sealed class DateOnlyHandler         : TypeHandlerBase<DateOnly> {

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(DateOnly);
            if( dbValue is DateOnly d )     return d;
            if( dbValue is DateTime dt )    return DateOnly.FromDateTime( dt );

            return DateOnly.Parse( dbValue.ToString()!, CultureInfo.InvariantCulture );
        }

        public override object? ToDbValue( object? clrValue )
        {
            if( clrValue is null )          return DBNull.Value;
            if( clrValue is DateOnly d )    return d;

            throw new InvalidCastException( "Expected DateOnly." );
        }

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            return ((DateOnly)clrValue).ToString( DateTimeHelpers.ISO8601DateFormat, CultureInfo.InvariantCulture );
        }

        public override object? FromText( string? text ) => string.IsNullOrEmpty( text )
                                                          ? default(DateOnly)
                                                          : DateOnly.Parse( text, CultureInfo.InvariantCulture );
    }
    public sealed class TimeOnlyHandler         : TypeHandlerBase<TimeOnly> {

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull )  return default(TimeOnly);
            if( dbValue is TimeOnly t )                 return t;
            if( dbValue is DateTime dt )                return TimeOnly.FromDateTime( dt );

            return TimeOnly.Parse( dbValue.ToString()!, CultureInfo.InvariantCulture );
        }
        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null )          return DBNull.Value;
            if( clrValue is TimeOnly t )    return t;

            throw new InvalidCastException( "Expected TimeOnly." );
        }

        public override string? ToText( object? clrValue )
        {
            if( clrValue is null ) return null;
            return ( (TimeOnly)clrValue).ToString("HH:mm:ss.fffffff", CultureInfo.InvariantCulture );
        }
        public override object? FromText( string? text ) => String.IsNullOrEmpty( text )
                                                          ? default(TimeOnly)
                                                          : TimeOnly.Parse( text, CultureInfo.InvariantCulture );
    }

    public sealed class GuidHandler             : TypeHandlerBase<Guid> {

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            Guid g = (Guid)Convert.ChangeType( clrValue, typeof(Guid) );

            return g.ToString( "D" );
        }

        public override object? FromText( string? text )
            => string.IsNullOrEmpty( text ) ? default(Guid) : Guid.Parse( text );
    }

    public sealed class ByteArrayHandler        : TypeHandlerBase<byte[]> {

        public override bool IsBlob => true;

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return Array.Empty<byte>();
            if( dbValue is byte[] b ) return b;

            throw new InvalidCastException( "Expected byte[] from DB." );
        }
        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            if( clrValue is byte[] b ) return b;

            throw new InvalidCastException( "Expected byte[] for ByteArrayHandler." );
        }

        public override byte[]? ToBlob( object? clrValue ) => clrValue as byte[];
        public override object? FromBlob( byte[]? blob ) => blob ?? Array.Empty<byte>();
    }

    public sealed class EnumHandler<TEnum>      : TypeHandlerBase<TEnum> where TEnum : struct, Enum {

        public EnumHandler( ITypeConversionService tcs ) : base( tcs ) {}


        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(TEnum);
            if( dbValue is string s ) return Enum.Parse<TEnum>( s, ignoreCase: true );

            object? underlying = Convert.ChangeType( dbValue, Enum.GetUnderlyingType( typeof(TEnum) ) );

            return Enum.ToObject( typeof(TEnum), underlying) ;
        }

        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            if( clrValue is TEnum e ) return e.ToString();

            throw new InvalidCastException();
        }

        public override string? ToText( object? clrValue )  => clrValue?.ToString();
        public override object? FromText( string? text )    => String.IsNullOrEmpty( text ) ? default(TEnum) : Enum.Parse<TEnum>( text, true );
    }
    public sealed class JsonHandler<T>          : TypeHandlerBase<T> {

        public JsonHandler( ITypeConversionService tcs ) : base( tcs ) {}


        public override bool IsBlob => false;

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            return JsonSerializer.Serialize( (T)clrValue );
        }
        public override object? FromText( string? text ) {

            if( String.IsNullOrEmpty( text ) ) return default(T);
            return JsonSerializer.Deserialize<T>( text );
        }

        public override object? FromDbValue( object? dbValue ) => FromText( dbValue as string );
        public override object? ToDbValue( object? clrValue )  => ToText( clrValue ) ?? (object)DBNull.Value;
    }
    public sealed class NullableHandler<T>      : TypeHandlerBase<T?> where T : struct {

        public NullableHandler( ITypeConversionService converter ) : base( converter ) {

            _innerType  = typeof(T);
            _inner      = converter.GetHandler( _innerType );
        }


        private readonly ITypeHandler   _inner;
        private readonly Type           _innerType;

        public override bool IsBlob     => _inner.IsBlob;

        public override object? FromDbValue ( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return null;

            return _inner.FromDbValue( dbValue );
        }
        public override object? ToDbValue   ( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;

            return _inner.ToDbValue( clrValue );
        }

        public override string? ToText      ( object? clrValue ) {

            if( clrValue is null ) return null;

            return _inner.ToText( clrValue );
        }
        public override object? FromText    ( string? text ) {

            if( String.IsNullOrEmpty( text ) ) return null;

            return _inner.FromText( text );
        }

        public override byte[]? ToBlob      ( object? clrValue ) {

            if( clrValue is null ) return null;

            return _inner.ToBlob( clrValue );
        }
        public override object? FromBlob    ( byte[]? blob ) {

            if( blob is null || blob.Length == 0 ) return null;

            return _inner.FromBlob( blob );
        }
    }

    public sealed class ValueTupleHandler<T>    : TypeHandlerBase<T> {

        public ValueTupleHandler( ITypeConversionService tcs ) : base( tcs ) {}


        public override bool IsBlob         => true;

        public override byte[]? ToBlob( object? clrValue ) {

            Type?           type;

            object?[]?      values;
            FieldInfo[]     fields;


            if( clrValue is null ) return null;

            type            = clrValue.GetType();

            fields          = type.GetFields();
            values          = fields.Select( f => f.GetValue( clrValue ) ).ToArray();

            return JsonSerializer.SerializeToUtf8Bytes( values );
        }

        public override object? FromBlob( byte[]? blob ) {

            object[]?       deserialized;


            if( blob is null ) return default(T);

            deserialized    = JsonSerializer.Deserialize<object[]>( blob! );

            return TypeConversionService.Create<T>( deserialized! );
        }
    }

    public sealed class FontDescriptorHandler   : TypeHandlerBase<FontDescriptor> {

        public override object? ToDbValue( object? clrValue ) {

            if( clrValue == null ) return (string?)null;
            return clrValue.ToString();
        }
        public override object? FromDbValue( object? dbValue ) {

            if( dbValue == null || dbValue is DBNull ) return (FontDescriptor?)null;
            return new FontDescriptor( dbValue.ToString()! );
        }

        public override string? ToText( object? clrValue ) {

            if( clrValue == null ) return (string?)null;
            return ToDbValue( clrValue )!.ToString();
        }
        public override object? FromText( string? text ) {

            if( String.IsNullOrWhiteSpace( text ) ) return (FontDescriptor?)null;
            return new FontDescriptor( text );
        }
    }
}