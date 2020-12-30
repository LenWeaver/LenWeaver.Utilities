using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class SqlParameter {

        public TypeCode     TypeCode        { get; set; }
        public TypeSubCode  TypeSubCode     { get; set; }

        public string       Name            { get; set; }

        public object?      Value           { get; set; }


        internal SqlParameter( string name, TypeCode typeCode, TypeSubCode typeSubCode, object? value ) {

            Name            = name;
            TypeCode        = typeCode;
            TypeSubCode     = typeSubCode;
            Value           = value;
        }
        internal SqlParameter( string name, TypeCode typeCode, object? value )      : this( name, typeCode,             TypeSubCode.Empty,      value ) {}

        public SqlParameter( string name, bool? value )                             : this( name, TypeCode.Boolean,                             value ) {}
        public SqlParameter( string name, byte? value )                             : this( name, TypeCode.Byte,                                value ) {}
        public SqlParameter( string name, sbyte? value )                            : this( name, TypeCode.SByte,                               value ) {}
        public SqlParameter( string name, char? value )                             : this( name, TypeCode.Char,                                value ) {}
        public SqlParameter( string name, short? value )                            : this( name, TypeCode.Int16,                               value ) {}
        public SqlParameter( string name, ushort? value )                           : this( name, TypeCode.UInt16,                              value ) {}
        public SqlParameter( string name, int? value )                              : this( name, TypeCode.Int32,                               value ) {}
        public SqlParameter( string name, uint? value )                             : this( name, TypeCode.UInt32,                              value ) {}
        public SqlParameter( string name, long? value )                             : this( name, TypeCode.Int64,                               value ) {}
        public SqlParameter( string name, ulong? value )                            : this( name, TypeCode.UInt64,                              value ) {}
        public SqlParameter( string name, float? value )                            : this( name, TypeCode.Single,                              value ) {}
        public SqlParameter( string name, double? value )                           : this( name, TypeCode.Double,                              value ) {}
        public SqlParameter( string name, decimal? value )                          : this( name, TypeCode.Decimal,                             value ) {}
        public SqlParameter( string name, string? value )                           : this( name, TypeCode.String,                              value ) {}

        public SqlParameter( string name, DateTime? value )                         : this( name, TypeCode.DateTime,                            value ) {}
        public SqlParameter( string name, DateTime? value, TypeSubCode subCode )    : this( name, TypeCode.DateTime,    subCode,                value ) {}
        public SqlParameter( string name, TimeSpan? value )                         : this( name, TypeCode.Empty,       TypeSubCode.TimeOnly,   value ) {}

        public SqlParameter( string name, byte[] value )                            : this( name, TypeCode.Byte,        TypeSubCode.Array,      value ) {}
    }
}
