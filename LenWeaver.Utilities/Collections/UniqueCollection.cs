using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class UniqueCollection<T> : SimpleCollectionBase<T> where T : IEquatable<T> {
        
        public UniqueCollection() : base() {}


        public void Add( T item ) {

            bool    alreadyInList   = false;


            foreach( IEquatable<T> t in inner ) {
                if( t.Equals( item ) ) {
                    alreadyInList = true;
                    break;
                }
            }

            if( !alreadyInList ) inner.Add( item );
        }
        public void Clear() {

            inner.Clear();
        }
        public void Remove( T item ) {

            inner.Remove( item );
        }


        public new T this[int index] {
            get { return inner[index]; }
        }
    }
}