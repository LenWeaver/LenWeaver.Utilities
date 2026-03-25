using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    internal class PopupCommandParameters {

        public bool     Handled         { get; set; }           = false;

        public string   DisplayText     { get; set; }           = String.Empty;

        public object?  NewValue        { get; internal set; }
        public object?  OldValue        { get; internal set; }


        public PopupCommandParameters( object? newValue, object? oldValue ) {

            NewValue    = newValue;
            OldValue    = oldValue;
        }
    }
}