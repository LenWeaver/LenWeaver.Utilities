using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LenWeaver.Utilities {

    public class NamedValue<T> : IComparable<NamedValue<T>> {

        public  string  Name    { get; set; }

        public  T       Value   { get; set; }


        public NamedValue( string name, T value ) {

            Name    = name;
            Value   = value;
        }


        public override string ToString() {
            
            return Name;
        }


        public int CompareTo( NamedValue<T>? other ) {
            
            if( other is null ) throw new ArgumentNullException( nameof(other), "Parameter cannot be null." );

            return Name.CompareTo( other.Name );
        }


        public (string Name, T Value) Deconstruct() {

            return (Name, Value);
        }
    }
}