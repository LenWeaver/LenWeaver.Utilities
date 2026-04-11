using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

#nullable disable
    public class PropertyBag {

        internal Dictionary<string, PropertyBagItem> Items { get; set; }


        public PropertyBag() {

            Clear();
        }


        public object this[string key] {
            get {
                object  result;


                if( Items.ContainsKey( key ) ) {
                    result = Items[key].Value;
                }
                else {
                    result = null;
                }

                return result;
            }
            set {
                if( Items.ContainsKey( key ) ) {
                    Items[key].Value = value;
                }
                else {
                    Items.Add( key, new PropertyBagItem( key, value ) );
                }
            }
        }

        public void Clear() {

            Items = new Dictionary<string, PropertyBagItem>();
        }
        public void Remove( string key ) {

            Items.Remove( key );
        }

        public bool ContainsKey( string key ) {

            return Items.ContainsKey( key );
        }

        public T GetValue<T>( string key, T defaultValue ) {

            T result;


            if( Items.ContainsKey( key ) ) {
                result = (T)Items[key].Value;
            }
            else {
                result = defaultValue;
            }

            return result;
        }

        public IEnumerable<string> Keys {
            get {
                foreach( KeyValuePair<string,PropertyBagItem> kvp in Items ) {
                    yield return kvp.Key;
                }
            }
        }
    }
}