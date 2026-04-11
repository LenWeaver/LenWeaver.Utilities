using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LenWeaver.Utilities {

    public class ApplicationSettingsException : Exception {

        public ApplicationSettingsException( [CallerMemberName] string? memberName = null,
                                             [CallerFilePath] string? filePath = null,
                                             [CallerLineNumber] int? lineNumber = null ) : base() {}
        public ApplicationSettingsException( string message ) : this() {}
        public ApplicationSettingsException( string message, Exception innerException ) : base( message, innerException ) {}
        


    }
}