using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class SimpleCollectionBase<T> : IEnumerable<T> {

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