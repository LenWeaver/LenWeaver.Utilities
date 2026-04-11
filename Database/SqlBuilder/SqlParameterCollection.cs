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

        public SqlParameter Add( string name, bool? value )                             => Add( new SqlParameter( name, DbTypeCode.Boolean,                           value ) );
        public SqlParameter Add( string name, byte? value )                             => Add( new SqlParameter( name, DbTypeCode.Byte,                              value ) );
        public SqlParameter Add( string name, sbyte? value )                            => Add( new SqlParameter( name, DbTypeCode.SByte,                             value ) );
        public SqlParameter Add( string name, char? value )                             => Add( new SqlParameter( name, DbTypeCode.Char,                              value ) );
        public SqlParameter Add( string name, short? value )                            => Add( new SqlParameter( name, DbTypeCode.Int16,                             value ) );
        public SqlParameter Add( string name, ushort? value )                           => Add( new SqlParameter( name, DbTypeCode.UInt16,                            value ) );
        public SqlParameter Add( string name, int? value )                              => Add( new SqlParameter( name, DbTypeCode.Int32,                             value ) );
        public SqlParameter Add( string name, uint? value )                             => Add( new SqlParameter( name, DbTypeCode.UInt32,                            value ) );
        public SqlParameter Add( string name, long? value )                             => Add( new SqlParameter( name, DbTypeCode.Int64,                             value ) );
        public SqlParameter Add( string name, ulong? value )                            => Add( new SqlParameter( name, DbTypeCode.UInt64,                            value ) );
        public SqlParameter Add( string name, float? value )                            => Add( new SqlParameter( name, DbTypeCode.Single,                            value ) );
        public SqlParameter Add( string name, double? value )                           => Add( new SqlParameter( name, DbTypeCode.Double,                            value ) );
        public SqlParameter Add( string name, decimal? value )                          => Add( new SqlParameter( name, DbTypeCode.Decimal,                           value ) );
        public SqlParameter Add( string name, DateTime? value )                         => Add( new SqlParameter( name, DbTypeCode.DateTime,                          value ) );
        public SqlParameter Add( string name, string value )                            => Add( new SqlParameter( name, DbTypeCode.String,                            value ) );
                                                                                                                        
    
        public void Clear() => base.inner.Clear();
    }
}