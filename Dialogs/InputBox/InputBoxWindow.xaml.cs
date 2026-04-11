using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LenWeaver.Utilities {

    internal partial class InputBoxWindow : Window {


        public InputBoxWindow() {

            InitializeComponent();

            txtText.Focus();
        }


        private void btnCancel_Click( object sender, RoutedEventArgs e ) {

            this.DialogResult = false;
            this.Close();
        }
        private void btnSave_Click( object sender, RoutedEventArgs e ) {

            this.DialogResult = true;
            this.Close();
        }
    }
}