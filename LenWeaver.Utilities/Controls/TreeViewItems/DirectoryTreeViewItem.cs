using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LenWeaver.Utilities {


    public class DirectoryTreeViewItem : TreeViewItem, IFileSystemEntry {

        public              DirectoryInfo           DirInfo         { get; }

        private readonly    Image                   img;

        #region Static Members
        protected static    EnumerationOptions?     enumOptions     = null;

        internal protected static    EnumerationOptions  EnumOptions {
            get {
                if( enumOptions is null ) {
                    enumOptions                     = new EnumerationOptions();
                    enumOptions.AttributesToSkip    = FileAttributes.System | FileAttributes.Hidden;
                }

                return enumOptions;
            }
        }
        #endregion

        protected DirectoryTreeViewItem                 ( DirectoryInfo dirInfo, bool checkForSubDirectories ) {

            StackPanel          sp;
            TextBlock           tb;


            DirInfo             = dirInfo;

            sp                  = new StackPanel();
            sp.Orientation      = Orientation.Horizontal;

            img                 = new Image();
            img.Margin          = new Thickness( 0d, 0d, 5d, 0d );
            img.Source          = new BitmapImage( ClosedFolderUri );
            img.Height          = 16d;
            img.Width           = 16d;

            tb                  = new TextBlock();
            tb.Text             = DirInfo.Name;

            sp.Children.Add( img );
            sp.Children.Add( tb );

            Header              = sp;
            Collapsed           += DirectoryTreeViewItem_Collapsed;
            Expanded            += DirectoryTreeViewItem_Expanded;

            if( checkForSubDirectories ) {
                RefreshSubDirectories( checkForSubDirectories );
            }
        }

        public DirectoryTreeViewItem                    ( DirectoryInfo dirInfo ) : this( dirInfo, true ) {}


        public void RefreshSubDirectories               ( bool includeSubDirectories ) {

            DirectoryInfo[]     subDirs;


            Items.Clear();

            subDirs         = DirInfo.GetDirectories( "*", DirectoryTreeViewItem.EnumOptions );

            for( int index = 0; index < subDirs.Length; index++ ) {
                Items.Add( new DirectoryTreeViewItem( subDirs[index], false ) );
            }
        }

        private void DirectoryTreeViewItem_Collapsed    ( object sender, System.Windows.RoutedEventArgs e ) {

            img.Source = new BitmapImage( ClosedFolderUri );
        }
        private void DirectoryTreeViewItem_Expanded     ( object sender, System.Windows.RoutedEventArgs e ) {

            if( e.Source is DirectoryTreeViewItem item ) {
                if( HasItems ) {
                    for( int index = 0; index < Items.Count; index++ ) {
                        if( Items[index] is DirectoryTreeViewItem subItem ) {
                            subItem.RefreshSubDirectories( true );
                        }
                    }
                }

                img.Source = new BitmapImage( OpenedFolderUri );
            }
        }


        public static Uri ClosedFolderUri   => new Uri( "/LenWeaver.Utilities;component/Images/Folder-Closed-01.png",   UriKind.RelativeOrAbsolute );
        public static Uri OpenedFolderUri   => new Uri( "/LenWeaver.Utilities;component/Images/Folder-01.png",          UriKind.RelativeOrAbsolute );

        public string Entry {
            get => DirInfo.Name.Replace( System.IO.Path.DirectorySeparatorChar.ToString(), null );
        }
    }
}