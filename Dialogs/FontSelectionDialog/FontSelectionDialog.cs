using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace LenWeaver.Utilities {

    //FUTURE: Allow the user to access four filter toggles.  Show monospace, proportional, emoji and symbol.
    public class FontSelectionDialog {

        public bool                 AllowFontSizeSelection          { get; set; }       = true;
        public bool                 AllowSearch                     { get; set; }       = true;
        public bool                 AllowTypefaceSelection          { get; set; }       = true;
        public bool                 IncludeEmojiFonts               { get; set; }       = false;
        public bool                 IncludeMonospaceFonts           { get; set; }       = true;
        public bool                 IncludeProportionalFonts        { get; set; }       = true;
        public bool                 IncludeSymbolFonts              { get; set; }       = false;

        public string               Title                           { get; set; }       = "Font Selection...";

        public Window?              Owner                           { get; set; }       = null;


        private                     bool                            templateApplied     = false;
        private                     FontDescriptor?                 delayedFont         = null;
        private readonly            FontSelectionWindow             fsw;


        public FontSelectionDialog() {

            fsw = new FontSelectionWindow();
            fsw.Dispatcher.BeginInvoke( new Action( () => {

                if( delayedFont != null ) SelectedFont = delayedFont;
            } ), DispatcherPriority.Loaded );
        }


        public bool?                ShowDialog() {

            if( Owner is not null ) fsw.Owner           = Owner;
            fsw.Title                                   = Title;

            fsw.txtSearch.Visibility                    = AllowSearch ? Visibility.Visible : Visibility.Collapsed;

            switch( AllowFontSizeSelection, AllowTypefaceSelection ) {
                case (true, true):
                    fsw.nudFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.nudFontSizeCenter.Visibility    = Visibility.Visible;
                    fsw.grpTypeface.Visibility          = Visibility.Visible;
                    fsw.nudFontSize                     = fsw.nudFontSizeCenter;
                    break;

                case (true, false):
                    fsw.nudFontSizeLeft.Visibility      = Visibility.Visible;
                    fsw.grpTypeface.Visibility          = Visibility.Collapsed;
                    fsw.nudFontSizeCenter.Visibility    = Visibility.Collapsed;
                    fsw.nudFontSize                     = fsw.nudFontSizeLeft;
                    break;

                case (false, true):
                    fsw.nudFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.nudFontSizeCenter.Visibility    = Visibility.Collapsed;
                    fsw.grpTypeface.Visibility          = Visibility.Visible;
                    break;

                case (false, false):
                    fsw.nudFontSizeLeft.Visibility      = Visibility.Collapsed;
                    fsw.nudFontSizeCenter.Visibility    = Visibility.Collapsed;
                    fsw.grpTypeface.Visibility          = Visibility.Collapsed;
                    break;
            }

            ConnectEventHandlers();

            PopulateFontFamilies();

            if( fsw.nudFontSize != null ) {
                fsw.nudFontSize.StringFormat = "{0} pt";
                fsw.nudFontSize.HorizontalContentAlignment = HorizontalAlignment.Center;
                fsw.nudFontSize.MaximumValue = 72m;
                fsw.nudFontSize.MinimumValue =  3m;
            }

            return fsw.ShowDialog();
        }


        public FontDescriptor?      SelectedFont {
            get {
                FontDescriptor?     result      = null;

                if( FontFamily != null && FontSize != null && Typeface != null ) {
                    result = new FontDescriptor( FontFamily, (double)FontSize, Typeface );
                }

                return result;
            }
            set {
                if( value != null ) {
                    if( fsw.TemplateLoaded ) {
                        FontFamily      = value.Family;
                        FontSize        = value.Size;
                        Typeface        = value.Typeface;
                    }
                    else {
                        delayedFont     = value;
                    }
                }
            }
        }

        private bool                CanSelect() {

            bool        result;

            
            result = fsw.lstFontFamily.SelectedItem != null;

            if( AllowFontSizeSelection && result ) {
                result = fsw.nudFontSize != null;
            }

            return result && AllowTypefaceSelection && fsw.lvwTypeface.SelectedItem != null;
        }
        private double?             FontSize {
            get => (double?)(fsw.nudFontSize?.Value ?? null);
            set {
                fsw.nudFontSize?.Value = (decimal?)value ?? 5.5m;
            }
        }
        private FontFamily?         FontFamily {
            get => fsw.lstFontFamily?.SelectedItem as FontFamily;
            set {
                fsw.lstFontFamily.SelectedItem = value;
                fsw.lstFontFamily.ScrollIntoView( value );
            }
        }
        private Typeface?           Typeface {
            get => fsw.lvwTypeface?.SelectedItem as Typeface;
            set {
                fsw.lvwTypeface.SelectedItem = value;
                fsw.lvwTypeface.ScrollIntoView( value );
            }
        }


        private void                ConnectEventHandlers() {

            fsw.btnSelect.Click                     += btnSelect_Click;
            fsw.lstFontFamily.SelectionChanged      += lstFontFamily_SelectionChanged;
            fsw.lvwTypeface.SelectionChanged        += lvwTypeface_SelectionChanged;
            fsw.txtSearch.TextChanged               += txtSearch_TextChanged;
            fsw.nudFontSize?.ValueChanged           += nudFontSize_ValueChanged;
        }

        private void nudFontSize_ValueChanged( object sender, RoutedPropertyChangedEventArgs<decimal> e ) {
            
            fsw.btnSelect.IsEnabled = CanSelect();
        }

        private void                PopulateFontFamilies( string? searchSpec = null ) {

            fsw.lstFontFamily.Items.Clear();

            foreach( FontFamily ff in Fonts.SystemFontFamilies ) {
                if( searchSpec == null || ff.Source.Contains( searchSpec, StringComparison.CurrentCultureIgnoreCase ) ) {
                    fsw.lstFontFamily.Items.Add( ff );
                }
            }

            fsw.lstFontFamily.Items.SortDescriptions.Add( new SortDescription( nameof(FontFamily.Source), ListSortDirection.Ascending ) );
        }
        //TODO: Display the GlyphTypeface.Sample text when present.
        private void                UpdateSampleText() {

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

            fsw.DialogResult = true;
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