using System;
using System.Collections.Generic;


namespace LenWeaver.Utilities {


    public class PropertyChangingRefEventArgs<T> : PropertyChangedRefEventArgs<T> where T : class {

        public bool Cancel { get; set; }    = false;


        public PropertyChangingRefEventArgs( T? newValue, T? oldValue ) : base( newValue, oldValue ) {}
        public PropertyChangingRefEventArgs() : this( null, null ) {}
    }


    public class PropertyChangingValEventArgs<T> : PropertyChangedValEventArgs<T> where T : struct {

        public bool Cancel { get; set; }    = false;


        public PropertyChangingValEventArgs( T? newValue, T? oldValue ) : base( newValue, oldValue ) {}
        public PropertyChangingValEventArgs() : this( null, null ) {}
    }
}