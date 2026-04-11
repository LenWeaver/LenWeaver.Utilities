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

using LenWeaver.Utilities;

namespace LenWeaver.Utilities {

    internal partial class PrintPreviewWindow : Window {

        public PrintPreviewWindow() {

            InitializeComponent();
        }

        private void btnExit_Click( object sender, RoutedEventArgs e ) {

            this.Close();
        }
        private void btnZoomIn_Click( object sender, RoutedEventArgs e ) {

            try {
                fdr.IncreaseZoom();
            }
            catch( Exception ex ) {
                ErrorMessage.Show( this, ex );
            }
        }
        private void btnZoomOut_Click( object sender, RoutedEventArgs e ) {

            try {
                fdr.DecreaseZoom();
            }
            catch( Exception ex ) {
                ErrorMessage.Show( this, ex );
            }
        }
    }
}