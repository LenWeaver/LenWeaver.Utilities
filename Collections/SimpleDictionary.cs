using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class SimpleDictionary<TKey,TValue> : SimpleDictionaryBase<TKey,TValue> where TKey : notnull {

        public SimpleDictionary() : base() {}


        public TValue this[TKey key] {
            get { return base.inner[key]; }
            set { base.inner[key] = value; }
        }


        public void Add( TKey key, TValue value ) {

            base.inner.Add( key, value );
        }
        public void Clear() {

            base.inner.Clear();
        }
        public void Remove( TKey key ) {

            base.inner.Remove( key );
        }
    }
}