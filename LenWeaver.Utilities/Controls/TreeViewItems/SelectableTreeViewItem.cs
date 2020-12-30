using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LenWeaver.Utilities {

    public class SelectableTreeViewItem : TreeViewItem {

        #region Static Dependency Members
        public static readonly      DependencyProperty          ExpanderBrushProperty =
                                    DependencyProperty.Register( nameof(ExpanderBrush), typeof(Brush), typeof(SelectableTreeViewItem),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty          IsCheckedProperty =
                                    DependencyProperty.Register( nameof(IsChecked), typeof(bool), typeof(SelectableTreeViewItem),
                                                                 new PropertyMetadata( false ) );

        public static readonly      DependencyProperty          RadioButtonGroupNameProperty =
                                    DependencyProperty.Register( nameof(RadioButtonGroupName), typeof(string), typeof(SelectableTreeViewItem),
                                                                 new PropertyMetadata( "" ) );

        public static readonly      DependencyProperty          SelectableModeProperty =
                                    DependencyProperty.Register( nameof(SelectableMode), typeof(SelectableTreeViewItemMode),
                                                                 typeof(SelectableTreeViewItem),
                                                                 new PropertyMetadata( SelectableTreeViewItemMode.GroupHeading,
                                                                                       SelectableMode_Changed ) );


        private static void         SelectableMode_Changed      ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            CheckBox            cb;
            RadioButton         rb;


            if( d is SelectableTreeViewItem item && item.Template != null ) {
                cb = (CheckBox)item.Template.FindName( "cbChecked", item );
                rb = (RadioButton)item.Template.FindName( "rbChecked", item );

                if( cb != null && rb != null ) {
                    switch( (SelectableTreeViewItemMode)e.NewValue ) {
                        case SelectableTreeViewItemMode.GroupHeading:
                            cb.Visibility   = Visibility.Collapsed;
                            rb.Visibility   = Visibility.Collapsed;
                            break;

                        case SelectableTreeViewItemMode.RadioButton:
                            cb.Visibility   = Visibility.Collapsed;
                            rb.Visibility   = Visibility.Visible;
                            break;

                        case SelectableTreeViewItemMode.CheckBox:
                            cb.Visibility   = Visibility.Visible;
                            rb.Visibility   = Visibility.Collapsed;
                            break;
                    }
                }
            }
        }
        #endregion


        static SelectableTreeViewItem() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(SelectableTreeViewItem),
                                                      new FrameworkPropertyMetadata( typeof(SelectableTreeViewItem) ) );
        }

        private SelectableTreeViewItem( string text, string groupName, SelectableTreeViewItemMode mode ) : base() {

            Header                  = text;
            RadioButtonGroupName    = groupName;
            SelectableMode          = mode;

            Loaded                  += SelectableTreeViewItem_Loaded;
        }

        public SelectableTreeViewItem( string text, SelectableTreeViewItemMode mode) : this( text, "", mode ) {}
        public SelectableTreeViewItem( string text, string groupName ) : this( text, groupName, SelectableTreeViewItemMode.RadioButton ) {}
        public SelectableTreeViewItem( string text ) : this( text, "", SelectableTreeViewItemMode.GroupHeading ) {}
        public SelectableTreeViewItem() : this( "", "", SelectableTreeViewItemMode.GroupHeading ) {}


        public override string ToString() {

            return $"{Header.ToString()} - {IsChecked} - {Items.Count}";
        }
        internal void UpdateExpanderBrush() {

            int                 checkBoxItems               = 0;
            int                 checkedCheckBoxItems        = 0;
            int                 radioButtonGroups           = 0;
            int                 radioButtonCompleteGroups   = 0;

            Brush?              brush                       = null;
            TreeView?           tvw;

            HashSet<string>     groupNames                  = new HashSet<string>();


            for( int index = 0; index < Items.Count; index++ ) {
                if( Items[index] is SelectableTreeViewItem item ) {
                    switch( item.SelectableMode ) {
                        case SelectableTreeViewItemMode.CheckBox:
                            checkBoxItems++;
                            if( item.IsChecked ) checkedCheckBoxItems++;
                            break;

                        case SelectableTreeViewItemMode.RadioButton:
                            if( !groupNames.Contains( item.RadioButtonGroupName ) ) {
                                radioButtonGroups++;
                                groupNames.Add( item.RadioButtonGroupName );
                            }

                            if( item.IsChecked ) {
                                radioButtonCompleteGroups++;
                            }
                            break;
                    }
                }
            }
            
            tvw = LogicalTreeHelper.GetParent( this ) as TreeView;
            if( tvw != null ) {
                if( checkedCheckBoxItems == 0 && radioButtonCompleteGroups == 0 ) {
                    brush = TreeViewExtensions.GetNoChildItemsCheckedBrush( tvw );
                }
                else if( (checkedCheckBoxItems > 0 && checkedCheckBoxItems < checkBoxItems) ||
                         (radioButtonCompleteGroups > 0 && radioButtonCompleteGroups < radioButtonGroups) ) {
                    brush = TreeViewExtensions.GetSomeChildItemsCheckedBrush( tvw );
                }
                else if( checkedCheckBoxItems == checkBoxItems && radioButtonCompleteGroups == radioButtonGroups ) {
                    brush = TreeViewExtensions.GetAllChildItemsCheckedBrush( tvw );
                }

                if( brush != null ) ExpanderBrush = brush;
            }
        }
        protected override void OnItemsChanged( NotifyCollectionChangedEventArgs e ) {

            base.OnItemsChanged( e );

            UpdateExpanderBrush();
        }


        public bool                         IsChecked {
            get { return (bool)GetValue( IsCheckedProperty ); }
            set { SetValue( IsCheckedProperty, value ); }
        }
        public string                       RadioButtonGroupName {
            get { return (string)GetValue( RadioButtonGroupNameProperty ); }
            set { SetValue( RadioButtonGroupNameProperty, value ); }
        }
        public SelectableTreeViewItemMode   SelectableMode {
            get { return (SelectableTreeViewItemMode)GetValue( SelectableModeProperty ); }
            set { SetValue( SelectableModeProperty, value ); }
        }
        public Brush                        ExpanderBrush {
            get { return (Brush)GetValue( ExpanderBrushProperty ); }
            set { SetValue( ExpanderBrushProperty, value ); }
        }


        private void SelectableTreeViewItem_Loaded  ( object sender, RoutedEventArgs e ) {

            ToggleButton?       btn;


            btn                 = this.Template?.FindName( "cbChecked", this ) as ToggleButton;
            if( btn != null ) {
                btn.Checked     += CheckedControl_Checked;
                btn.Unchecked   += CheckedControl_Unchecked;
            }

            btn                 = this.Template?.FindName( "rbChecked", this ) as ToggleButton;
            if( btn != null ) {
                btn.Checked     += CheckedControl_Checked;
                btn.Unchecked   += CheckedControl_Unchecked;
            }
        }
        private void CheckedControl_Checked         ( object sender, RoutedEventArgs e ) {

            IsChecked = true;

            if( Parent is SelectableTreeViewItem item ) {
                item.UpdateExpanderBrush();
            }
        }
        private void CheckedControl_Unchecked       ( object sender, RoutedEventArgs e ) {

            IsChecked = false;

            if( Parent is SelectableTreeViewItem item ) {
                item.UpdateExpanderBrush();
            }
        }
    }
}