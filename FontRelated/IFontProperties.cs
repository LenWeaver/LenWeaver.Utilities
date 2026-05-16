using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public interface IReadOnlyFontProperties {
        FontFamily          FontFamily      { get; }

        FontStretch         FontStretch     { get; }
        FontStyle           FontStyle       { get; }
        FontWeight          FontWeight      { get; }

        double              FontSize        { get; }
    }

    public interface IFontProperties : IReadOnlyFontProperties {
        new FontFamily      FontFamily      { get; set; }
                                               
        new FontStretch     FontStretch     { get; set; }
        new FontStyle       FontStyle       { get; set; }
        new FontWeight      FontWeight      { get; set; }
                                               
        new double          FontSize        { get; set; }
    }
}