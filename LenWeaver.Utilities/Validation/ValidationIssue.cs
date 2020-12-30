using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class ValidationIssue {

        public  IssueSeverity   Severity        { get; private set; }

        public  string          Description     { get; private set; }
        public  string          Field           { get; private set; }


        public ValidationIssue( IssueSeverity severity, string field, string description ) {

            Severity        = severity;
            Field           = field;
            Description     = description;
        }
        public ValidationIssue( string field, string description ) : this( IssueSeverity.Warning, field, description ) {}


        public override string ToString() {

            return $"{Field} - {Severity} - {Description}";
        }
    }
}