using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class XmlSectionList : SimpleListBase<XmlSection> {


        internal XmlSectionList() : base() { }


        public void Remove( string name ) {

            base.inner.RemoveAll( s => s.Name == name );
        }

        public bool Exists( string name ) {

            return base.inner.Exists( s => s.Name == name );
        }

        public XmlSection this[string name] {
            get {
                XmlSection? result;

                result = base.inner.Find( s => s.Name == name );
                if( result == null ) {
                    result = new XmlSection( name );

                    base.inner.Add( result );
                }

                return result;
            }
        }
    }
}