using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {

    public class NavigationBarRoutedEventArgs : RoutedEventArgs {

        public NavigationBarItem?   NewItem     { get; protected internal set; }
        public NavigationBarItem?   OldItem     { get; protected internal set; }


        public NavigationBarRoutedEventArgs( RoutedEvent routedEvent, object source ) : base( routedEvent, source ) {}
        public NavigationBarRoutedEventArgs( RoutedEvent routedEvent ) : base( routedEvent ) {}
        public NavigationBarRoutedEventArgs() : base () {}
    }
}