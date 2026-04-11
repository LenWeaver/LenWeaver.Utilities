using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LenWeaver.Utilities {

    public static class XmlExtensions {

        static XmlExtensions() {}


        public static XmlAttribute AppendAttribute( this XmlElement element, string name, string value ) {

            XmlAttribute    result;
            

            result          = element.OwnerDocument.CreateAttribute( name );
            result.Value    = value;

            element.Attributes.Append( result );

            return result;
        }
    }
}