using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LenWeaver.Utilities {

    public class FolderList : SimpleCollectionBase<string> {

        public  bool        VerifyExistence         { get; set; }   = true;


        public FolderList() : base() {}


        public void Add( string folderPath ) {

            
            if( VerifyExistence ) {
                if( !Directory.Exists( folderPath ) ) {
                    throw new DirectoryNotFoundException( $"The specified folder ({folderPath}) does not exist." );
                }
            }
            else {
                ArgumentException.ThrowIfNullOrWhiteSpace( folderPath );

                if( folderPath.ContainsAny( Path.GetInvalidPathChars() ) ) {
                    throw ExceptionBuilder.Create<InvalidOperationException>( "Specified folder name contains invalid characters." )
                                          .AddData( nameof(folderPath), folderPath );
                }

                if( base.inner.FindIndex( f => f.Equals( folderPath, StringComparison.OrdinalIgnoreCase ) ) != -1 ) {
                    throw new ArgumentException( $"The specified folder path ({folderPath}) is already in the folder collection." );
                }
            }

            base.inner.Add( folderPath );
        }
        public void Clear() => base.ClearList();


        public override string ToString() => $"FolderList contains {base.Count} item(s).";
    }
}