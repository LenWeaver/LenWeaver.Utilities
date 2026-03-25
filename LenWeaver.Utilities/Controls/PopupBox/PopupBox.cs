using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LenWeaver.Utilities {

    [TemplatePart( Name = "PART_TextBox",       Type = typeof(TextBox) )]
    [TemplatePart( Name = "PART_ClearButton",   Type = typeof(Button) )]
    [TemplatePart( Name = "PART_PopupButton",   Type = typeof(Button) )]
    public class PopupBox : Control {

        private string                  unalteredDisplayText        = String.Empty;

        private Func<object,string>?    selectedObjectFormatter     = null;

        private TextBox?                PART_TextBox                = null;


        public delegate void SelectedObjectChangedRoutedEventHandler( object sender, SelectedObjectChangedRoutedEventArgs e );


        #region Static Members
        public static readonly  DependencyPropertyKey AvailableTextWidthPropertyKey =
                                DependencyProperty.RegisterReadOnly( nameof(AvailableTextWidth), typeof(double?), typeof(PopupBox),
                                                                     new PropertyMetadata( null ) );

        public static readonly  DependencyProperty  AvailableTextWidthProperty =
                                AvailableTextWidthPropertyKey.DependencyProperty;

        public static readonly  DependencyProperty  ClearCommandProperty =
                                DependencyProperty.Register( nameof(ClearCommand), typeof(ICommand), typeof(PopupBox),
                                                             new PropertyMetadata( null ) );

        public static readonly  DependencyProperty  EllipsisLocationProperty =
                                DependencyProperty.Register( nameof(EllipsisLocation), typeof(EllipsisLocation),
                                                             typeof(PopupBox), new PropertyMetadata( EllipsisLocation.End ) );

        public static readonly  DependencyProperty  EnableTextTrimmingProperty =
                                DependencyProperty.Register( nameof(EnableTextTrimming), typeof(bool),
                                                             typeof(PopupBox), new FrameworkPropertyMetadata( defaultValue: true,
                                                                                                                     flags: FrameworkPropertyMetadataOptions.AffectsArrange,
                                                                                                   propertyChangedCallback: EnabledTextTrimming_Changed ) );

        public static readonly  DependencyProperty  MinimumButtonWidthProperty =
                                DependencyProperty.Register( nameof(MinimumButtonWidth), typeof(double),
                                                             typeof(PopupBox), new PropertyMetadata( 24d ) );

        public static readonly  DependencyProperty  PopupCommandProperty =
                                DependencyProperty.Register( nameof(PopupCommand), typeof(ICommand), typeof(PopupBox),
                                                             new PropertyMetadata( null ) );

        public static readonly  DependencyProperty  SelectedObjectChangedCommandProperty =
                                DependencyProperty.Register( nameof(SelectedObjectChangedCommand), typeof(ICommand), typeof(PopupBox),
                                                             new PropertyMetadata( null ) );

        public static readonly  DependencyProperty  SelectedObjectProperty =
                                DependencyProperty.Register( nameof(SelectedObject), typeof(object),
                                                             typeof(PopupBox), new PropertyMetadata( null, SelectedObject_Changed ) );


        public static readonly  DependencyProperty  ShowClearButtonProperty =
                                DependencyProperty.Register( nameof(ShowClearButton), typeof(bool), typeof(PopupBox),
                                                             new PropertyMetadata( true ) );

        public static readonly  DependencyProperty  TextProperty =
                                DependencyProperty.Register( nameof(Text), typeof(string), typeof(PopupBox),
                                                             new FrameworkPropertyMetadata( String.Empty,
                                                                                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );
        

        public static readonly  RoutedEvent         ClearClickedEvent =
                                EventManager.RegisterRoutedEvent( nameof(ClearClicked), RoutingStrategy.Bubble,
                                                                  typeof(RoutedEventHandler), typeof(PopupBox) );

        public static readonly  RoutedEvent         PopupClickedEvent =
                                EventManager.RegisterRoutedEvent( nameof(PopupClicked), RoutingStrategy.Bubble,
                                                                  typeof(RoutedEventHandler), typeof(PopupBox) );

        public static readonly  RoutedEvent         SelectedObjectChangedEvent =
                                EventManager.RegisterRoutedEvent( nameof(SelectedObjectChanged), RoutingStrategy.Bubble,
                                                                  typeof(SelectedObjectChangedRoutedEventHandler), typeof(PopupBox) );

        #endregion


        static PopupBox() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(PopupBox),
                                                      new FrameworkPropertyMetadata( typeof(PopupBox) ) );
        }

        public PopupBox() {}


        public event RoutedEventHandler ClearClicked {
            add => AddHandler( ClearClickedEvent, value );
            remove => RemoveHandler( ClearClickedEvent, value );
        }
        public event RoutedEventHandler PopupClicked {
            add => AddHandler( PopupClickedEvent, value );
            remove => RemoveHandler( PopupClickedEvent, value );
        }
        public event SelectedObjectChangedRoutedEventHandler SelectedObjectChanged {
            add => AddHandler( SelectedObjectChangedEvent, value );
            remove => RemoveHandler( SelectedObjectChangedEvent, value );
        }


        public ICommand ClearCommand {
            get => (ICommand)GetValue( ClearCommandProperty );
            set => SetValue( ClearCommandProperty, value );
        }
        public ICommand PopupCommand {
            get => (ICommand)GetValue( PopupCommandProperty );
            set => SetValue( PopupCommandProperty, value );
        }
        public ICommand SelectedObjectChangedCommand {
            get => (ICommand)GetValue( SelectedObjectChangedCommandProperty );
            set => SetValue( SelectedObjectChangedCommandProperty, value );
        }


        public bool EnableTextTrimming {
            get => (bool)GetValue( EnableTextTrimmingProperty );
            set => SetValue( EnableTextTrimmingProperty, value );
        }
        public bool ShowClearButton {
            get => (bool)GetValue( ShowClearButtonProperty );
            set => SetValue( ShowClearButtonProperty, value );
        }

        public string Text {
            get => (string)GetValue( TextProperty );
            internal set => SetValue( TextProperty, value );
        }

        public EllipsisLocation EllipsisLocation {
            get => (EllipsisLocation)GetValue( EllipsisLocationProperty );
            set => SetValue( EllipsisLocationProperty, value );
        }

        public object? SelectedObject {
            get => GetValue( SelectedObjectProperty );
            set => SetValue( SelectedObjectProperty, value );
        }

        public Func<object,string>? SelectedObjectFormatter {
            get => selectedObjectFormatter;
            set => selectedObjectFormatter = value;
        }

        public double? AvailableTextWidth {
            get => (double?)GetValue( AvailableTextWidthProperty );
            internal set => SetValue( AvailableTextWidthPropertyKey, value );
        }
        public double MinimumButtonWidth {
            get => (double)GetValue( MinimumButtonWidthProperty );
            set => SetValue( MinimumButtonWidthProperty, value );
        }

        public string UnalteredDisplayText {
            get => unalteredDisplayText;
            set => unalteredDisplayText = value;
        }


        public override void OnApplyTemplate() {

            base.OnApplyTemplate();


            PART_TextBox = GetTemplateChild( nameof(PART_TextBox) ) as TextBox ?? 
                                             throw new MissingMemberException( $"{nameof(PART_TextBox)} not found in {nameof(PopupBox)} style template." );


            if( GetTemplateChild( "PART_PopupButton" ) is Button popupButton ) {

                popupButton.Click += (s, e) => {

                    RaiseEvent( new RoutedEventArgs( PopupClickedEvent ) );

                    if( PopupCommand?.CanExecute( null ) == true ) PopupCommand.Execute( null );
                };
            }

            if( GetTemplateChild( "PART_ClearButton" ) is Button clearButton ) {

                clearButton.Click += (s, e) => {

                    if( ClearCommand?.CanExecute( null ) == true ) {
                        ClearCommand.Execute( null );
                    }
                    else {
                        Text            = String.Empty;
                        SelectedObject  = null;
                    }
                };
            }
        }

        protected override Size ArrangeOverride( Size arrangeBounds ) {

            RecalculateAvailableTextWidth( arrangeBounds.Width );
            UpdateDisplayText( UnalteredDisplayText );

            return base.ArrangeOverride( arrangeBounds );
        }
        protected void InvokeSelectedObjectChanged( object? oldValue, object? newValue) {

            bool                    complete        = false;

            string?                 displayText     = Text;

            PopupCommandParameters  parameters;


            if( newValue is not null ) {
                UnalteredDisplayText    = newValue?.ToString() ?? String.Empty;

                if( selectedObjectFormatter is not null && newValue is not null ) {
                    displayText         = selectedObjectFormatter( newValue );

                    complete            = true;
                }
            
                if( SelectedObjectChangedCommand is not null ) {
                    parameters          = new PopupCommandParameters( newValue, oldValue );

                    if( SelectedObjectChangedCommand.CanExecute( parameters ) ) {
                        SelectedObjectChangedCommand.Execute( parameters );

                        complete        = parameters.Handled;

                        if( complete ) displayText = parameters.DisplayText;
                    }
                }

                SelectedObjectChangedRoutedEventArgs args = new( SelectedObjectChangedEvent );

                args.NewValue           = newValue;
                args.OldValue           = oldValue;
                args.DisplayText        = Text;

                RaiseEvent( args );

                if( args.Handled ) {
                    complete        = true;

                    displayText     = args.DisplayText;
                }

                if( !complete ) displayText = SelectedObject?.ToString() ?? String.Empty;
            }

            UpdateDisplayText( displayText );
        }
        protected void RecalculateAvailableTextWidth( double controlWidth ) {

            double unusable  = MinimumButtonWidth;


            unusable        += (PART_TextBox?.BorderThickness.Left  ?? 0d)  +   (PART_TextBox?.BorderThickness.Right    ?? 0d);
            unusable        += (PART_TextBox?.Padding.Left          ?? 0d)  +   (PART_TextBox?.Padding.Right            ?? 0d);
            unusable        += ShowClearButton                       ?          MinimumButtonWidth                       : 0d;


            AvailableTextWidth = Math.Max( 0d, controlWidth - unusable );
        }
        protected void UpdateDisplayText( string displayText ) {
            
            double              displayTextWidth;

            FontDescriptor      fd;


            if( PART_TextBox is null ) throw new NullReferenceException( $"{nameof(PopupBox)}.{nameof(PART_TextBox)} is null in {nameof(UpdateDisplayText)}." );

            UnalteredDisplayText    = displayText;

            if( EnableTextTrimming ) {
                fd                  = new FontDescriptor( PART_TextBox );

                displayTextWidth    = fd.MeasureText( displayText );

                if( displayTextWidth > AvailableTextWidth ) {
                    displayText     = fd.TruncateWithEllipsis( displayText, AvailableTextWidth.Value,
                                                               FontDescriptor.EllipsisChar, EllipsisLocation );
                }
            }

            Text                    = displayText;
        }

        private static void EnabledTextTrimming_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            if( d is PopupBox pb ) {
                pb.InvalidateArrange();
                pb.InvokeSelectedObjectChanged( null, pb.SelectedObject );
            }
        }
        private static void SelectedObject_Changed( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            ((PopupBox)d).InvokeSelectedObjectChanged( e.OldValue, e.NewValue );
        }
    }
}