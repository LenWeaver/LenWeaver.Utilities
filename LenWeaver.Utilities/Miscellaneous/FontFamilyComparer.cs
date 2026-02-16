using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class FontFamilyComparer : IComparer<FontFamily> {


        public int Compare( FontFamily? x, FontFamily? y ) {

            if( x is null || y is null ) throw new ArgumentNullException();

            return x.Source.CompareTo( y.Source );
        }
    }
}