using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class SqlParameterCollection : SimpleCollectionBase<SqlParameter> {

        internal SqlParameterCollection() : base() {}


        public SqlParameter Add( SqlParameter param ) {
        
            base.inner.Add( param );

            return param;
        }

        public SqlParameter Add( string name, bool? value )                             => Add( new SqlParameter( name, TypeCode.Boolean,                           value ) );
        public SqlParameter Add( string name, byte? value )                             => Add( new SqlParameter( name, TypeCode.Byte,                              value ) );
        public SqlParameter Add( string name, sbyte? value )                            => Add( new SqlParameter( name, TypeCode.SByte,                             value ) );
        public SqlParameter Add( string name, char? value )                             => Add( new SqlParameter( name, TypeCode.Char,                              value ) );
        public SqlParameter Add( string name, short? value )                            => Add( new SqlParameter( name, TypeCode.Int16,                             value ) );
        public SqlParameter Add( string name, ushort? value )                           => Add( new SqlParameter( name, TypeCode.UInt16,                            value ) );
        public SqlParameter Add( string name, int? value )                              => Add( new SqlParameter( name, TypeCode.Int32,                             value ) );
        public SqlParameter Add( string name, uint? value )                             => Add( new SqlParameter( name, TypeCode.UInt32,                            value ) );
        public SqlParameter Add( string name, long? value )                             => Add( new SqlParameter( name, TypeCode.Int64,                             value ) );
        public SqlParameter Add( string name, ulong? value )                            => Add( new SqlParameter( name, TypeCode.UInt64,                            value ) );
        public SqlParameter Add( string name, float? value )                            => Add( new SqlParameter( name, TypeCode.Single,                            value ) );
        public SqlParameter Add( string name, double? value )                           => Add( new SqlParameter( name, TypeCode.Double,                            value ) );
        public SqlParameter Add( string name, decimal? value )                          => Add( new SqlParameter( name, TypeCode.Decimal,                           value ) );

        public SqlParameter Add( string name, DateTime? value )                         => Add( new SqlParameter( name, TypeCode.DateTime,                          value ) );
        public SqlParameter Add( string name, DateTime? value, TypeSubCode subCode )    => Add( new SqlParameter( name, TypeCode.DateTime,  subCode,                value ) );
        public SqlParameter Add( string name, TimeSpan? value )                         => Add( new SqlParameter( name, TypeCode.Empty,     TypeSubCode.TimeOnly,   value ) );

        public SqlParameter Add( string name, string value )                            => Add( new SqlParameter( name, TypeCode.String,                            value ) );

        public SqlParameter Add( string name, byte[] value )                            => Add( new SqlParameter( name, TypeCode.Byte,      TypeSubCode.Array,      value ) );
    
        public void Clear() => base.inner.Clear();
        }
}