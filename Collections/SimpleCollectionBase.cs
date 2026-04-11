using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class SimpleCollectionBase<T> : IEnumerable<T>, ICollection<T>, ICollection {

        internal protected List<T>      inner;


#pragma warning disable CS8618 // 'inner' reference is set in 'ClearList' method.
        public SimpleCollectionBase() {
#pragma warning restore CS8618

            ClearList();
        }


        protected virtual void ClearList() {

            inner = new List<T>();
        }

        public int Count {
            get{ return inner.Count; }
        }


        public T this[int index] {
            get => inner[index];
        }


        #region ICollection<T> and ICollection Implementation
        bool ICollection<T>.IsReadOnly => false;
        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotImplementedException();
        #endregion
        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator() {

            return inner.GetEnumerator();
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {

            return ((System.Collections.IEnumerable)inner).GetEnumerator();
        }

        void ICollection<T>.Add( T item ) {
            throw new NotImplementedException();
        }

        void ICollection<T>.Clear() {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains( T item ) {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo( T[] array, int arrayIndex ) {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove( T item ) {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo( Array array, int index ) {
            throw new NotImplementedException();
        }
        #endregion
    }
}