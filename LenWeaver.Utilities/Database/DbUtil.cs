using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace LenWeaver.Utilities {

    public sealed class DbUtil : IDisposable {

        private DbConnection           con;
        private DbProviderFactory      factory;
        

        public DbUtil( DbProviderFactory factory, string connectionString ) {

            bool    connectionOpened        = false;


            try {
                if( !factory.CanCreateDataAdapter ) throw new ArgumentException( "Passed Db Factory must be able to create data adapters." );

                this.factory                = factory;
                this.con                    = factory.CreateConnection();
                this.con.ConnectionString   = connectionString;

                this.con.Open();
                connectionOpened            = true;
            }
            catch( Exception ex ) {
                throw new DatabaseException( "Unable to open database.", ex );
            }
            finally {
                if( connectionOpened ) con.Close();
            }
            
        }
        public DbUtil( DbConnection con ) {

            bool    connectionOpened        = false;


            try {
                this.factory                = DbProviderFactories.GetFactory( con );
                if( !factory.CanCreateDataAdapter ) throw new ArgumentException( "Passed Db Factory must be able to create data adapters." );

                this.con                    = con;

                this.con.Open();
                connectionOpened            = true;
            }
            catch( Exception ex ) {
                throw new DatabaseException( "Unable to open database.", ex );

            }
            finally {
                if( connectionOpened ) con.Close();
            }
        }


        public  DbConnection        Connection  => con;
        public  DbProviderFactory   Factory     => factory;


        private void            ExecuteNonQueryInternal     ( DbTransaction? transaction, params string[] commands ) {

            bool        alreadyOpened   = true;

            DbCommand?  cmd             = null;


            try {
                if( con.State != ConnectionState.Open ) {
                    con.Open();
                    alreadyOpened       = false;
                }

                cmd                     = con.CreateCommand();
                cmd.CommandTimeout      = con.ConnectionTimeout;
                cmd.CommandType         = CommandType.Text;
                cmd.Transaction         = transaction;

                foreach( string s in commands ) {
                    cmd.CommandText     = s;
                    cmd.ExecuteNonQuery();
                }
            }
            finally {
                if( cmd != null ) cmd.Dispose();
                if( con.State != ConnectionState.Closed && !alreadyOpened ) con.Close();
            }
        }
        private object          ExecuteScalarInternal       ( DbTransaction? transaction, string command ) {

            bool        alreadyOpened   = true;

            DbCommand?  cmd             = null;

            object      result;


            try {
                if( con.State != ConnectionState.Open ) {
                    con.Open();
                    alreadyOpened       = false;
                }

                cmd                     = con.CreateCommand();

                cmd.CommandType         = CommandType.Text;
                cmd.CommandTimeout      = con.ConnectionTimeout;
                cmd.CommandText         = command;
                cmd.Transaction         = transaction;

                result                  = cmd.ExecuteScalar();
            }
            finally {
                if( cmd != null ) cmd.Dispose();
                if( con.State != ConnectionState.Closed && !alreadyOpened ) con.Close();
            }

            return result;
        }
        private DataSet         ExecuteInternal             ( DbTransaction? transaction, params string[] commands ) {

            bool                alreadyOpened   = false;

            DataSet             result          = new DataSet();
            DataTable           dt;
            DbCommand?          command         = null;
            DbDataAdapter?      adapter         = null;


            try {
                if( con.State != ConnectionState.Open ) {
                    alreadyOpened               = true;
                    con.Open();
                }

                foreach( string s in commands ) {
                    dt                          = new DataTable();
                    adapter                     = factory.CreateDataAdapter();
                    command                     = factory.CreateCommand();

                    command.CommandText         = s;
                    command.CommandType         = CommandType.Text;
                    command.Connection          = con;
                    command.Transaction         = transaction;

                    adapter.SelectCommand       = command;
                    adapter.Fill( dt );

                    result.Tables.Add( dt );
                }
            }
            catch( Exception ex ) {
                throw new DatabaseException( "Unable to execute queries.", ex );
            }
            finally {
                if( command != null ) command.Dispose();
                if( adapter != null ) adapter.Dispose();
                if( con.State != ConnectionState.Closed && !alreadyOpened ) con.Close();
            }

            return result;
        }
        
        public DbTransaction    BeginTransaction            ( IsolationLevel isolationLevel ) {

            if( con.State != ConnectionState.Open ) con.Open();

            return con.BeginTransaction( isolationLevel );
        }
        public DbTransaction    BeginTransaction            () {

            return BeginTransaction( IsolationLevel.ReadCommitted );
        }


        public void             ExecuteNonQuery             ( DbTransaction transaction, params string[] commands ) {

            ExecuteNonQueryInternal( transaction, commands );
        }
        public void             ExecuteNonQuery             ( params string[] commands ) {

            ExecuteNonQueryInternal( null, commands );
        }

        public object           ExecuteScalar               ( DbTransaction transaction, string command ) {

            return ExecuteScalarInternal( transaction, command );
        }
        public object           ExecuteScalar               ( string command ) {

            return ExecuteScalarInternal( null, command );
        }

        public T                ExecuteScalar<T>            ( DbTransaction transaction, string command, T defaultValue ) {

            T       result = defaultValue;

            object  o;


            o = ExecuteScalar( transaction, command );
            if( o != null && o != DBNull.Value ) {
                result = (T)Convert.ChangeType( o, typeof( T ) );
            }

            return result;
        }
        public T                ExecuteScalar<T>            ( string command, T defaultValue ) {

            T       result = defaultValue;

            object  o;


            o = ExecuteScalar( command );
            if( o != null && o != DBNull.Value ) {
                result = (T)Convert.ChangeType( o, typeof( T ) );
            }

            return result;
        }

        public DataSet          Execute                     ( DbTransaction transaction, params string[] commands ) {

            return ExecuteInternal( transaction, commands );
        }
        public DataSet          Execute                     ( params string[] commands ) {

            return ExecuteInternal( null, commands );
        }


        public void Dispose() {
            
            if( con.State != ConnectionState.Closed ) {
                con.Close();
            }
        }
    }
}