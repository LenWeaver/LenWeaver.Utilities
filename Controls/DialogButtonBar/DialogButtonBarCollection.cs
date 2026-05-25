using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LenWeaver.Utilities {

    public class DialogButtonBarCollection : ObservableCollection<DialogButton> {

        internal DialogButtonBarCollection() {}


        public void Add( string text, bool isDefault, bool isCancel, ICommand command ) {

            Add( new DialogButton( text, isDefault, isCancel, command ) );
        }
        public void Add( string text, bool isDefault, bool isCancel, Action<DialogButton> clicked ) {

            Add( new DialogButton( text, isDefault, isCancel, clicked: clicked ) );
        }
        public void Add( string text, ICommand command ) {

            Add( new DialogButton( text, false, false, command ) );
        }
        public void Add( string text, Action<DialogButton> clicked ) {

            Add( new DialogButton( text, false, false, clicked: clicked ) );
        }
        public void Add( string text ) {

            Add( new DialogButton( text ) );
        }
    }
}