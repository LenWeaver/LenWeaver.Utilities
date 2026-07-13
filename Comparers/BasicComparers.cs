using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace LenWeaver.Utilities {

    public static class BasicComparers {

        public static BooleanComparer           Boolean             {
            get => field ??= new BooleanComparer();
        }
        public static ByteComparer              Byte                {
            get => field ??= new ByteComparer();
        }
        public static SByteComparer             SByte               {
            get => field ??= new SByteComparer();
        }
        public static Int16Comparer             Int16               {
            get => field ??= new Int16Comparer();
        }
        public static UInt16Comparer            UInt16              {
            get => field ??= new UInt16Comparer();
        }
        public static Int32Comparer             Int32               {
            get => field ??= new Int32Comparer();
        }
        public static UInt32Comparer            UInt32              {
            get => field ??= new UInt32Comparer();
        }
        public static Int64Comparer             Int64               {
            get => field ??= new Int64Comparer();
        }
        public static UInt64Comparer            UInt64              {
            get => field ??= new UInt64Comparer();
        }
        public static SingleComparer            Single              {
            get => field ??= new SingleComparer();
        }
        public static DoubleComparer            Double              {
            get => field ??= new DoubleComparer();
        }
        public static DecimalComparer           Decimal             {
            get => field ??= new DecimalComparer();
        }
        public static DateTimeComparer          DateTime            {
            get => field ??= new DateTimeComparer();
        }
        public static DateOnlyComparer          DateOnly            {
            get => field ??= new DateOnlyComparer();
        }
        public static TimeOnlyComparer          TimeOnly            {
            get => field ??= new TimeOnlyComparer();
        }
        public static TimeSpanComparer          TimeSpan            {
            get => field ??= new TimeSpanComparer();
        }
        public static CharComparer              Char                {
            get => field ??= new CharComparer();
        }

        public static TextComparer              StringComparerCurrentCulture                {
            get => field ??= new TextComparer( StringComparison.CurrentCulture );
        }
        public static TextComparer              StringComparerCurrentCultureIgnoreCase      {
            get => field ??= new TextComparer( StringComparison.CurrentCultureIgnoreCase );
        }
        public static TextComparer              StringComparerInvariantCulture              {
            get => field ??= new TextComparer( StringComparison.InvariantCulture );
        }
        public static TextComparer              StringComparerInvariantCultureIgnoreCase    {
            get => field ??= new TextComparer( StringComparison.InvariantCultureIgnoreCase );
        }
        public static TextComparer              StringComparerOrdinal                       {
            get => field ??= new TextComparer( StringComparison.Ordinal );
        }
        public static TextComparer              StringComparerOrdinalIgnoreCase             {
            get => field ??= new TextComparer( StringComparison.OrdinalIgnoreCase );
        }

        public static IComparer                 FindComparer( Type searchFor ) {

            ArgumentNullException.ThrowIfNull( searchFor );

            if( searchFor.IsEnum ) searchFor = searchFor.GetEnumUnderlyingType();

            if     ( searchFor == typeof(bool) )        return Boolean;
            else if( searchFor == typeof(byte) )        return Byte;
            else if( searchFor == typeof(sbyte) )       return SByte;
            else if( searchFor == typeof(short) )       return Int16;
            else if( searchFor == typeof(ushort) )      return UInt16;
            else if( searchFor == typeof(int) )         return Int32;
            else if( searchFor == typeof(uint) )        return UInt32;
            else if( searchFor == typeof(long) )        return Int64;
            else if( searchFor == typeof(ulong) )       return UInt64;
            else if( searchFor == typeof(float) )       return Single;
            else if( searchFor == typeof(double) )      return Double;
            else if( searchFor == typeof(decimal) )     return Decimal;
            else if( searchFor == typeof(DateTime) )    return DateTime;
            else if( searchFor == typeof(DateOnly) )    return DateOnly;
            else if( searchFor == typeof(TimeOnly) )    return TimeOnly;
            else if( searchFor == typeof(TimeSpan) )    return TimeSpan;
            else if( searchFor == typeof(char) )        return Char;
            else if( searchFor == typeof(string) )      return StringComparerCurrentCulture;

            else {
                throw ExceptionBuilder.Create<ArgumentOutOfRangeException>( $"The specified type: {searchFor.FullName} is not supported " +
                                                                            $"by the {nameof(FindComparer)} method." )
                                      .AddData( "Supported Types:", "Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Single, Double, " +
                                                                    "Decimal, DateTime, DateOnly, TimeOnly, TimeSpan and Char.  Strings " +
                                                                    "will default to StringComparerCurrentCulture." );
            }
        }
        public static IComparer<T?>             FindComparer<T>() {

            Type t = typeof(T);


            if( t.IsEnum ) {
                Type underlying = t.GetEnumUnderlyingType();
                IComparer underlyingComparer = FindComparer( underlying );

                return (IComparer<T?>)Activator.CreateInstance( typeof(EnumComparer<>).MakeGenericType( t ),
                                                                underlyingComparer )!;
            }

            if     ( typeof(T) == typeof(bool) )        return (IComparer<T?>)Boolean;
            else if( typeof(T) == typeof(byte) )        return (IComparer<T?>)Byte;
            else if( typeof(T) == typeof(sbyte) )       return (IComparer<T?>)SByte;
            else if( typeof(T) == typeof(short) )       return (IComparer<T?>)Int16;
            else if( typeof(T) == typeof(ushort) )      return (IComparer<T?>)UInt16;
            else if( typeof(T) == typeof(int) )         return (IComparer<T?>)Int32;
            else if( typeof(T) == typeof(uint) )        return (IComparer<T?>)UInt32;
            else if( typeof(T) == typeof(long) )        return (IComparer<T?>)Int64;
            else if( typeof(T) == typeof(ulong) )       return (IComparer<T?>)UInt64;
            else if( typeof(T) == typeof(float) )       return (IComparer<T?>)Single;
            else if( typeof(T) == typeof(double) )      return (IComparer<T?>)Double;
            else if( typeof(T) == typeof(decimal) )     return (IComparer<T?>)Decimal;
            else if( typeof(T) == typeof(DateTime) )    return (IComparer<T?>)DateTime;
            else if( typeof(T) == typeof(DateOnly) )    return (IComparer<T?>)DateOnly;
            else if( typeof(T) == typeof(TimeOnly) )    return (IComparer<T?>)TimeOnly;
            else if( typeof(T) == typeof(TimeSpan) )    return (IComparer<T?>)TimeSpan;
            else if( typeof(T) == typeof(char) )        return (IComparer<T?>)Char;
            else if( typeof(T) == typeof(string) )      return (IComparer<T?>)StringComparerCurrentCulture;

            else {
                throw ExceptionBuilder.Create<ArgumentOutOfRangeException>( $"The specified type: {typeof(T).FullName} is not supported " +
                                                                            $"by the {nameof(FindComparer)}<> method." )
                                      .AddData( "Supported Types:", "Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, " +
                                                                    "Single, Double, Decimal, DateTime, DateOnly, TimeOnly, TimeSpan and Char.  " +
                                                                    "Strings will default to StringComparerCurrentCulture." );
            }
        }
        public static IComparer<string>         FindStringComparer( StringComparison cmp ) {

            switch( cmp ) {
                case StringComparison.CurrentCulture:               return StringComparerCurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:     return StringComparerCurrentCultureIgnoreCase;
                case StringComparison.InvariantCulture:             return StringComparerInvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase:   return StringComparerInvariantCultureIgnoreCase;
                case StringComparison.Ordinal:                      return StringComparerOrdinal;
                case StringComparison.OrdinalIgnoreCase:            return StringComparerOrdinalIgnoreCase;

                default:
                    throw ExceptionBuilder.Create<UnknownEnumValueException<StringComparison>>( $"Unknown value for enum {nameof(StringComparison)}." );
            }
        }


        public static bool TryCompareNulls( object? x, object? y, CompareNullPlacement nullPlacement, [NotNullWhen( true )] out int result ) {

            if( x is null && y is null ) {
                result = 0;
                return true;
            }

            if( x is null ) {
                result = nullPlacement == CompareNullPlacement.NullsOnTop ? -1 : 1;
                return true;
            }

            if( y is null ) {
                result = nullPlacement == CompareNullPlacement.NullsOnTop ? 1 : -1;
                return true;
            }

            result = 0;
            return false;
        }
        public static bool TryCompareNulls( object? x, object? y, [NotNullWhen( true )] out int result ) => TryCompareNulls( x, y,
                                                                                                                             CompareNullPlacement.NullsOnTop,
                                                                                                                             out result );
    }

    public class BooleanComparer                : BasicValueComparerBase<bool>          {}
    public class ByteComparer                   : BasicValueComparerBase<byte>          {}
    public class SByteComparer                  : BasicValueComparerBase<sbyte>         {}
    public class Int16Comparer                  : BasicValueComparerBase<short>         {}
    public class UInt16Comparer                 : BasicValueComparerBase<ushort>        {}
    public class Int32Comparer                  : BasicValueComparerBase<int>           {}
    public class UInt32Comparer                 : BasicValueComparerBase<uint>          {}
    public class Int64Comparer                  : BasicValueComparerBase<long>          {}
    public class UInt64Comparer                 : BasicValueComparerBase<ulong>         {}
    public class SingleComparer                 : BasicValueComparerBase<float>         {}
    public class DoubleComparer                 : BasicValueComparerBase<double>        {}
    public class DecimalComparer                : BasicValueComparerBase<decimal>       {}
    public class DateTimeComparer               : BasicValueComparerBase<DateTime>      {}
    public class DateOnlyComparer               : BasicValueComparerBase<DateOnly>      {}
    public class TimeOnlyComparer               : BasicValueComparerBase<TimeOnly>      {}
    public class TimeSpanComparer               : BasicValueComparerBase<TimeSpan>      {}
    public class CharComparer                   : BasicValueComparerBase<char>          {}

    public class TextComparer                   : BasicReferenceComparerBase<string>    {

        private readonly StringComparison    comparisonType;


        internal TextComparer( StringComparison cmp ) : base() {

            comparisonType  = cmp;
        }

        protected override int CompareCore( string? x, string? y ) {

            if( x != null && y != null )    return x.CompareTo( y, comparisonType );
            if( x == null && y == null )    return 0;

            return x != null ? 1 : -1;
        }
    }
    public class EnumComparer<TEnum>            : IComparer<TEnum?> where TEnum : struct, Enum {

        private readonly IComparer _underlying;

        public EnumComparer( IComparer underlying ) {

            _underlying = underlying;
        }

        public int Compare( TEnum? x, TEnum? y ) {

            if( BasicComparers.TryCompareNulls( x, y, out int result ) ) {
                return result;
            }

            object ux = Convert.ChangeType( x!.Value, Enum.GetUnderlyingType( typeof(TEnum) ) );
            object uy = Convert.ChangeType( y!.Value, Enum.GetUnderlyingType( typeof(TEnum) ) );

            return _underlying.Compare( ux, uy );
        }
    }

    public abstract class BasicComparerBase<T>              : IComparer<T>, IComparer {

        public int Compare( T? x, T? y ) => CompareCore( x, y );

        int IComparer.Compare( object? x, object? y ) {

            if( BasicComparers.TryCompareNulls( x, y, out int result ) ) {
                return result;
            }
        
            if( x is T tX && y is T tY )    return CompareCore( tX, tY );
        
            if( x!.GetType().IsEnum ) {
                return ((IComparable)x!).CompareTo( y );
            }

            throw new ArgumentException( $"Arguments are not of type {typeof(T).FullName}." );
        }

        protected abstract int CompareCore( T? x, T? y );
    }
    public abstract class BasicValueComparerBase<T>         : BasicComparerBase<T?> where T : struct, IComparable<T> {

        protected override int CompareCore( T? x, T? y ) {

            if( BasicComparers.TryCompareNulls( x, y, out int result ) ) {
                return result;
            }

            return x!.Value.CompareTo( y!.Value );
        }
    }
    public abstract class BasicReferenceComparerBase<T>     : BasicComparerBase<T> where T : class, IComparable<T> {

        protected override int CompareCore( T? x, T? y ) {

            if( BasicComparers.TryCompareNulls( x, y, out int result ) ) {
                return result;
            }

            return x!.CompareTo( y );
        }
    }
}