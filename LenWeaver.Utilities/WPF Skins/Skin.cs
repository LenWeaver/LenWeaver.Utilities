using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {


    public class Skin : ISkin {

        private readonly string         _folder;

        private ResourceDictionary?     _shared         = null;

        private ResourceDictionary?     _button         = null;
        private ResourceDictionary?     _calendar       = null;
        private ResourceDictionary?     _comboBox       = null;
        private ResourceDictionary?     _datePicker     = null;
        private ResourceDictionary?     _listBox        = null;
        private ResourceDictionary?     _textBox        = null;


        internal Skin( string folder ) {
        
            _folder = folder;
        }


        public ResourceDictionary Shared {
            get {
                ResourceDictionary      rd;


                if( _shared == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "Shared" );

                    _shared     = rd;
                }

                return _shared;
            }
        }

        public ResourceDictionary Button {
            get {
                ResourceDictionary      rd;


                if( _button == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "Button" );

                    _button     = rd;
                }

                return _button;
            }
        }
        public ResourceDictionary Calendar {
            get {
                ResourceDictionary      rd;


                if( _calendar == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "Calendar" );

                    _calendar   = rd;
                }

                return _calendar;
            }
        }
        public ResourceDictionary ComboBox {
            get {
                ResourceDictionary      rd;


                if( _comboBox == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "ComboBox" );

                    _comboBox   = rd;
                }

                return _comboBox;
            }
        }
        public ResourceDictionary DatePicker {
            get {
                ResourceDictionary      rd;


                if( _datePicker == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "DatePicker" );

                    _datePicker     = rd;
                }

                return _datePicker;
            }
        }
        public ResourceDictionary ListBox {
            get {
                ResourceDictionary      rd;


                if( _listBox == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "ListBox" );

                    _listBox    = rd;
                }

                return _listBox;
            }
        }
        public ResourceDictionary TextBox {
            get {
                ResourceDictionary      rd;


                if( _textBox == null ) {
                    rd          = new ResourceDictionary();
                    rd.Source   = BuildUri( "TextBox" );

                    _textBox    = rd;
                }

                return _textBox;
            }
        }


        internal Uri BuildUri( string control ) {

            return new Uri( $"/LenWeaver.Utilities;component" +
                            $"/WPF Skins/{_folder}/Skin-{control}.xaml", UriKind.RelativeOrAbsolute );
        }
    }
}