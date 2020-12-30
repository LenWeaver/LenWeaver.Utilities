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

            return "Insert";
        }
        private string GenerateSqlSelect() {

            return "Select";
        }
        private string GenerateSqlUpdate() {

            return "Update";
        }
        #endregion
        #region Static ToSql Methods
        public static string ToSql( SqlParameter p ) {

            string result   = String.Empty;


            switch( p.TypeCode ) {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                case TypeCode.Object:
                    result = SqlBuilder.ToSql( p.Value );
                    break;

                case TypeCode.Empty:
                    break;
                case TypeCode.DateTime:
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException( $"Unknown value for '{nameof( p.TypeCode )}'. " );
            }

            return result;
        }

        public static string ToSql( DateTime dt )                   => $"{dt.ToString( DateTimeHelpers.ISO8601DateTimeFormat )}";
        public static string ToSql( TimeSpan ts )                   => ts.ToString( "c" );
        
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
                case SqlAction.CreateTable:     result = GenerateCreateTable();         break;
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