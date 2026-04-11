using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class XmlKeyList : SimpleListBase<XmlKey> {


        internal XmlKeyList() : base() { }


        public void Clear() {

            base.ClearList();
        }
        public void Remove( string name ) {

            base.inner.RemoveAll( s => s.Name == name );
        }

        public bool Exists( string name ) {

            return base.inner.Exists( s => s.Name == name );
        }

        public XmlKey? this[string name] {
            get {
                XmlKey?  result  = null;


                foreach( XmlKey key in base.inner ) {
                    if( key.Name == name ) {
                        result = key;
                        break;
                    }
                }

                return result;
            }
        }

        public XmlKey Add( XmlKey key ) {

            if( !Exists( key.Name ) ) {
                base.inner.Add( key );
            }
            else {
                Remove( key.Name );
                base.inner.Add( key );
            }

            return key;
        }
        public XmlKey Add( string name, string value, string type ) {

            XmlKey  key;


            if( !base.inner.Exists( x => x.Name == name ) ) {
                key = new XmlKey( name, value, type );

                base.inner.Add( key );
            }
            else {
#pragma warning disable CS8600
                key         = this[name];


                key!.Value  = value;
                key.Type    = type;
#pragma warning restore CS8600
            }

            return key;
        }
        public XmlKey Add( string name, string value ) {

            return Add( name, value, "String" );
        }
        public XmlKey Add( string name, bool value ) {

            return Add( name, value ? "True" : "False", "Boolean" );
        }
        public XmlKey Add( string name, int value ) {

            return Add( name, value.ToString(), "Int32" );
        }
        public XmlKey Add( string name, long value ) {

            return Add( name, value.ToString(), "Int64" );
        }
        public XmlKey Add( string name, float value ) {

            return Add( name, value.ToString(), "Single" );
        }
        public XmlKey Add( string name, double value ) {

            return Add( name, value.ToString(), "Double" );
        }
        public XmlKey Add( string name, decimal value ) {

            return Add( name, value.ToString() );
        }
        public XmlKey Add( string name, DateTime value ) {

            return Add( name, value.ToString( DateTimeHelpers.ISO8601DateTimeFormat ), "DateTime" );
        }
    }
}