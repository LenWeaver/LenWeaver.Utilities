using System.Collections.Generic;

namespace LenWeaver.Utilities {

    public class SimpleCollection<T> : SimpleCollectionBase<T> {


        public SimpleCollection() : base() {}


        public void Add( T item ) {

            base.inner.Add( item );
        }
        public void AddRange( IEnumerable<T> items ) {

            base.inner.AddRange( items );
        }
        public void Clear() {

            base.inner.Clear();
        }
        public void Remove( T item ) {

            base.inner.Remove( item );
        }
        public void RemoveAt( int index ) {

            base.inner.RemoveAt( index );
        }

        public new T this[int index] {
            get{ return base[index]; }
            set{ base.inner[index] = value; }
        }
    }
}