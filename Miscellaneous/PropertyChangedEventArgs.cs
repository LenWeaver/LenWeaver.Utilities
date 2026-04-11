using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {

    public class PropertyChangedRefEventArgs<T> : RoutedEventArgs where T : class {

        public T?   NewValue    { get; protected internal set; }
        public T?   OldValue    { get; protected internal set; }

        public T[]? NewValues   { get; protected internal set; }    = null;


        public PropertyChangedRefEventArgs( T? newValue, T? oldValue ) : base() {
            
            NewValue    = newValue;
            OldValue    = oldValue;
        }
        public PropertyChangedRefEventArgs() : this( null, null ) {}
    }


    public class PropertyChangedValEventArgs<T> : RoutedEventArgs where T : struct {

        public T?   NewValue    { get; protected internal set; }
        public T?   OldValue    { get; protected internal set; }

        public T[]? NewValues   { get; protected internal set; }    = null;


        public PropertyChangedValEventArgs( T? newValue, T? oldValue ) : base() {

            NewValue    = newValue;
            OldValue    = oldValue;
        }
        public PropertyChangedValEventArgs() : this( null, null ) {}
    }
}