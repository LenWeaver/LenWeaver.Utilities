using System.Collections.Generic;

namespace LenWeaver.Utilities {

    public class SimpleCollection<T> : SimpleCollectionBase<T> {


        public SimpleCollection() : base() {}


        public virtual void Add( T item ) {

            base.inner.Add( item );
        }
        public virtual void AddRange( IEnumerable<T> items ) {

            base.inner.AddRange( items );
        }
        public virtual void Clear() {

            base.ClearList();
        }
        public virtual void Remove( T item ) {

            base.inner.Remove( item );
        }
        public virtual void RemoveAt( int index ) {

            base.inner.RemoveAt( index );
        }

        public new T this[int index] {
            get{ return base[index]; }
            set{ base.inner[index] = value; }
        }
    }
}