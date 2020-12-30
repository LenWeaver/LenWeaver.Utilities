using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public enum ValidationMode {
        Quick,
        Comprehensive
    }


    public interface ITable {
        bool                        IsValid         { get; }

        string                      TableName       { get; }

        ValidationIssueCollection   Validate();
        ValidationIssueCollection   Validate( ValidationMode mode );
    }
}