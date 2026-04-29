using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace LenWeaver.Utilities {


    public static class NullableConvert {


        static NullableConvert() {}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool          IsAnyNull( object? o )                  => o == null || Convert.IsDBNull( o );

        public static T?            ToNullable<T>( object o ) where T : struct {

            if( IsAnyNull( o ) ) return null;

            return (T)Convert.ChangeType( o, typeof(T) );
        }

        public static bool?         ToBoolean( object o )                   => ToNullable<bool>( o );
        public static byte?         ToByte( object o )                      => ToNullable<byte>( o );
        public static sbyte?        ToSByte( object o )                     => ToNullable<sbyte>( o );
        public static char?         ToChar( object o )                      => ToNullable<char>( o );
        public static short?        ToInt16( object o )                     => ToNullable<short>( o );
        public static ushort?       ToUInt16( object o )                    => ToNullable<ushort>( o );
        public static int?          ToInt32( object o )                     => ToNullable<int>( o );
        public static uint?         ToUInt32( object o )                    => ToNullable<uint>( o );
        public static long?         ToInt64( object o )                     => ToNullable<long>( o );
        public static ulong?        ToUInt64( object o )                    => ToNullable<ulong>( o );
        public static float?        ToSingle( object o )                    => ToNullable<float>( o );
        public static double?       ToDouble( object o )                    => ToNullable<double>( o );
        public static decimal?      ToDecimal( object o )                   => ToNullable<decimal>( o );
        public static DateTime?     ToDateTime( object o )                  => ToNullable<DateTime>( o );
        public static string?       ToString( object o )                    => IsAnyNull( o ) ? (string?)null   : Convert.ToString( o );
        public static string?       ToString( object o, string ifNull )     => IsAnyNull( o ) ? ifNull          : Convert.ToString( o );

        public static DateOnly?     ToDateOnly( object? o ) {

            if( IsAnyNull( o ) ) return null;

            return o switch  {
                DateOnly     d  => d,
                DateTime    dt  => DateOnly.FromDateTime( dt ),
                string       s  => DateOnly.Parse( s ),
                _               => throw new InvalidCastException( "Unable to cast to DateOnly." )
            };
        }
        public static TimeOnly?     ToTimeOnly( object? o ) {

            if( IsAnyNull( o ) ) return null;

            return o switch  {
                TimeOnly     t  => t,
                TimeSpan    ts  => TimeOnly.FromTimeSpan( ts ),
                DateTime    dt  => TimeOnly.FromDateTime( dt ),
                string       s  => TimeOnly.Parse( s ),
                _               => throw new InvalidCastException( "Unable to cast to TimeOnly." )
            };
        }
        public static TimeSpan?     ToTimeSpan( object o ) {

            if( IsAnyNull( o ) ) return null;

            return o switch {
                TimeSpan    t => t,
                TimeOnly   to => new TimeSpan( to.Ticks ),
                DateTime   dt => dt.TimeOfDay,
                string      s => TimeSpan.Parse( s ),
                _             => throw new InvalidCastException( "Unable to cast to TimeSpan." )
            };
        }
    }
}