using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public partial class FontSelectionDialogWindow : Window {


        public FontSelectionDialogWindow() {

            FontFamilyComparer  comparer    = new FontFamilyComparer();


            InitializeComponent();


            for( double fontSize = 5d; fontSize < 73d; fontSize++ ) {
                cboFontSize.Items.Add( fontSize );
            }

            lstFontFamily.ItemsSource = Fonts.SystemFontFamilies.Order( comparer );
        }


        private void AdjustSampleText() {

            FamilyTypeface?                 ftf;
            FontFamily                      ff;


            if( lstFontFamily.SelectedItem != null && lvwFontAttributes.SelectedItem != null ) {
                ff                          = (FontFamily)lstFontFamily.SelectedItem;
                ftf                         = (FamilyTypeface)lvwFontAttributes.SelectedItem;

                btnSelect.IsEnabled         = true;
            }
            else {
                ff                          = FontExtensions.SegoeUI;
                ftf                         = null;

                btnSelect.IsEnabled         = false;
            }

            tbSample.FontFamily             = ff;
            tbSample.FontStyle              = ftf != null ? ftf.Style   : FontStyles.Normal;
            tbSample.FontStretch            = ftf != null ? ftf.Stretch : FontStretches.Normal;
            tbSample.FontWeight             = ftf != null ? ftf.Weight  : FontWeights.Normal;
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