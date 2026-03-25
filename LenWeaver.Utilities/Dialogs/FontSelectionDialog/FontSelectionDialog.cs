using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class FontSelectionDialog {

        public bool                 AllowFontSizeSelection          { get; set; }       = true;
        public bool                 AllowSearch                     { get; set; }       = true;
        public bool                 AllowTypefaceSelection          { get; set; }       = true;
        public bool                 IncludeEmojiFonts               { get; set; }       = false;
        public bool                 IncludeMonospaceFonts           { get; set; }       = true;
        public bool                 IncludeProportionalFonts        { get; set; }       = true;
        public bool                 IncludeSymbolFonts              { get; set; }       = false;

        public string               Title                           { get; set; }       = "Font Selection...";

        public FontDescriptor?      SelectedFont                    { get; set; }       = null;
        public Window?              Owner                           { get; set; }       = null;


        private readonly            FontSelectionWindow             fsw;


        public FontSelectionDialog() {

            fsw = new FontSelectionWindow();
        }


        public bool?                ShowDialog() {

            if( Owner is not null ) fsw.Owner           = Owner;
            fsw.Title                                   = Title;

            fsw.txtSearch.Visibility                    = AllowSearch ? Visibility.Visible : Visibility.Collapsed;

            switch( AllowFontSizeSelection, AllowTypefaceSelection ) {
                case (true, true):
                    fsw.cboFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.cboFontSizeCenter.Visibility    = Visibility.Visible;
                    fsw.grpTypeface.Visibility          = Visibility.Visible;
                    fsw.cboFontSize                     = fsw.cboFontSizeCenter;
                    break;

                case (true, false):
                    fsw.cboFontSizeLeft.Visibility      = Visibility.Visible;
                    fsw.grpTypeface.Visibility          = Visibility.Collapsed;
                    fsw.cboFontSize                     = fsw.cboFontSizeLeft;
                    break;

                case (false, true):
                    fsw.cboFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.cboFontSizeCenter.Visibility    = Visibility.Collapsed;
                    fsw.grpTypeface.Visibility          = Visibility.Visible;
                    break;

                case (false, false):
                    fsw.cboFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.grpTypeface.Visibility          = Visibility.Collapsed;
                    break;
            }

            ConnectEventHandlers();

            PopulateFontSizes();
            PopulateFontFamilies();

            if( SelectedFont is null ) {
                if( fsw.lstFontFamily.Items.Count > 0 ) fsw.lstFontFamily.SelectedIndex = 0;
                if( fsw.lvwTypeface.Items.Count > 0 )   fsw.lvwTypeface.SelectedIndex = 0;

                if( fsw.cboFontSize is not null ) {
                    FontSize = 11d;
                }
            }

            return fsw.ShowDialog();
        }


        //public FontDescriptor   SelectedFont {
        //    get {
        //        FontDescriptor      result;

        //    }
        //}

        private bool            CanSelect() {

            bool        result;


            result = fsw.lstFontFamily.SelectedItem != null;

            if( AllowFontSizeSelection ) {
                if( fsw.cboFontSize is not null ) {
                    if( !Double.TryParse( fsw.cboFontSize.Text, out _ ) ) result = false;
                }
                else {
                    result = false;
                }
            }

            return result && AllowTypefaceSelection && fsw.lvwTypeface.SelectedItem != null;
        }
        private double?         FontSize {
            get => fsw?.cboFontSize?.SelectedItem as double?;
            set {
                if( fsw.cboFontSize is not null ) {
                    foreach( NamedValue<double> nv in fsw.cboFontSize.Items ) {
                        if( nv.Value == value ) {
                            fsw.cboFontSize.SelectedItem = nv;
                            break;
                        }
                    }
                }
            }
        }
        private FontFamily?     FontFamily {
            get => fsw?.lstFontFamily?.SelectedItem as FontFamily;
            set {
                fsw.lstFontFamily.SelectedItem = value;
                //foreach( FontFamily ff in fsw.lstFontFamily.Items ) {
                //    if( ff == value ) {

                //    }
                //}
            }
        }
        private Typeface?       Typeface {
            get => fsw.lvwTypeface?.SelectedItem as Typeface;
            set => fsw.lvwTypeface.SelectedItem = value;
        }


        private void            ConnectEventHandlers() {

            fsw.btnSelect.Click                     += btnSelect_Click;
            fsw.lstFontFamily.SelectionChanged      += lstFontFamily_SelectionChanged;
            fsw.lvwTypeface.SelectionChanged        += lvwTypeface_SelectionChanged;
            fsw.txtSearch.TextChanged               += txtSearch_TextChanged;

            if( fsw.cboFontSize is not null ) {
                //fsw.cboFontSize.V
                fsw.cboFontSize.SelectionChanged    += cboFontSize_SelectionChanged;
            }
        }
        private void            PopulateFontFamilies( string? searchSpec = null ) {

            fsw.lstFontFamily.Items.Clear();

            foreach( FontFamily ff in Fonts.SystemFontFamilies ) {
                if( searchSpec is null || ff.Source.Contains( searchSpec, StringComparison.CurrentCultureIgnoreCase ) ) {
                    fsw.lstFontFamily.Items.Add( ff );
                }
            }
        }
        private void            PopulateFontSizes() {

            if( fsw.cboFontSize is not null ) {
                for( double size = 5d; size < 73d; size += 2 ) {
                    fsw.cboFontSize.Items.Add( new NamedValue<double>( size.ToString(), size ) );
                }
            }
        }
        private void            UpdateSampleText() {

            FontFamily?     ff      = FontFamily;
            Typeface?       tf      = Typeface;


            if( ff is not null && tf is not null ) {
                fsw.tbSample.FontFamily     = ff;
                fsw.tbSample.FontStretch    = tf.Stretch;
                fsw.tbSample.FontStyle      = tf.Style;
                fsw.tbSample.FontWeight     = tf.Weight;
            }
        }


        private void btnSelect_Click                ( object sender, RoutedEventArgs e ) {
        }
        private void cboFontSize_SelectionChanged   ( object sender, SelectionChangedEventArgs e ) {

            fsw.btnSelect.IsEnabled = CanSelect();
        }
        private void lstFontFamily_SelectionChanged ( object sender, SelectionChangedEventArgs e ) {

            FontFamily?     ff                  = FontFamily;
            Typeface?       selectedTypeface    = Typeface;


            fsw.lvwTypeface.ItemsSource         = null;

            if( ff is not null ) {
                fsw.lvwTypeface.ItemsSource     = ff.GetTypefaces();

                if( selectedTypeface is not null ) {
                    foreach( Typeface tf in fsw.lvwTypeface.Items ) {
                        if( selectedTypeface.Stretch == tf.Stretch &&
                            selectedTypeface.Style   == tf.Style   &&
                            selectedTypeface.Weight  == tf.Weight ) {

                            fsw.lvwTypeface.SelectedItem = tf;

                            break;
                        }
                    }
                }

                if( fsw.lvwTypeface.SelectedItem is null && fsw.lvwTypeface.Items.Count > 0 ) {
                    fsw.lvwTypeface.SelectedItem = fsw.lvwTypeface.Items[0];
                }

                fsw.btnSelect.IsEnabled = CanSelect();

                UpdateSampleText();
            }
        }
        private void lvwTypeface_SelectionChanged   ( object sender, SelectionChangedEventArgs e ) {

            fsw.btnSelect.IsEnabled = CanSelect();

            UpdateSampleText();
        }
        private void txtSearch_TextChanged          ( object sender, TextChangedEventArgs e ) {

            string? s = ((TextBox)sender)?.Text.Trim();


            PopulateFontFamilies( s );
        }
    }
}