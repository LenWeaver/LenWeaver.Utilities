using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class SortDescriptor {

        public bool     SortAscending       { get; }

        public string   ColumnName          { get; }


        public SortDescriptor( string columnName, bool sortAscending ) {

            ColumnName      = columnName;
            SortAscending   = sortAscending;
        }
        public SortDescriptor( string columnName ) : this( columnName, true ) {}


        public (string columnName,bool sortAscending) Deconstruct() => (ColumnName,SortAscending);


        public override string ToString() => ColumnName;
    }
}