using System;
using System.Collections.Generic;


namespace LenWeaver.Utilities {


    public static class NullableConvert {


        static NullableConvert() {}


        public static bool      IsAnyNull( object o )               => o == null || Convert.IsDBNull( o );


        public static bool?     ToBoolean( object o )               => IsAnyNull( o ) ? (bool?)null     : Convert.ToBoolean( o );
        public static byte?     ToByte( object o )                  => IsAnyNull( o ) ? (byte?)null     : Convert.ToByte( o );
        public static sbyte?    ToSByte( object o )                 => IsAnyNull( o ) ? (sbyte?)null    : Convert.ToSByte( o );
        public static char?     ToChar( object o )                  => IsAnyNull( o ) ? (char?)null     : Convert.ToChar( o );
        public static short?    ToInt16( object o )                 => IsAnyNull( o ) ? (short?)null    : Convert.ToInt16( o );
        public static ushort?   ToUInt16( object o )                => IsAnyNull( o ) ? (ushort?)null   : Convert.ToUInt16( o );
        public static int?      ToInt32( object o )                 => IsAnyNull( o ) ? (int?)null      : Convert.ToInt32( o );
        public static uint?     ToUInt32( object o )                => IsAnyNull( o ) ? (uint?)null     : Convert.ToUInt32( o );
        public static long?     ToInt64( object o )                 => IsAnyNull( o ) ? (long?)null     : Convert.ToInt64( o );
        public static ulong?    ToUInt64( object o )                => IsAnyNull( o ) ? (ulong?)null    : Convert.ToUInt64( o );
        public static float?    ToSingle( object o )                => IsAnyNull( o ) ? (float?)null    : Convert.ToSingle( o );
        public static double?   ToDouble( object o )                => IsAnyNull( o ) ? (double?)null   : Convert.ToDouble( o );
        public static decimal?  ToDecimal( object o )               => IsAnyNull( o ) ? (decimal?)null  : Convert.ToDecimal( o );
        public static DateTime? ToDateTime( object o )              => IsAnyNull( o ) ? (DateTime?)null : Convert.ToDateTime( o );
        public static string?   ToString( object o )                => IsAnyNull( o ) ? (string?)null   : Convert.ToString( o );
        public static string?   ToString( object o, string ifNull ) => IsAnyNull( o ) ? ifNull          : Convert.ToString( o );
    }
}