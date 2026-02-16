using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class DateStampedItem<T> : IComparable<DateStampedItem<T>> {

        public  DateTime        DateStamp       { get; internal set; }

        public  T               Item            { get; set; }


        internal DateStampedItem( T item, DateTime dateStamp ) {

            DateStamp   = dateStamp;
            Item        = item;
        }


        #region IComparable<DateStampedItem<T>> Implementation
        public int CompareTo( DateStampedItem<T>? other ) {

            if( other is null ) throw new ArgumentNullException( nameof(other) );

            return DateStamp.CompareTo( other.DateStamp );
        }
        #endregion
    }
}