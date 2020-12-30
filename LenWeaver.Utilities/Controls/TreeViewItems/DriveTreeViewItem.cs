using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LenWeaver.Utilities {

    public class DriveTreeViewItem : TreeViewItem, IFileSystemEntry {

        public  DriveInfo       DriveInfo   { get; }


        public DriveTreeViewItem( DriveInfo driveInfo ) {

            Image           img;
            StackPanel      sp;
            TextBlock       tb;


            DriveInfo       = driveInfo;

            sp              = new StackPanel();
            sp.Orientation  = Orientation.Horizontal;

            img             = new Image();
            img.Source      = new BitmapImage( GetDriveImage( DriveInfo.DriveType ) );
            img.Margin      = new Thickness( 0d, 0d, 5d, 0d );
            img.Height      = 16d;
            img.Width       = 16d;

            tb              = new TextBlock();
            tb.Text         = $"{DriveInfo.Name} - {DriveInfo.VolumeLabel}";

            sp.Children.Add( img );
            sp.Children.Add( tb );

            Header          = sp;

            PopulateDirectoryList( Items, DriveInfo.RootDirectory.GetDirectories( "*", DirectoryTreeViewItem.EnumOptions ) );
        }
        

        public static Uri DriveImageFixed       => new Uri( "/LenWeaver.Utilities;component/Image/Drive-Fixed-01.png",      UriKind.RelativeOrAbsolute );
        public static Uri DriveImageCDRom       => new Uri( "/LenWeaver.Utilities;component/Image/Drive-CDRom-01.png",      UriKind.RelativeOrAbsolute );
        public static Uri DriveImageNetwork     => new Uri( "/LenWeaver.Utilities;component/Image/Drive-Network-01.png",    UriKind.RelativeOrAbsolute );
        public static Uri DriveImageRemovable   => new Uri( "/LenWeaver.Utilities;component/Image/Drive-Removable-01.png",  UriKind.RelativeOrAbsolute );
        public static Uri DriveImageUnknown     => new Uri( "/LenWeaver.Utilities;component/Image/Drive-Unknown-01.png",    UriKind.RelativeOrAbsolute );

        public string Entry {
            get => DriveInfo.Name.Replace( System.IO.Path.DirectorySeparatorChar.ToString(), null );
        }

        public static Uri GetDriveImage( DriveType driveType ) {

            Uri         result;


            switch( driveType ) {
                case DriveType.Removable:   result  = DriveImageRemovable;      break;
                case DriveType.Fixed:       result  = DriveImageFixed;          break;
                case DriveType.Network:     result  = DriveImageNetwork;        break;
                case DriveType.CDRom:       result  = DriveImageCDRom;          break;

                default:                    result  = DriveImageUnknown;        break;
            }

            return result;
        }

        internal static void PopulateDirectoryList( ItemCollection items, DirectoryInfo[] folders ) {

            foreach( DirectoryInfo dirInfo in folders ) {
                items.Add( new DirectoryTreeViewItem( dirInfo ) );
            }
        }
    }
}