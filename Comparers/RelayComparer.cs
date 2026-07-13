using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LenWeaver.Utilities {

    public class RelayComparer<T> : IComparer<T> {

        private readonly CompareNullPlacement   nullPlacement;

        private readonly ComparerDelegate<T>?   comparerDelegate;
        private readonly Func<T?,T?,int>?       compareFunction;


        public RelayComparer( ComparerDelegate<T> cmpDelegate,  CompareNullPlacement cmpNullPlacement ) {

            nullPlacement           = cmpNullPlacement;

            comparerDelegate        = cmpDelegate;
            compareFunction         = null;
        }
        public RelayComparer( ComparerDelegate<T> cmpDelegate ) : this( cmpDelegate, CompareNullPlacement.NullsOnTop ) {}
        public RelayComparer( Func<T?,T?,int> cmpFunction,      CompareNullPlacement cmpNullPlacement ) {

            nullPlacement           = cmpNullPlacement;

            comparerDelegate        = null;
            compareFunction         = cmpFunction;
        }
        public RelayComparer( Func<T?,T?,int> cmpFunction )     : this( cmpFunction, CompareNullPlacement.NullsOnTop ) {}


        public int Compare( T? x, T? y ) {

            if( BasicComparers.TryCompareNulls( x, y, nullPlacement, out int result ) ) {
                return result;
            }

            return compareFunction?.Invoke( x, y ) ?? comparerDelegate?.Invoke( x, y )
                                                   ?? throw new ArgumentNullException( null, "Neither ComparerDelegate or Func provided." );
        }
    }
}