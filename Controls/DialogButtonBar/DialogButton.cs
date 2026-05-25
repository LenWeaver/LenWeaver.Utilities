using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LenWeaver.Utilities {

    public class DialogButton : DependencyObject {

        #region Dependency Property Declarations
        public static readonly  DependencyProperty CommandProperty =
                                DependencyProperty.Register( nameof(Command), typeof(ICommand), typeof(DialogButton) );

        public static readonly  DependencyProperty IsCancelProperty =
                                DependencyProperty.Register( nameof(IsCancel), typeof(bool), typeof(DialogButton) );

        public static readonly  DependencyProperty IsDefaultProperty =
                                DependencyProperty.Register( nameof(IsDefault), typeof(bool), typeof(DialogButton) );

        public static readonly  DependencyProperty MinWidthProperty =
                                DependencyProperty.Register( nameof(MinWidth), typeof(double), typeof(DialogButton),
                                                             new PropertyMetadata( 75d ) );

        public static readonly  DependencyProperty TextProperty =
                                DependencyProperty.Register( nameof(Text), typeof(string), typeof(DialogButton) );
        #endregion


        public Action<DialogButton>? Clicked { get; set; }


        public DialogButton() : this( String.Empty, false, false, null, null ) {}
        public DialogButton( string text, bool isDefault = false, bool isCancel = false,
                             ICommand? command = null, Action<DialogButton>? clicked = null ) {

            Text        = text;
            IsDefault   = isDefault;
            IsCancel    = isCancel;
            Command     = command;
            Clicked     = clicked;
        }


        public bool         IsCancel    {
            get => (bool)GetValue( IsCancelProperty );
            set => SetValue( IsCancelProperty, value );
        }
        public bool         IsDefault   {
            get => (bool)GetValue( IsDefaultProperty );
            set => SetValue( IsDefaultProperty, value );
        }

        public double       MinWidth    {
            get => (double)GetValue( MinWidthProperty );
            set => SetValue( MinWidthProperty, value );
        }

        public string       Text        {
            get => (string)GetValue( TextProperty );
            set => SetValue( TextProperty, value );
        }

        public ICommand?    Command     {
            get => (ICommand?)GetValue( CommandProperty );
            set => SetValue( CommandProperty, value );
        }
    }
}