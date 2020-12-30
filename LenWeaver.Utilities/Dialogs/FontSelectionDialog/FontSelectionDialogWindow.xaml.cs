using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class FontSelectionDialogWindow : Window {

        private readonly    ObservableCollection<double>        fontSizes;


        public FontSelectionDialogWindow() {

            InitializeComponent();

            fontSizes = new ObservableCollection<double>();

            for( double fontSize = 5d; fontSize < 73d; fontSize++ ) {
                fontSizes.Add( fontSize );
            }
        }


        private void AdjustSampleText() {

            FamilyTypeface?         ftf;
            FontFamily              ff;


            if( lstFontFamily.SelectedItem != null && lvwFontAttributes.SelectedItem != null ) {
                ff                  = (FontFamily)lstFontFamily.SelectedItem;
                ftf                 = (FamilyTypeface)lvwFontAttributes.SelectedItem;

                btnSelect.IsEnabled = true;
            }
            else {
                ff                  = new FontFamily( "Segoe UI" );
                ftf                 = null;

                btnSelect.IsEnabled = false;
            }

            tbSample.FontFamily     = ff;
            tbSample.FontStyle      = ftf != null ? ftf.Style   : FontStyles.Normal;
            tbSample.FontStretch    = ftf != null ? ftf.Stretch : FontStretches.Normal;
            tbSample.FontWeight     = ftf != null ? ftf.Weight  : FontWeights.Normal;
        }


        private void btnClose_Click                     ( object sender, RoutedEventArgs e ) {

            DialogResult = false;
            Close();
        }
        private void btnSelect_Click                    ( object sender, RoutedEventArgs e ) {

            DialogResult = true;
            Close();
        }
        private void lstFontFamily_SelectionChanged     ( object sender, SelectionChangedEventArgs e ) {

            if( e.AddedItems.Count == 1 ) {
                if( e.AddedItems[0] is FontFamily ff ) {
                    lvwFontAttributes.ItemsSource   = ff.FamilyTypefaces;
                }
            }

            AdjustSampleText();
        }
        private void lvwFontAttributes_SelectionChanged ( object sender, SelectionChangedEventArgs e ) {

            AdjustSampleText();
        }


    }
}