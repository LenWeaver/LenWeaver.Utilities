using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    [Flags()]
    public enum LoadingStrategies {
        ApplicationFolder           = 0b_0000_0001,
        LoadedAssemblies            = 0b_0000_0010,
        ReferencedAssemblies        = 0b_0000_0100,
        SpecifiedFolders            = 0b_0000_1000,


        All                         = ApplicationFolder | LoadedAssemblies | ReferencedAssemblies | SpecifiedFolders
    }
}