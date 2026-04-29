using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LenWeaver.Utilities {

    public class ApplicationSettingsException : Exception {

        public  int?        LineNumber      { get; internal set; }

        public  string?     FilePath        { get; internal set; }
        public  string?     MemberName      { get; internal set; }


        public ApplicationSettingsException( string message,
                                             [CallerMemberName] string? memberName  = null,
                                             [CallerFilePath]   string? filePath    = null,
                                             [CallerLineNumber] int?    lineNumber  = null ) : base( message ) {
        
            MemberName      = memberName;
            FilePath        = filePath;

            LineNumber      = lineNumber;
        }
        public ApplicationSettingsException( string message,
                                             Exception innerException,
                                             [CallerMemberName] string? memberName  = null,
                                             [CallerFilePath]   string? filePath    = null,
                                             [CallerLineNumber] int? lineNumber     = null ): base( message, innerException ) {

            MemberName      = memberName;
            FilePath        = filePath;

            LineNumber      = lineNumber;
        }
    }
}