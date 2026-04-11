using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class XmlKey {

        public  string  Name    { get; internal set; }
        public  string  Type    { get; set; }
        public  string  Value   { get; set; }


        internal XmlKey( string name, string value, string type ) {

            Name    = name;
            Value   = value;
            Type    = type;
        }
        internal XmlKey( string name, string value ) : this( name, value, "String" ) { }


        public override string ToString() {

            return Value;
        }
    }
}