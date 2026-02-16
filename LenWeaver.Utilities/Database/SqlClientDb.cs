using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace LenWeaver.Utilities {

    public class SqlClientDb : IDatabase {

        private readonly    SqlConnection       con;


        public              IDbConnection       Connection          => con;


        public SqlClientDb( SqlConnection con ) {

            try {
                this.con = con;

                con.Open( SqlConnectionOverrides.OpenWithoutRetry );
                con.Close();
            }
            catch( Exception ex ) {
                throw new DatabaseException( $"{nameof(SqlClientDb)} constructor failed.", ex );
            }
        }
        public SqlClientDb( string connectionString ) : this( new SqlConnection( connectionString ) ) {}
        public SqlClientDb( string connectionString, SqlCredential credential ) : this( new SqlConnection( connectionString, credential ) ) {}


        #region Internal Implementation Methods
        protected internal void ExecuteNonQueryInternal( IDbTransaction? transaction, string? command, string[]? commands ) {

            SqlCommand          sqlCommand;


            if( command != null ) {
                sqlCommand                  = con.CreateCommand();
                sqlCommand.CommandType      = CommandType.Text;
                sqlCommand.CommandText      = command;
                sqlCommand.ExecuteNonQuery();
            }
            else if( commands != null ) {
            }
        }
        
        protected internal DataSet ExecuteInternal( IDbTransaction? transaction, string? command, string[]? commands ) {

            DataSet             result;
            DataTable           dt;
            SqlDataAdapter      adapter;


            result                          = new DataSet();

            if( command != null ) {
                adapter                     = new SqlDataAdapter( command, con );
                adapter.Fill( result );
            }
            else if( commands != null ) {
                foreach( string s in commands! ) {
                    dt                      = new DataTable();

                    adapter                 = new SqlDataAdapter( s, con );
                    adapter.Fill( dt );

                    result.Tables.Add( dt );
                }
            }

            return result;
        }
        #endregion


        public void             ExecuteNonQuery( IDbTransaction transaction, string command ) {
            throw new NotImplementedException();
        }
        public void             ExecuteNonQuery( IDbTransaction transaction, params string[] commands ) {
            throw new NotImplementedException();
        }
        public void             ExecuteNonQuery( string command ) {
            throw new NotImplementedException();
        }
        public void             ExecuteNonQuery( params string[] commands ) {
            throw new NotImplementedException();
        }

        public object           ExecuteScalar( IDbTransaction transaction, string command ) {
            throw new NotImplementedException();
        }
        public object           ExecuteScalar( string command ) {
            throw new NotImplementedException();
        }

        public T                ExecuteScalar<T>( IDbTransaction transaction, string command, T defaultValue ) {
            throw new NotImplementedException();
        }
        public T                ExecuteScalar<T>( string command, T defaultValue ) {
            throw new NotImplementedException();
        }

        public DataSet          Execute( IDbTransaction transaction, string command ) {
            
            return ExecuteInternal( transaction, command, null );
        }
        public DataSet          Execute( IDbTransaction transaction, string[] commands ) {
            
            return ExecuteInternal( transaction, null, commands );
        }
        public DataSet          Execute( string command ) {
            
            return ExecuteInternal( null, command, null );
        }
        public DataSet          Execute( params string[] commands ) {
            
            return ExecuteInternal( null, null, commands );
        }

        public IDbTransaction   BeginTransaction() {
        
            return con.BeginTransaction();
        }
    }
}