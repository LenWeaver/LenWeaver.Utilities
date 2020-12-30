using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class DateStampedCollection<T> : IEnumerable<DateStampedItem<T>> where T : notnull {

        public      int                         Capacity { get; set; }

        private     List<DateStampedItem<T>>    inner;


#pragma warning disable CS8618 // 'inner' reference will be set in 'Clear' method.
        public DateStampedCollection( int capacity ) {
#pragma warning restore CS8618

            Clear();

            if( capacity < 0 ) throw new ArgumentOutOfRangeException( "Capacity must be zero (to indicate an unlimited capacity) or a positive value." );

            Capacity = capacity;
        }
        public DateStampedCollection() : this( 0 ) {}


        public int Count {
            get { return inner.Count; }
        }
        public DateStampedItem<T> this[int index] {
            get { return inner[index]; }
        }


        public void Clear() {

            inner   = new List<DateStampedItem<T>>();
        }

        public DateStampedItem<T> Add( T item ) {

            return Add( item, DateTime.Now );
        }
        public DateStampedItem<T> Add( T item, DateTime dateStamp ) {

            DateStampedItem<T>      stampedItem;


            stampedItem = new DateStampedItem<T>( item, dateStamp );

            inner.Add( stampedItem );
            inner.Sort();

            if( Capacity != 0 && inner.Count > Capacity ) {
                inner.RemoveAt( 0 );
            }

            return stampedItem;
        }
        public DateStampedItem<T> Remove( T item ) {

            DateStampedItem<T>? result  = null;


            foreach( DateStampedItem<T> dsi in inner ) {
                if( dsi.Item.Equals( item ) ) {
                    result = dsi;
                    inner.Remove( result );
                    break;
                }
            }

            if( result == null ) throw new ArgumentException( "Specified item is not in collection." );

            return result;
        }


        #region IEnumerable Implementation
        public IEnumerator<DateStampedItem<T>> GetEnumerator() {
            
            foreach( DateStampedItem<T> item in inner ) {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {

            foreach( DateStampedItem<T> item in inner ) {
                yield return item;
            }
        }
        #endregion
    }
}