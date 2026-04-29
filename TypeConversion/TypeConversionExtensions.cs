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

        public static string ToSqlLiteral( this object? value, ITypeConversionService types ) {

            if( value is null ) return "NULL";

            ITypeHandler? handler = types.GetHandler( value.GetType() );
            string? text = handler.ToText( value );

            // For simplicity: quote and escape text
            if( handler.IsBlob ) throw new NotSupportedException( "Literal BLOBs not supported here." );

            if( text is null ) return "NULL";

            // Basic escaping for SQLite-style single quotes
            string? escaped = text.Replace( "'", "''" );
            return $"'{escaped}'";
        }
    }
}