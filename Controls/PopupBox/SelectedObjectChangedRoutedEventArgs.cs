using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LenWeaver.Utilities {

    public class SelectedObjectChangedRoutedEventArgs : RoutedEventArgs {

        public string   DisplayText     { get; set; }   = String.Empty;

        public object?  NewValue        { get; set; }   = null;
        public object?  OldValue        { get; set; }   = null;


        internal SelectedObjectChangedRoutedEventArgs( RoutedEvent routedEvent, object source ) : base( routedEvent, source ) {}
        internal SelectedObjectChangedRoutedEventArgs( RoutedEvent routedEvent )                : base( routedEvent, null ) {}
        internal SelectedObjectChangedRoutedEventArgs()                                         : base( null, null ) {}
    }
}