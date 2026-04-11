using System;
using System.Collections.Generic;


namespace LenWeaver.Utilities {

    public enum SymbolViewSource {
        None,
        Character,          //Symbol source will be a character from a font, such as Segoe MDL2 Assets.
        Image,              //Symbol source will be an image.
        Path                //Symbol source will be path geometry.
    }
}