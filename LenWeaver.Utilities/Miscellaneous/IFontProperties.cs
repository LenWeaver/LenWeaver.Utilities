using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public interface IReadOnlyFontProperties {
        FontFamily      FontFamily      { get; }

        FontStretch     FontStretch     { get; }
        FontStyle       FontStyle       { get; }
        FontWeight      FontWeight      { get; }

        double          FontSize        { get; }
    }

    [Browsable( false )]
    [EditorBrowsable( EditorBrowsableState.Never )]
    [Description( "This interface is not intended to be used except in IFontProperties." )]
    public interface IWriteOnlyFontProperties {
        FontFamily      FontFamily      { set; }
                                          
        FontStretch     FontStretch     { set; }
        FontStyle       FontStyle       { set; }
        FontWeight      FontWeight      { set; }
                                          
        double          FontSize        { set; }
    }

    public interface IFontProperties : IReadOnlyFontProperties, IWriteOnlyFontProperties {}
}