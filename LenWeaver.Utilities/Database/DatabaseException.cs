using System;
using System.Collections.Generic;
using System.Data;

namespace LenWeaver.Utilities {


    [Serializable]
    public class DatabaseException : Exception {

        public string DatabaseName      { get; set; }   = String.Empty;


        public DatabaseException() {}
        public DatabaseException( Exception ex ) : this( ex.Message, ex ) {}
        public DatabaseException( string message ) : base( message ) {}
        public DatabaseException( IDbConnection con, string message ) : this( message ) {
        
            DatabaseName    = con.Database;
        }
        public DatabaseException( IDbConnection con, Exception ex ) : this( con, ex.Message, ex ) {}
        public DatabaseException( string message, Exception inner ) : base( message, inner ) {}
        public DatabaseException( IDbConnection con, string message, Exception inner ) : this( message, inner ) {

            DatabaseName    = con.Database;
        }
    }    
}