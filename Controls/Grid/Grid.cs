using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LenWeaver.Utilities {

    public class Grid : System.Windows.Controls.Grid {

        private readonly Dictionary<string,int> columnKeys      = new();
        private readonly Dictionary<string,int> rowKeys         = new();


        public static readonly  DependencyProperty KeyProperty =
                                DependencyProperty.RegisterAttached( "Key", typeof(string),
                                                                     typeof(Grid), new PropertyMetadata( null, OnKeyChanged ) );

        public static readonly  DependencyProperty RowKeyProperty =
                                DependencyProperty.RegisterAttached( "RowKey", typeof(string),
                                                                     typeof(Grid), new PropertyMetadata( null, OnRowKeyChanged ) );

        public static readonly  DependencyProperty ColumnKeyProperty =
                                DependencyProperty.RegisterAttached( "ColumnKey", typeof(string),
                                                                     typeof(Grid), new PropertyMetadata( null, OnColumnKeyChanged ) );


        public Grid() {}


        protected override void OnInitialized                   ( EventArgs e ) {

            base.OnInitialized( e );

            RebuildRowKeys();
            RebuildColumnKeys();
        }


        public          void    RebuildRowKeys                  () {

            string?             name;

            RowDefinition?      rd;


            rowKeys.Clear();

            for( int i = 0; i < RowDefinitions.Count; i++ ) {
                rd      = RowDefinitions[i];
                name    = GetKey( rd );
                if( !String.IsNullOrEmpty( name ) )
                    rowKeys[name] = i;
            }

            UpdateChildrenRowAssignments();
        }
        public          void    RebuildColumnKeys               () {

            string?             name;

            ColumnDefinition?   cd;


            columnKeys.Clear();

            for( int i = 0; i < ColumnDefinitions.Count; i++ ) {
                cd      = ColumnDefinitions[i];
                name    = GetKey( cd );
                if( !String.IsNullOrEmpty( name ) ) {
                    columnKeys[name] = i;
                }
            }

            UpdateChildrenColumnAssignments();
        }


        private         void    ApplyRowKey                     ( UIElement child ) {

            string?     name;


            name = GetRowKey( child );
            if( name != null && rowKeys.TryGetValue( name, out int index ) ) {
                SetRow( child, index );
            }
        }
        private         void    ApplyColumnKey                  ( UIElement child ) {

            string?     name;


            name = GetColumnKey( child );
            if( name != null && columnKeys.TryGetValue( name, out int index ) ) {
                SetColumn( child, index );
            }
        }
        private         void    UpdateChildrenRowAssignments    () {

            foreach( UIElement child in Children ) {
                ApplyRowKey( child );
            }
        }
        private         void    UpdateChildrenColumnAssignments () {

            foreach( UIElement child in Children ) {
                ApplyColumnKey( child );
            }
        }

        public static   void    SetColumnKey                    ( DependencyObject obj, string value )  => obj.SetValue( ColumnKeyProperty, value );
        public static   void    SetKey                          ( DependencyObject obj, string value )  => obj.SetValue( KeyProperty, value );
        public static   void    SetRowKey                       ( DependencyObject obj, string value )  => obj.SetValue( RowKeyProperty, value );

        [AttachedPropertyBrowsableForChildren()]
        public static   string  GetColumnKey                    ( DependencyObject obj )                => (string)obj.GetValue( ColumnKeyProperty );
        [AttachedPropertyBrowsableForChildren()]
        public static   string  GetKey                          ( DependencyObject obj )                => (string)obj.GetValue( KeyProperty );
        [AttachedPropertyBrowsableForChildren()]
        public static   string  GetRowKey                       ( DependencyObject obj )                => (string)obj.GetValue( RowKeyProperty );

        private static  void    OnColumnKeyChanged              ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            Grid?       grid;


            if( d is UIElement child ) {
                grid = FindParentGrid( child );
                grid?.ApplyColumnKey( child );
            }
        }
        private static  void    OnKeyChanged                    ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            if( d is RowDefinition rd    && rd.Parent is Grid rg )  rg.RebuildRowKeys();
            if( d is ColumnDefinition cd && cd.Parent is Grid cg )  cg.RebuildColumnKeys();
        }
        private static  void    OnRowKeyChanged                 ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            Grid?       grid;


            if( d is UIElement child ) {
                grid = FindParentGrid( child );
                grid?.ApplyRowKey( child );
            }
        }

        private static  Grid?   FindParentGrid                  ( DependencyObject obj ) {

            while( obj != null ) {
                if( obj is Grid g ) return g;

                obj = LogicalTreeHelper.GetParent( obj );
            }

            return null;
        }
    }
}