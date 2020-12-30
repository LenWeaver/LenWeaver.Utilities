using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class SimpleListBase<T> : IEnumerable<T> {

        protected internal List<T>      inner;


#pragma warning disable CS8618 // 'inner' reference will be set in 'ClearList' method.
        public SimpleListBase() {
#pragma warning restore CS8618

            ClearList();
        }


        internal void ClearList() {

            inner = new List<T>();
        }

        public int Count {
            get { return inner.Count; }
        }

        public void CopyTo( int index, T[] array, int arrayIndex, int count ) {

            inner.CopyTo( index, array, arrayIndex, count );
        }
        public void CopyTo( T[] array, int arrayIndex ) {

            inner.CopyTo( array, arrayIndex );
        }
        public void CopyTo( T[] array ) {

            inner.CopyTo( array );
        }
        public void Sort() {

            inner.Sort();
        }
        public void Sort( int index, int count, IComparer<T> comparer ) {

            inner.Sort( index, count, comparer );
        }
        public void Sort( IComparer<T> comparer ) {

            inner.Sort( comparer );
        }
        public void Sort( Comparison<T> comparison ) {

            inner.Sort( comparison );
        }

        public bool Contains( T item ) {

            return inner.Contains( item );
        }

        public int BinarySearch( T item, IComparer<T> comparer ) {

            return inner.BinarySearch( item, comparer );
        }
        public int BinarySearch( T item ) {

            return inner.BinarySearch( item );
        }
        public int IndexOf( T item, int index, int count ) {

            return inner.IndexOf( item, index, count );
        }

        public T[] ToArray() {

            return inner.ToArray();
        }

        public T this[int index] {
            get{ return inner[index]; }
        }

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator() {

            return inner.GetEnumerator();
        }
        #endregion
        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {

            return ((System.Collections.IEnumerable)inner).GetEnumerator();
        }
        #endregion
    }
}