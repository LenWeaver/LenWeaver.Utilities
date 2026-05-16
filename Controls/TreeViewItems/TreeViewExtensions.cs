using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class TreeViewExtensions {

        #region Dependency Members
        public static readonly      DependencyProperty      NoChildItemsCheckedBrushProperty =
                                    DependencyProperty.RegisterAttached( "NoChildItemsCheckedBrush", typeof(Brush), typeof(TreeView),
                                                                         new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      SomeChildItemsCheckedBrushProperty =
                                    DependencyProperty.RegisterAttached( "SomeChildItemsCheckedBrush", typeof(Brush), typeof(TreeView),
                                                                         new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      AllChildItemsCheckedBrushProperty =
                                    DependencyProperty.RegisterAttached( "AllChildItemsCheckedBrush", typeof(Brush), typeof(TreeView),
                                                                         new PropertyMetadata( Brushes.Black ) );
        #endregion


        static TreeViewExtensions() {}


        extension( TreeViewItem? item ) {
            public TreeViewItem? ParentItem {
                get {
                    DependencyObject    depObj;

                    TreeViewItem        result;


                    ArgumentNullException.ThrowIfNull( item );

                    result  = item;
                    depObj  = VisualTreeHelper.GetParent( item );

                    while( depObj != null ) {
                        if( depObj is TreeViewItem ) {
                            result = (TreeViewItem)depObj;
                        }

                        depObj = VisualTreeHelper.GetParent( depObj );
                    }

                    return result;
                }
            }
        }

        public static void SetNoChildItemsCheckedBrush      ( DependencyObject obj, Brush newValue ) {

            obj.SetValue( NoChildItemsCheckedBrushProperty, newValue );
        }
        public static void SetSomeChildItemsCheckedBrush    ( DependencyObject obj, Brush newValue ) {

            obj.SetValue( SomeChildItemsCheckedBrushProperty, newValue );
        }
        public static void SetAllChildItemsCheckedBrush     ( DependencyObject obj, Brush newValue ) {

            obj.SetValue( AllChildItemsCheckedBrushProperty, newValue );
        }

        public static Brush GetNoChildItemsCheckedBrush     ( DependencyObject obj ) {

            return (Brush)obj.GetValue( NoChildItemsCheckedBrushProperty );
        }
        public static Brush GetSomeChildItemsCheckedBrush   ( DependencyObject obj ) {

            return (Brush)obj.GetValue( SomeChildItemsCheckedBrushProperty );
        }
        public static Brush GetAllChildItemsCheckedBrush    ( DependencyObject obj ) {

            return (Brush)obj.GetValue( AllChildItemsCheckedBrushProperty );
        }
    }
}