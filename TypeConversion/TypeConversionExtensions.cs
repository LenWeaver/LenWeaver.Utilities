using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LenWeaver.Utilities {

    public static class TypeConversionExtensions {

        private static ITypeHandler[] allHandlers   = Array.Empty<ITypeHandler>();

        //Any new non-generic handlers added to LINK:CLRTypeHandlers.cs
        //should have their compile-time type added to AllHandlers.
        public static ITypeHandler[] AllHandlers {
            get {
                if( allHandlers.Length == 0 ) {
                    allHandlers  = [new BooleanHandler(),       new CharHandler(),          new ByteHandler(),
                                    new SByteHandler(),         new Int16Handler(),         new UInt16Handler(),
                                    new Int32Handler(),         new UInt32Handler(),        new Int64Handler(),
                                    new UInt64Handler(),        new SingleHandler(),        new DoubleHandler(),
                                    new DecimalHandler(),       new StringHandler(),        new DateTimeHandler(),
                                    new DateOnlyHandler(),      new TimeOnlyHandler(),      new GuidHandler(),
                                    new ByteArrayHandler(),     new FontDescriptorHandler()];
                }

                return allHandlers;
            }
        }

        public static   Type[]  xAllHandlers     = [typeof(BooleanHandler),      typeof(CharHandler),        typeof(ByteHandler),        typeof(SByteHandler),
                                                   typeof(Int16Handler),        typeof(UInt16Handler),      typeof(Int32Handler),       typeof(UInt32Handler),
                                                   typeof(Int64Handler),        typeof(UInt64Handler),      typeof(SingleHandler),      typeof(DoubleHandler),
                                                   typeof(DecimalHandler),      typeof(StringHandler),      typeof(DateTimeHandler),    typeof(DateOnlyHandler),
                                                   typeof(TimeOnlyHandler),     typeof(GuidHandler),        typeof(ByteArrayHandler),   typeof(FontDescriptorHandler)];

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

    }
}