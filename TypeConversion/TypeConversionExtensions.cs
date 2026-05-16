using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LenWeaver.Utilities {

    public static class TypeConversionExtensions {

        public static DbParameter AddTypedParameter( this DbCommand cmd, string name, object? value, ITypeConversionService types ) {

            DbParameter? parameter = cmd.CreateParameter();
            parameter.ParameterName = name;

            if( value is null ) {
                parameter.Value = DBNull.Value;
                cmd.Parameters.Add(parameter);
                
                return parameter;
            }

            ITypeHandler handler = types.GetHandler( value.GetType() );
            parameter.Value = handler.ToDbValue( value );

            // Optionally set DbType based on handler.ClrType
            // parameter.DbType = ...

            cmd.Parameters.Add( parameter );
            return parameter;
        }

        public static CLRTypeCode GetCLRTypeCode( this object? o ) {

            Type        t;


            if( o is null )                     return CLRTypeCode.Empty;
            if( o is DBNull )                   return CLRTypeCode.DBNull;

            t = o.GetType();

            //Most common date types first for performance.
            if( t == typeof(string) )           return CLRTypeCode.String;
            if( t == typeof(int) )              return CLRTypeCode.Int32;
            if( t == typeof(DateTime) )         return CLRTypeCode.DateTime;
            if( t == typeof(double) )           return CLRTypeCode.Double;
            if( t == typeof(decimal) )          return CLRTypeCode.Decimal;

            if( t == typeof(bool) )             return CLRTypeCode.Boolean;
            if( t == typeof(DateOnly) )         return CLRTypeCode.DateOnly;
            if( t == typeof(TimeOnly) )         return CLRTypeCode.TimeOnly;
            if( t == typeof(long) )             return CLRTypeCode.Int64;
            if( t == typeof(byte[]) )           return CLRTypeCode.ByteArray;

            //Least common data types last.
            if( t == typeof(byte) )             return CLRTypeCode.Byte;
            if( t == typeof(ulong) )            return CLRTypeCode.UInt64;
            if( t == typeof(float) )            return CLRTypeCode.Single;
            if( t == typeof(short) )            return CLRTypeCode.Int16;
            if( t == typeof(ushort) )           return CLRTypeCode.UInt16;
            if( t == typeof(uint) )             return CLRTypeCode.UInt32;
            if( t == typeof(sbyte) )            return CLRTypeCode.SByte;
            if( t == typeof(char) )             return CLRTypeCode.Char;
            if( t == typeof(TimeSpan) )         return CLRTypeCode.TimeSpan;
            if( t == typeof(Guid) )             return CLRTypeCode.Guid;
            if( t == typeof(DateTimeOffset) )   return CLRTypeCode.DateTimeOffset;

            if( t.IsValueType )                 return CLRTypeCode.Object;

            return CLRTypeCode.Object;
        }
    }
}