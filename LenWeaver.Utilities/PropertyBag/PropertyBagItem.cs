using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    internal class PropertyBagItem {

        public string   Key       { get; internal set; }

        public object?  Value     { get; internal set; }

        public Type     Type      { get; internal set; }


        internal PropertyBagItem( string key, bool value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Boolean);
        }
        internal PropertyBagItem( string key, byte value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Byte);
        }
        internal PropertyBagItem( string key, char value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Char);
        }
        internal PropertyBagItem( string key, short value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Int16);
        }
        internal PropertyBagItem( string key, ushort value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(UInt16);
        }
        internal PropertyBagItem( string key, int value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Int32);
        }
        internal PropertyBagItem( string key, uint value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(UInt32);
        }
        internal PropertyBagItem( string key, long value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Int64);
        }
        internal PropertyBagItem( string key, ulong value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(UInt64);
        }
        internal PropertyBagItem( string key, DateTime value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(DateTime);
        }
        internal PropertyBagItem( string key, Guid value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Guid);
        }
        internal PropertyBagItem( string key, TimeSpan value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(TimeSpan);
        }
        internal PropertyBagItem( string key, string value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(String);
        }
        internal PropertyBagItem( string key, object? value ) {

            Key     = key;
            Value   = value;
            Type    = typeof(Object);
        }
    }
}