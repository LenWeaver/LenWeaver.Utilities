using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class UniqueCollection<T> : SimpleCollectionBase<T> where T : IEquatable<T> {
        
        public UniqueCollection() : base() {}


        public virtual void Add( T item ) {

            bool    alreadyInList   = false;


            foreach( IEquatable<T> t in inner ) {
                if( t.Equals( item ) ) {
                    alreadyInList = true;
                    break;
                }
            }

            if( !alreadyInList ) inner.Add( item );
        }
        public virtual void AddRange( params IEnumerable<T> items ) {

            foreach( T item in items ) Add( item );
        }
        public virtual void Clear() {

            inner.Clear();
        }
        public virtual void Remove( T item ) {

            inner.Remove( item );
        }


        public new T this[int index] {
            get { return inner[index]; }
        }
    }
}