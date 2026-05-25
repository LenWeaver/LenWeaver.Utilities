using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LenWeaver.Utilities {

    public class DialogButtonBarRoutedEventArgs : RoutedEventArgs {

        public DialogButton     DialogButton        { get; init; }


        internal DialogButtonBarRoutedEventArgs( DialogButton btn ) : base() {

            DialogButton        = btn;
        }
        internal DialogButtonBarRoutedEventArgs( DialogButton btn, RoutedEvent routedEvent ) : base( routedEvent ) {

            DialogButton        = btn;
        }
        internal DialogButtonBarRoutedEventArgs( DialogButton btn, RoutedEvent routedEvent, object source ) : base( routedEvent, source ) {

            DialogButton        = btn;
        }
    }
}