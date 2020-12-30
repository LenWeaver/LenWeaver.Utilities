using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class XmlCommentCollection : SimpleCollectionBase<string> {


        internal XmlCommentCollection() : base() { }


        public void Add( string s ) {

            base.inner.Add( s );
        }
        public void RemoveAt( int index ) {

            base.inner.RemoveAt( index );
        }
    }
}