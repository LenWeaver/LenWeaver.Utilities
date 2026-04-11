using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace LenWeaver.Utilities {

    internal partial class FontSelectionWindow : Window {

        internal ComboBox?  cboFontSize     = null;


        public FontSelectionWindow() {

            InitializeComponent();
        }


        public override void OnApplyTemplate() {

            base.OnApplyTemplate();


            if( txtSearch is not null && txtSearch.Visibility == Visibility.Visible ) {
                txtSearch.Focus();
            }
            else {
                lstFontFamily?.Focus();
            }
        }
    }
}