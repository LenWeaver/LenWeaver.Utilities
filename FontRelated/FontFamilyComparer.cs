using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace LenWeaver.Utilities {


    public sealed class FontFamilyComparer : IComparer<FontFamily> {

        public static readonly FontFamilyComparer Instance = new();

        private static readonly XmlLanguage Lang = XmlLanguage.GetLanguage( CultureInfo.CurrentUICulture.IetfLanguageTag );

        public int Compare( FontFamily? x, FontFamily? y ) {

            ArgumentNullException.ThrowIfNull( x );
            ArgumentNullException.ThrowIfNull( y );

            string nameX = x.FamilyNames.TryGetValue( Lang, out string? nx ) ? nx : x.Source;
            string nameY = y.FamilyNames.TryGetValue( Lang, out string? ny ) ? ny : y.Source;

            return String.Compare( nameX, nameY, StringComparison.OrdinalIgnoreCase );
        }
    }
}