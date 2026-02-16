using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    internal partial class ConnectionStringDialogWindow : Window {

        public ConnectionStringDialogWindow() {

            InitializeComponent();
        }


        private void CommandClose_CanExecute( object sender, CanExecuteRoutedEventArgs e ) {

            e.CanExecute = true;
        }
        private void CommandClose_Executed( object sender, ExecutedRoutedEventArgs e ) {

            this.Close();
        }
        private void CommandSave_CanExecute( object sender, CanExecuteRoutedEventArgs e ) {

            e.CanExecute = true;
        }
        private void CommandSave_Executed( object sender, ExecutedRoutedEventArgs e ) {

        }
    }
}