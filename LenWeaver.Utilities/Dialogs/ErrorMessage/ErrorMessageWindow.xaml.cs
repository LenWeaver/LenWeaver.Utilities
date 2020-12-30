using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace LenWeaver.Utilities {

    internal partial class ErrorMessageWindow : Window {


        public ErrorMessageWindow() {

            InitializeComponent();
        }


        internal string ErrorMessageText {
            get{ return txtErrorMessage.Text; }
            set{ txtErrorMessage.Text = value; }
        }
        internal string ExceptionType {
            get{ return lblExceptionType?.Content?.ToString() ?? String.Empty; }
            set{ lblExceptionType.Content = value; }
        }

        internal void AddException( Exception ex ) {

            bool                    firstStack  = true;

            NamedValue<string>      nv;


            try {
                lstExceptions.Items.Add( new ErrorMessageItem( "Message:",      ex.Message ) );
                lstExceptions.Items.Add( new ErrorMessageItem( "Type:",         StringHelpers.PascalCaseToDisplayString( ex.GetType().Name ) ) );
                lstExceptions.Items.Add( new ErrorMessageItem( "Source:",       ex?.Source ?? String.Empty ) );

                foreach( string s in ex!.StackTrace!.Split( '\n' ) ) {
                    if( firstStack ) {
                        nv = new NamedValue<string>( "Stack Trace:", s.Replace( '\n', ' ' ).Trim() );

                        firstStack = false;
                    }
                    else {
                        nv = new NamedValue<string>( String.Empty, s.Replace( '\n', ' ' ).Trim() );
                    }

                    lstExceptions.Items.Add( new ErrorMessageItem( nv.Name, nv.Value ) );
                }

                lstExceptions.Items.Add( new Separator() );
            }
            catch( Exception e ) {
                MessageBox.Show( this, e.Message );
            }
        }

        internal string GetExceptionDetails() {

            StringBuilder       sb;


            sb = new StringBuilder();

            for( int index = 0; index < lstExceptions.Items.Count; index++ ) {
                if( lstExceptions.Items[index] is ErrorMessageItem item ) {
                    sb.AppendLine( $"{item.ItemName,14} {item.ItemValue}" );
                }
            }

            return sb.ToString();
        }

        private void btnCopy_Click( object sender, RoutedEventArgs e ) {

            try {
                Clipboard.SetText( GetExceptionDetails() );
            }
            catch( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }
        private void btnOk_Click( object sender, RoutedEventArgs e ) {

            this.Close();
        }
        private void btnSave_Click( object sender, RoutedEventArgs e ) {

            SaveFileDialog  sfd;


            try {
                sfd             = new SaveFileDialog();
                sfd.Filter      = "Log Files (*.log)|*.log|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title       = "Save Exception Details";
                sfd.FileName    = "Exception.log";

                if( sfd.ShowDialog() ?? false ) {
                    File.AppendAllText( sfd.FileName, GetExceptionDetails() );
                }
            }
            catch( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }
    }
}