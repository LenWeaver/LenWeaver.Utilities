using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace LenWeaver.Utilities {

    public sealed class BooleanHandler      : TypeHandlerBase<bool> {

        public override string TypeName => "$Boolean";
    }
    public sealed class ByteHandler         : TypeHandlerBase<byte> {

        public override string TypeName => "$Byte";
    }
    public sealed class Int32Handler        : TypeHandlerBase<int> {

        public override string TypeName => "$Int32";
    }
    public sealed class Int64Handler        : TypeHandlerBase<long> {

        public override string TypeName => "$Int64";
    }
    public sealed class DoubleHandler       : TypeHandlerBase<double> {

        public override string TypeName => "$Double";
    }
    public sealed class DecimalHandler      : TypeHandlerBase<decimal> {

        public override string TypeName => "$Decimal";
    }
    public sealed class StringHandler       : TypeHandlerBase<string> {

        public override string TypeName => "$String";

        public override object? FromDbValue( object? dbValue ) => dbValue is DBNull ? null : dbValue?.ToString();
    }

    public sealed class DateTimeHandler     : TypeHandlerBase<DateTime> {

        public override string TypeName => "$DateTime";

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
    public sealed class DateOnlyHandler     : TypeHandlerBase<DateOnly> {

        public override string TypeName => "$DateOnly";

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(DateOnly);
            if( dbValue is DateOnly d ) return d;

            return DateOnly.Parse( dbValue.ToString()!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind );
        }
        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            DateOnly d = (DateOnly)Convert.ChangeType( clrValue, typeof(DateTime) );

            return d;
        }

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            DateOnly d = (DateOnly)Convert.ChangeType( clrValue, typeof(DateTime) );

            return d.ToString( "o", CultureInfo.InvariantCulture );
        }
        public override object? FromText( string? text ) => String.IsNullOrEmpty( text ) ? default(DateOnly) : DateOnly.Parse( text, null, DateTimeStyles.RoundtripKind );
    }
    public sealed class TimeOnlyHandler     : TypeHandlerBase<TimeOnly> {

        public override string TypeName => "$TimeOnly";

        public override object? FromDbValue( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(TimeOnly);
            if( dbValue is TimeOnly t ) return t;

            return TimeOnly.Parse( dbValue.ToString()!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind );
        }
        public override object? ToDbValue( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            TimeOnly t = (TimeOnly)Convert.ChangeType( clrValue, typeof(DateTime) );

            return t;
        }

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            TimeOnly t = (TimeOnly)Convert.ChangeType( clrValue, typeof(DateTime) );

            return t.ToString( "o", CultureInfo.InvariantCulture );
        }
        public override object? FromText( string? text ) => String.IsNullOrEmpty( text ) ? default(TimeOnly) : TimeOnly.Parse( text, null, DateTimeStyles.RoundtripKind );
    }

    public sealed class GuidHandler         : TypeHandlerBase<Guid> {

        public override string TypeName => "$Guid";

        public override string? ToText( object? clrValue ) {

            if( clrValue is null ) return null;
            Guid g = (Guid)Convert.ChangeType( clrValue, typeof(Guid) );

            return g.ToString( "D" );
        }

        public override object? FromText( string? text )
            => string.IsNullOrEmpty( text ) ? default(Guid) : Guid.Parse( text );
    }
    public sealed class ByteArrayHandler    : TypeHandlerBase<byte[]> {

        public override string TypeName => "$ByteArray";
        public override bool IsBlob => true;

        public override object? FromDbValue( object? dbValue ) {

            if(dbValue is null || dbValue is DBNull ) return Array.Empty<byte>();
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

    public sealed class EnumHandler<TEnum>  : TypeHandlerBase<TEnum> where TEnum : struct, Enum {

        public override string TypeName => "$Enum:" + typeof(TEnum).FullName;

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

        public override string? ToText( object? clrValue ) => clrValue?.ToString();
        public override object? FromText( string? text ) => string.IsNullOrEmpty( text ) ? default(TEnum) : Enum.Parse<TEnum>( text, true );
    }

    [UnderConstruction( Developer = "LW", ToDo = "Just testing the attribute." )]
    public sealed class JsonHandler<T>      : TypeHandlerBase<T> {

        public override string TypeName => "$Json:" + typeof(T).FullName;
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
}