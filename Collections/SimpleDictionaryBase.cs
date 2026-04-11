using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class SimpleDictionaryBase<TKey,TValue> : IEnumerable<KeyValuePair<TKey,TValue>> where TKey : notnull {
        
        protected Dictionary<TKey,TValue>       inner;


        public SimpleDictionaryBase( IEqualityComparer<TKey>? comparer ) {

            inner = new Dictionary<TKey, TValue>( comparer );
        }
        public SimpleDictionaryBase() : this( null ) {}


        protected void ClearList() {

            inner.Clear();
        }

        public bool ContainsKey( TKey key ) {

            return inner.ContainsKey( key );
        }
        public bool ContainsValue( TValue value ) {

            return inner.ContainsValue( value );
        }
        public bool TryGetValue( TKey key, out TValue value ) {

            return inner.TryGetValue( key, out value );
        }

        public int Count {
            get{ return inner.Count; }
        }

        public IEqualityComparer<TKey> Comparer {
            get{ return inner.Comparer; }
        }

        public Dictionary<TKey, TValue>.KeyCollection   Keys {
            get{ return inner.Keys; }
        }
        public Dictionary<TKey, TValue>.ValueCollection Values {
            get{ return inner.Values; }
        }

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            
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