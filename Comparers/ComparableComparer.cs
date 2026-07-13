using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class ComparableComparer<T> : IComparer<T> where T : IComparable<T> {

        private readonly CompareNullPlacement   nullPlacement;


        public ComparableComparer( CompareNullPlacement placement ) {

            nullPlacement = placement;
        }
        public ComparableComparer() : this( CompareNullPlacement.NullsOnTop ) {}


        public int Compare( T? x, T? y ) {

            if( BasicComparers.TryCompareNulls( x, y, nullPlacement, out int result ) ) {
                return result;
            }

            return x!.CompareTo( y );
        }
    }
}