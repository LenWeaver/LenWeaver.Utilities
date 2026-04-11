using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LenWeaver.Utilities {

    public class LogFile : IDisposable {

        private StreamWriter?   sw      = null;

        public  bool            AutoFlush           { get; set; }
        public  bool            Enabled             { get; set; }
        public  bool            PrefixDateTime      { get; set; }

        public  string          Filename            { get; set; }


        public LogFile( string filename ) {

            AutoFlush           = false;
            Enabled             = true;
            PrefixDateTime      = true;

            Filename            = filename;
        }
        public LogFile() : this( String.Empty ) {}


        public void Write( string message ) {

            try {
                if( Enabled ) {
                    if( sw == null ) {
                        if( String.IsNullOrWhiteSpace( Filename ) ) {
                            throw new ApplicationException( "Filename property has not been set." );
                        }

                        sw  = new StreamWriter( Filename, append: true );
                    }

                    if( PrefixDateTime ) {
                        sw.Write( DateTime.Now.ToString( DateTimeHelpers.ISO8601DateTimeFormat ) );
                        sw.Write( " - " );
                    }

                    sw.WriteLine( message );

                    if( AutoFlush ) {
                        sw.Flush();
                    }
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to write to log file.", ex );
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose( bool disposing ) {

            if( !disposedValue ) {
                if( disposing ) {
                    if( sw != null ) sw.Dispose();
                }

                disposedValue = true;
            }
        }

        ~LogFile() {

            Dispose( false );
        }

        public void Dispose() {

            Dispose( true );

            GC.SuppressFinalize( this );
        }
        #endregion
    }
}