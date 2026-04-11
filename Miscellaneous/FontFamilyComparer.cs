using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public class FontFamilyComparer : IComparer<FontFamily> {


        public int Compare( FontFamily? x, FontFamily? y ) {

            ArgumentNullException.ThrowIfNull( x );
            ArgumentNullException.ThrowIfNull( y );

            return x.Source.CompareTo( y.Source );
        }
    }
}