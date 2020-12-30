using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class SqlParameter {

        public DbTypeCode   TypeCode        { get; set; }

        public string       Name            { get; set; }

        public object?      Value           { get; set; }


        public SqlParameter( string name, DbTypeCode typeCode, object? value ) {

            Name            = name;
            TypeCode        = typeCode;
            Value           = value;
        }

        public SqlParameter( string name, bool? value )         : this( name, DbTypeCode.Boolean,       value ) {}
        public SqlParameter( string name, byte? value )         : this( name, DbTypeCode.Byte,          value ) {}
        public SqlParameter( string name, sbyte? value )        : this( name, DbTypeCode.SByte,         value ) {}
        public SqlParameter( string name, char? value )         : this( name, DbTypeCode.Char,          value ) {}
        public SqlParameter( string name, short? value )        : this( name, DbTypeCode.Int16,         value ) {}
        public SqlParameter( string name, ushort? value )       : this( name, DbTypeCode.UInt16,        value ) {}
        public SqlParameter( string name, int? value )          : this( name, DbTypeCode.Int32,         value ) {}
        public SqlParameter( string name, uint? value )         : this( name, DbTypeCode.UInt32,        value ) {}
        public SqlParameter( string name, long? value )         : this( name, DbTypeCode.Int64,         value ) {}
        public SqlParameter( string name, ulong? value )        : this( name, DbTypeCode.UInt64,        value ) {}
        public SqlParameter( string name, float? value )        : this( name, DbTypeCode.Single,        value ) {}
        public SqlParameter( string name, double? value )       : this( name, DbTypeCode.Double,        value ) {}
        public SqlParameter( string name, decimal? value )      : this( name, DbTypeCode.Decimal,       value ) {}
        public SqlParameter( string name, string? value )       : this( name, DbTypeCode.String,        value ) {}
                                                                              
        public SqlParameter( string name, DateTime? value )     : this( name, DbTypeCode.DateTime,      value ) {}
        public SqlParameter( string name, TimeSpan? value )     : this( name, DbTypeCode.Time,          value ) {}
                                                                              
        public SqlParameter( string name, byte[] value )        : this( name, DbTypeCode.Binary,        value ) {}
    }
}
