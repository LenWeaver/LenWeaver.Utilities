using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LenWeaver.Utilities {

    [TemplatePart( Name = "PART_TextBox",       Type = typeof(TextBox) )]
    [TemplatePart( Name = "PART_ClearButton",   Type = typeof(Button) )]
    [TemplatePart( Name = "PART_PopupButton",   Type = typeof(Button) )]
    public class PopupBox : Control {

        #region Static Members
        public static readonly  DependencyProperty MinimumButtonWidthProperty =
                                DependencyProperty.Register( nameof(MinimumButtonWidth), typeof(double),
                                                             typeof(PopupBox), new PropertyMetadata( 24d ) );

        public static readonly  DependencyProperty ClearCommandProperty =
                                DependencyProperty.Register( nameof(ClearCommand), typeof(ICommand), typeof(PopupBox),
                                                             new PropertyMetadata( null ) );

        public static readonly  DependencyProperty PopupCommandProperty =
                                DependencyProperty.Register( nameof(PopupCommand), typeof(ICommand), typeof(PopupBox),
                                                             new PropertyMetadata( null ) );

        public static readonly  DependencyProperty ShowClearButtonProperty =
                                DependencyProperty.Register( nameof(ShowClearButton), typeof(bool), typeof(PopupBox),
                                                             new PropertyMetadata( true ) );

        public static readonly  DependencyProperty TextProperty =
                                DependencyProperty.Register( nameof(Text), typeof(string), typeof(PopupBox),
                                                             new FrameworkPropertyMetadata( String.Empty,
                                                                                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );


        public static readonly  RoutedEvent ClearClickedEvent =
                                EventManager.RegisterRoutedEvent( nameof(ClearClicked), RoutingStrategy.Bubble,
                                                                  typeof(RoutedEventHandler), typeof(PopupBox) );

        public static readonly  RoutedEvent PopupClickedEvent =
                                EventManager.RegisterRoutedEvent( nameof(PopupClicked), RoutingStrategy.Bubble,
                                                                  typeof(RoutedEventHandler), typeof(PopupBox) );

        #endregion


        static PopupBox() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(PopupBox),
                                                      new FrameworkPropertyMetadata( typeof(PopupBox) ) );
        }


        public event RoutedEventHandler ClearClicked {
            add => AddHandler( ClearClickedEvent, value );
            remove => RemoveHandler( ClearClickedEvent, value );
        }
        public event RoutedEventHandler PopupClicked {
            add => AddHandler( PopupClickedEvent, value );
            remove => RemoveHandler( PopupClickedEvent, value );
        }


        public ICommand ClearCommand {
            get => (ICommand)GetValue( ClearCommandProperty );
            set => SetValue( ClearCommandProperty, value );
        }
        public ICommand PopupCommand {
            get => (ICommand)GetValue( PopupCommandProperty );
            set => SetValue( PopupCommandProperty, value );
        }


        public bool ShowClearButton {
            get => (bool)GetValue( ShowClearButtonProperty );
            set => SetValue( ShowClearButtonProperty, value );
        }
        public double MinimumButtonWidth {
            get => (double)GetValue( MinimumButtonWidthProperty );
            set => SetValue( MinimumButtonWidthProperty, value );
        }

        public string Text {
            get => (string)GetValue( TextProperty );
            set => SetValue( TextProperty, value );
        }


        public override void OnApplyTemplate() {

            base.OnApplyTemplate();

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
                        Text = string.Empty;
                    }
                };
            }
        }
    }
}