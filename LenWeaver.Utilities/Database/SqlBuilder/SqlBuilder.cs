using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class SqlBuilder {

        private         StringBuilder           sql                         = new StringBuilder();


        public          bool                    ReturnsIdentity             { get; set; }       = false;

        public          SqlAction               Action                      { get; set; }

        public          string                  Name                        { get; set; }

        public          IDatabaseParameters     DatabaseParameters          { get; set; }

        public          SqlParameterCollection  Parameters                  { get; }            = new SqlParameterCollection();
        public          SqlParameterCollection  Where                       { get; }            = new SqlParameterCollection();


        /// <summary>
        /// Initializes a new instance of the SqlBuilder class.
        /// </summary>
        /// <param name="databaseType">The type of database engine being employed.</param>
        /// <param name="action">The action the generated sql will perform.</param>
        /// <param name="name">The entity (typically a database, table or stored procedure) that is the subject of the specified SqlAction.</param>
        public SqlBuilder( IDatabaseParameters databaseParameters, SqlAction action, string name ) {

            DatabaseParameters  = databaseParameters;
            Action              = action;
            Name                = name;
        }
        /// <summary>
        /// Initializes a new instance of the SqlBuilder class.
        /// </summary>
        /// <param name="action">The action the generated sql will perform.</param>
        /// <param name="name">The entity (typically a database, table or stored procedure) that is the subject of the specified SqlAction.</param>
        public SqlBuilder( SqlAction action, string name ) : this( SqlBuilder.DefaultDatabaseParameters, action, name ) {}


        public void Clear() {

            sql.Clear();
            Parameters.Clear();
            Where.Clear();
        }

        #region Sql Generator Methods
        private string GenerateCreateDatabase() {

            return DatabaseParameters.CreateDatabaseTemplate.Replace( "@DATABASE_NAME@", Name );
        }
        private string GenerateCreateTable() {

            return DatabaseParameters.CreateTableTemplate.Replace( "@TABLE_NAME@", Name );
        }
        private string GenerateSqlDelete() {

            if( Parameters.Count == 0 ) throw new InvalidOperationException( "Parameters collection is empty." );

            sql.Clear();
            sql.Append( DatabaseParameters.DeleteCommandTemplate );
            sql.Replace( "@TABLE_NAME@", Name );
            sql.AppendFormat( "DELETE FROM {0} WHERE", Name );

            for( int i = 0; i < Where.Count; i++ ) {
                if( i != 0 ) sql.Append( " AND" );

                sql.AppendFormat( " {0} = {1}", Where[i].Name, SqlBuilder.ToSql( Where[i] ) );
            }

            return sql.ToString();
        }
        private string GenerateSqlInsert() {

            StringBuilder       fields;
            StringBuilder       values;


            fields      = new StringBuilder();
            values      = new StringBuilder();

            foreach( SqlParameter p in Parameters ) {
                if( fields.Length > 0 ) fields.Append( ',' );
                fields.Append( p.Name );

                if( values.Length > 0 ) values.Append( ',' );
                values.Append( ToSql( p ) );
            }

            sql.Clear();
            sql.Append( DatabaseParameters.InsertCommandTemplate );
            sql.Replace( "@TABLE_NAME@",    Name );
            sql.Replace( "@TABLE_FIELDS@",  fields.ToString() );
            sql.Replace( "@TABLE_VALUES@",  values.ToString() );

            if( ReturnsIdentity ) sql.Append( DatabaseParameters.ReturnsIdentityTemplate );

            return sql.ToString();
        }
        private string GenerateSqlSelect() {

            return "Select";
        }
        private string GenerateSqlUpdate() {

            return "Update";
        }
        #endregion
        #region Static ToSql Methods
        public static string ToSql( SqlParameter p, IDatabaseParameters databaseParameters ) {

            DateTime    dt;

            string      result   = String.Empty;


            switch( p.TypeCode ) {
                case DbTypeCode.Boolean:
                case DbTypeCode.Char:
                case DbTypeCode.SByte:
                case DbTypeCode.Byte:
                case DbTypeCode.Int16:
                case DbTypeCode.UInt16:
                case DbTypeCode.Int32:
                case DbTypeCode.UInt32:
                case DbTypeCode.Int64:
                case DbTypeCode.UInt64:
                case DbTypeCode.Single:
                case DbTypeCode.Double:
                case DbTypeCode.Decimal:
                case DbTypeCode.Object:
                    result          = SqlBuilder.ToSql( p.Value );
                    break;

                case DbTypeCode.String:
                    if( p.Value != null ) {
                        result      = $"{databaseParameters.OpeningStringQuote}" +
                                      p.Value.ToString() +
                                      $"{databaseParameters.ClosingStringQuote}";
                    }
                    else {
                        result      = databaseParameters.NullTemplate;
                    }
                    break;

                case DbTypeCode.Empty:
                    break;
                case DbTypeCode.DateTime:
                    if( p.Value != null ) {
                        result      = $"{databaseParameters.OpeningDateQuote}" +
                                      ((DateTime)p.Value).ToString( databaseParameters.DateTimeFormat ) +
                                      $"{databaseParameters.ClosingDateQuote}";
                    }
                    else {
                        result      = databaseParameters.NullTemplate;
                    }
                    break;
                case DbTypeCode.Date:
                    if( p.Value != null ) {
                        result      = $"{databaseParameters.OpeningDateQuote}" +
                                      ((DateTime)p.Value).ToString( databaseParameters.DateFormat ) +
                                      $"{databaseParameters.ClosingDateQuote}";
                    }
                    else {
                        result      = databaseParameters.NullTemplate;
                    }
                    break;
                case DbTypeCode.Time:
                    if( p.Value != null ) {
                        if( p.Value is DateTime ) {
                            dt      = (DateTime)p.Value;

                            result  = $"{databaseParameters.OpeningDateQuote}" +
                                      dt.ToString( databaseParameters.TimeFormat ) +
                                      $"{databaseParameters.ClosingDateQuote}";
                        }
                        else if( p.Value is TimeSpan ts ) {
                            dt      = new DateTime( ts.Ticks );
                            
                            result  = $"{databaseParameters.OpeningDateQuote}" +
                                      dt.ToString( databaseParameters.TimeFormat ) +
                                      $"{databaseParameters.ClosingDateQuote}";
                        }
                        else {
                            throw new ArgumentException( "DateTime or TimeSpan type expected." );
                        }
                    }
                    else {
                        result      = databaseParameters.NullTemplate;
                    }
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException( $"Unknown value for '{nameof( p.TypeCode )}'. " );
            }

            return result;
        }
        public static string ToSql( SqlParameter p ) {

            return ToSql( p, SqlBuilder.DefaultDatabaseParameters );
        }

        public static string ToSql( DateTime dt )                   => $"{DefaultDatabaseParameters.OpeningDateQuote}" +
                                                                       $"{dt.ToString( DateTimeHelpers.ISO8601DateTimeFormat )}" +
                                                                       $"{DefaultDatabaseParameters.ClosingDateQuote}";
        public static string ToSql( TimeSpan ts )                   => $"{DefaultDatabaseParameters.OpeningDateQuote}" +
                                                                       ts.ToString( "hhmmss" ) +
                                                                       $"{DefaultDatabaseParameters.ClosingDateQuote}";
        
        public static string ToSql<T>( T? value ) where T : struct  => value?.ToString() ?? "NULL";
        public static string ToSql<T>( T? value ) where T : class   => value?.ToString() ?? "NULL";
        #endregion
        #region Other Static Members
        private static  IDatabaseParameters?    defaultDatabaseParameters   = null;

        public static IDatabaseParameters       DefaultDatabaseParameters {
            get {
                if( defaultDatabaseParameters == null ) {
                    defaultDatabaseParameters = new SqlServerParameters();
                }

                return defaultDatabaseParameters;
            }
            set => defaultDatabaseParameters = value;
        }
        #endregion

        public override string ToString() {

            string      result;


            switch( Action ) {
                //case SqlAction.CreateDatabase:  result = GenerateCreateDatabase();      break;
                //case SqlAction.CreateTable:     result = GenerateCreateTable();         break;
                case SqlAction.Delete:          result = GenerateSqlDelete();           break;
                case SqlAction.Insert:          result = GenerateSqlInsert();           break;
                case SqlAction.Select:          result = GenerateSqlSelect();           break;
                case SqlAction.Update:          result = GenerateSqlUpdate();           break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException( $"Unknown value for {nameof(Action)} property." );
            }

            return result;
        }
    }
}