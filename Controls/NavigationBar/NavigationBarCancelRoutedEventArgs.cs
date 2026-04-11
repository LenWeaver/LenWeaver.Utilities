using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {


    public class NavigationBarCancelRoutedEventArgs : NavigationBarRoutedEventArgs {

        public  bool        Cancel { get; set; }


        public NavigationBarCancelRoutedEventArgs( RoutedEvent routedEvent, object source ) : base( routedEvent, source ) {}
        public NavigationBarCancelRoutedEventArgs( RoutedEvent routedEvent ) : base( routedEvent ) {}
        public NavigationBarCancelRoutedEventArgs() : base() {}
    }
}