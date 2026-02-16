using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LenWeaver.Utilities {

    public static class XmlHelpers {


        public static XmlAttribute CreateAttribute( XmlDocument xml, string name, string value ) {

            XmlAttribute        result;


            try {
                result          = xml.CreateAttribute( name );

                result.Value    = value;
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to create xml attribute.", ex );
            }

            return result;
        }
    }
}