using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LenWeaver.Utilities {

    public static class ErrorMessage {

        public static Window?                                       DefaultOwner { get; set; } = null;

        public static event EventHandler<ErrorMessageEventArgs>?    ErrorMessageShown;


        public static void Show( Window? owner, string text ) {
        
            Show( owner, text, "An Error Has Occurred!" );
        }
        public static void Show( string text ) {

            Show( (Window?)null, text );
        }
        public static void Show( Window? owner, string text, string caption ) {
        
            ErrorMessageWindow  emw;


            try {
                emw                     = new ErrorMessageWindow();
                emw.Title               = caption;
                emw.ErrorMessageText    = text;

                if( owner != null ) {
                    emw.Owner = owner;
                }
                else if( DefaultOwner != null ) {
                    emw.Owner = DefaultOwner;
                }
                else {
                    emw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

                emw.ShowDialog();
            }
            catch( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }
        public static void Show( string text, string caption ) {

            Show( null, text, caption );
        }
        public static void Show( Window? owner, Exception ex ) {
        
            Show( owner, ex, "An Error Has Occurred!" );
        }
        public static void Show( Exception ex ) {

            Show( null, ex );
        }
        public static void Show( Window? owner, Exception ex, string caption ) {
            
            ErrorMessageEventArgs   args;
            ErrorMessageWindow      emw;
            Exception?              exc     = ex;


            try {
                emw                     = new ErrorMessageWindow();
                emw.Title               = caption;
                emw.ExceptionType       = StringHelpers.PascalCaseToDisplayString( ex.GetType().Name );
                emw.ErrorMessageText    = exc.Message;

                while( exc != null ) {
                    emw.AddException( exc );

                    exc = exc.InnerException;
                }

                if( owner != null ) {
                        emw.Owner = owner;
                }
                else if( DefaultOwner != null ) {
                    emw.Owner = DefaultOwner;
                }
                else {
                    emw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

                emw.ShowDialog();

                if( ErrorMessageShown != null ) {
                    args            = new ErrorMessageEventArgs();
                    args.Exception  = exc;

                    ErrorMessageShown( emw, args );
                }
            }
            catch( Exception excep ) {
                MessageBox.Show( excep.Message );
            }
        }
        public static void Show( Exception ex, string caption ) {

            Show( null, ex, caption );
        }
    }
}