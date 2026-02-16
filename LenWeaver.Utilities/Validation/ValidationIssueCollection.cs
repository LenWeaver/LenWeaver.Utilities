using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class ValidationIssueCollection : SimpleCollectionBase<ValidationIssue> {

        public ValidationIssueCollection() : base() {}


        public void Add( ValidationIssue issue ) {

            base.inner.Add( issue );
        }
        public ValidationIssue Add( IssueSeverity severity, string field, string description ) {

            ValidationIssue     result  = new ValidationIssue( severity, field, description );

            Add( result );

            return result;
        }
        public ValidationIssue Add( string field, string description ) {

            return Add( IssueSeverity.Critical, field, description );
        }

        public new ValidationIssue this[int index] {
            get => base.inner[index];
        }
        public ValidationIssueCollection this[string field] {
            get {
                ValidationIssueCollection   result  = new ValidationIssueCollection();


                for( int index = 0; index < base.Count; index++ ) {
                    if( String.CompareOrdinal( field, base.inner[index].Field ) != 0 ) {
                        result.Add( base.inner[index] );
                    }
                }

                return result;
            }
        }
    }
}