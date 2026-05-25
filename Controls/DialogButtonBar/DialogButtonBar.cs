using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace LenWeaver.Utilities {

    [ContentProperty(nameof(Buttons))]
    public class DialogButtonBar : Control {

        public static readonly  DependencyProperty  ButtonsProperty =
                                DependencyProperty.Register( nameof(Buttons), typeof(DialogButtonBarCollection),
                                                             typeof(DialogButtonBar), new PropertyMetadata( null ) );

        public static readonly  RoutedEvent         ButtonClickedEvent =
                                EventManager.RegisterRoutedEvent( nameof(ButtonClicked), RoutingStrategy.Bubble,
                                                                  typeof(RoutedEventHandler), typeof(DialogButtonBar) );

        static DialogButtonBar() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(DialogButtonBar),
                                                      new FrameworkPropertyMetadata( typeof(DialogButtonBar) ) );
        }


        public DialogButtonBar() {

            Buttons = new DialogButtonBarCollection();
        }


        public event RoutedEventHandler ButtonClicked {
            add => AddHandler( ButtonClickedEvent, value );
            remove => RemoveHandler( ButtonClickedEvent, value );
        }

        public DialogButtonBarCollection Buttons {
            get => (DialogButtonBarCollection)GetValue( ButtonsProperty );
            set => SetValue( ButtonsProperty, value );
        }


        public override void OnApplyTemplate() {

            base.OnApplyTemplate();


            AddHandler( Button.ClickEvent, new RoutedEventHandler( OnButtonClick ) );
        }

        private void OnButtonClick( object sender, RoutedEventArgs e ) {

            if( e.OriginalSource is Button btn && btn.DataContext is DialogButton model ) {
                model.Clicked?.Invoke( model );

                if( model.Command?.CanExecute( model ) == true ) model.Command.Execute( model );
            }
        }
    }
}