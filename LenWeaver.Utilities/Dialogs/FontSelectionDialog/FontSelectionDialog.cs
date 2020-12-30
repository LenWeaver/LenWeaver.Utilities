using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class FontSelectionDialog {

        private             bool                        allowFontSizeSelection;
        private             bool                        allowTypefaceSelection;

        private             double                      fontSize;

        private             FontStretch                 fontStretch;
        private             FontStyle                   fontStyle;
        private             FontWeight                  fontWeight;
        private             FontFamily?                 fontFamily;

        private readonly    FontSelectionDialogWindow   fontSelection;


        public FontSelectionDialog() {

            allowFontSizeSelection  = true;
            allowTypefaceSelection  = true;

            fontSize                = Double.NaN;

            fontStretch             = FontStretches.Normal;
            fontStyle               = FontStyles.Normal;
            fontWeight              = FontWeights.Normal;
            fontFamily              = null;

            fontSelection           = new FontSelectionDialogWindow();
            fontSelection.Owner     = Application.Current.MainWindow;
        }


        public bool             AllowFontSizeSelection {
            get { return allowFontSizeSelection; }
            set { allowFontSizeSelection = value; }
        }
        public bool             AllowTypefaceSelection {
            get => allowTypefaceSelection;
            set => allowTypefaceSelection = value;
        }
        public double           FontSize {
            get { return fontSize; }
            set { fontSize = value; }
        }
        public string           Title {
            get { return fontSelection.Title; }
            set { fontSelection.Title = value ?? "Font Selection..."; }
        }
        public FontStretch      FontStretch {
            get { return fontStretch; }
            set { fontStretch = value; }
        }
        public FontStyle        FontStyle {
            get { return fontStyle; }
            set { fontStyle = value; }
        }
        public FontWeight       FontWeight {
            get { return fontWeight; }
            set { fontWeight = value; }
        }
        public FontFamily?      FontFamily {
            get { return fontFamily; }
            set { fontFamily = value; }
        }


        public bool? ShowDialog() {

            bool?               result;

            FamilyTypeface?     ftf;


            fontSelection.cboFontSize.Visibility        = allowFontSizeSelection ? Visibility.Visible : Visibility.Collapsed;
            fontSelection.tbFontSize.Visibility         = allowFontSizeSelection ? Visibility.Visible : Visibility.Collapsed;

            fontSelection.lvwFontAttributes.Visibility  = allowTypefaceSelection ? Visibility.Visible : Visibility.Collapsed;

            fontSelection.lstFontFamily.SelectedItem    = fontFamily;
            fontSelection.lstFontFamily.ScrollIntoView( fontFamily );


            if( fontSelection.lstFontFamily.SelectedItem != null ) {
                for( int index = 0; index < fontSelection.lvwFontAttributes.Items.Count; index++ ) {
                    ftf = (FamilyTypeface)fontSelection.lvwFontAttributes.Items[index];

                    if( ftf.Stretch == fontStretch && ftf.Style == fontStyle && ftf.Weight == fontWeight ) {
                        fontSelection.lvwFontAttributes.SelectedItem = ftf;
                        fontSelection.lvwFontAttributes.ScrollIntoView( ftf );
                        break;
                    }
                }
            }

            for( int index = 0; index < fontSelection.cboFontSize.Items.Count; index++ ) {
                if( (double)fontSelection.cboFontSize.Items[index] == fontSize ) {
                    fontSelection.cboFontSize.SelectedIndex = index;
                    break;
                }
            }

            result                  = fontSelection.ShowDialog();

            if( result ?? false ) {
                fontFamily          = fontSelection.lstFontFamily.SelectedItem      as FontFamily;

                if( fontSelection.lvwFontAttributes.SelectedItem != null ) { 
                    ftf             = fontSelection.lvwFontAttributes.SelectedItem  as FamilyTypeface;

                    fontStretch     = ftf!.Stretch;
                    fontStyle       = ftf.Style;
                    fontWeight      = ftf.Weight;
                }
                if( allowFontSizeSelection ) {
                    fontSize        = (double)fontSelection.cboFontSize.SelectedValue;
                }
            }

            return result;
        }
    }
}