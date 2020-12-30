using System;
using System.Collections.Generic;


namespace LenWeaver.Utilities {

    public class DayViewCollection : SimpleCollectionBase<DayView> {

        internal DayViewCollection() : base() {}


        protected internal void Add( DayView dv ) {

            base.inner.Add( dv );
        }
        protected internal void Clear() {

            base.ClearList();
        }

        public DayView this[int index] {
            get => base.inner[index];
        }
        public DayView? this[DateTime date] {
            get {
                DayView?    result  = null;

                
                for( int index = 0; index < base.Count; index++ ) {
                    if( date.Date.CompareTo( base.inner[index].Date.Date ) == 0 ) {
                        result = base.inner[index];
                        break;
                    }
                }

                return result;
            }
        }
    }
}