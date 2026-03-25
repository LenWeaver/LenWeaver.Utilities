using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    [ContentProperty( "Content" )]
    public class CaptionBar : Control {

        private                 Button?         btnClose                        = null;
        private                 Button?         btnIcon                         = null;
        private                 Button?         btnMaximize                     = null;
        private                 Button?         btnMinimize                     = null;
        private                 Button?         btnRestore                      = null;
        private                 Window?         window                          = null;


        private static readonly double          DefaultCloseButtonWidth         = 45d;
        private static readonly double          DefaultMaximizeButtonWidth      = 35d;
        private static readonly double          DefaultMinimizeButtonWidth      = 35d;
        private static readonly double          DefaultRestoreButtonWidth       = 35d;
        private static readonly string          DefaultTitle                    = String.Empty;
        private static readonly object?         DefaultContent                  = null;
        private static readonly Brush           DefaultCloseButtonBackground    = Brushes.OrangeRed;
        private static readonly Brush           DefaultCloseButtonForeground    = Brushes.White;
        private static readonly Brush           DefaultMaximizeButtonBackground = Brushes.Lime;
        private static readonly Brush           DefaultMaximizeButtonForeground = Brushes.White;
        private static readonly Brush           DefaultMinimizeButtonBackground = Brushes.Lime;
        private static readonly Brush           DefaultMinimizeButtonForeground = Brushes.White;
        private static readonly Brush           DefaultRestoreButtonBackground  = Brushes.Lime;
        private static readonly Brush           DefaultRestoreButtonForeground  = Brushes.White;
        private static readonly ImageSource?    DefaultImageSource              = null;


        public static readonly                  DependencyProperty CloseButtonBackgroundProperty =
                                                DependencyProperty.Register( nameof(CloseButtonBackground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultCloseButtonBackground ) );

        public static readonly                  DependencyProperty CloseButtonForegroundProperty =
                                                DependencyProperty.Register( nameof(CloseButtonForeground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultCloseButtonForeground ) );

        public static readonly                  DependencyProperty CloseButtonWidthProperty =
                                                DependencyProperty.Register( nameof(CloseButtonWidth), typeof(double), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultCloseButtonWidth ) );

        public static readonly                  DependencyProperty ContentProperty =
                                                DependencyProperty.Register( nameof(Content), typeof(object), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultContent ) );

        public static readonly                  DependencyProperty IconImageSourceProperty =
                                                DependencyProperty.Register( nameof(IconImageSource), typeof(ImageSource), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultImageSource ) );

        public static readonly                  DependencyProperty MaximizeButtonBackgroundProperty =
                                                DependencyProperty.Register( nameof(MaximizeButtonBackground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultMaximizeButtonBackground ) );

        public static readonly                  DependencyProperty MaximizeButtonForegroundProperty =
                                                DependencyProperty.Register( nameof(MaximizeButtonForeground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultMaximizeButtonForeground ) );

        public static readonly                  DependencyProperty MaximizeButtonWidthProperty =
                                                DependencyProperty.Register( nameof(MaximizeButtonWidth), typeof(double), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultMaximizeButtonWidth ) );

        public static readonly                  DependencyProperty MinimizeButtonBackgroundProperty =
                                                DependencyProperty.Register( nameof(MinimizeButtonBackground), typeof(Brush), typeof(CaptionBar), 
                                                new PropertyMetadata( DefaultMinimizeButtonBackground ) );

        public static readonly                  DependencyProperty MinimizeButtonForegroundProperty =
                                                DependencyProperty.Register( nameof(MinimizeButtonForeground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultMinimizeButtonForeground ) );

        public static readonly                  DependencyProperty MinimizeButtonWidthProperty =
                                                DependencyProperty.Register( nameof(MinimizeButtonWidth), typeof(double), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultMinimizeButtonWidth ) );

        public static readonly                  DependencyProperty RestoreButtonBackgroundProperty =
                                                DependencyProperty.Register( nameof(RestoreButtonBackground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultRestoreButtonBackground ) );

        public static readonly                  DependencyProperty RestoreButtonForegroundProperty =
                                                DependencyProperty.Register( nameof(RestoreButtonForeground), typeof(Brush), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultRestoreButtonForeground ) );

        public static readonly                  DependencyProperty RestoreButtonWidthProperty =
                                                DependencyProperty.Register( nameof(RestoreButtonWidth), typeof(double), typeof(CaptionBar),
                                                new PropertyMetadata( DefaultRestoreButtonWidth ) );

        public static readonly                  DependencyProperty TitleProperty =
                                                DependencyProperty.Register( nameof(Title), typeof(string), typeof(CaptionBar), 
                                                new PropertyMetadata( DefaultTitle ) );


        static CaptionBar() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(CaptionBar), new FrameworkPropertyMetadata( typeof(CaptionBar) ) );

        }

        public CaptionBar() {

            Loaded += CaptionBar_Loaded;
        }


        public double           CloseButtonWidth {
            get { return (double)GetValue( CloseButtonWidthProperty ); }
            set { SetValue( CloseButtonWidthProperty, value ); }
        }
        public double           MaximizeButtonWidth {
            get { return (double)GetValue( MaximizeButtonWidthProperty ); }
            set { SetValue( MaximizeButtonWidthProperty, value ); }
        }
        public double           MinimizeButtonWidth {
            get { return (double)GetValue( MinimizeButtonWidthProperty ); }
            set { SetValue( MinimizeButtonWidthProperty, value ); }
        }
        public double           RestoreButtonWidth {
            get { return (double)GetValue( RestoreButtonWidthProperty ); }
            set { SetValue( RestoreButtonWidthProperty, value ); }
        }
        public string           Title {
            get { return (string)GetValue( TitleProperty ); }
            set { SetValue( TitleProperty, value ); }
        }
        public object           Content {
            get { return (object)GetValue( ContentProperty ); }
            set { SetValue( ContentProperty, value ); }
        }
        public Brush            CloseButtonBackground {
            get { return (Brush)GetValue( CloseButtonBackgroundProperty ); }
            set { SetValue( CloseButtonBackgroundProperty, value ); }
        }
        public Brush            CloseButtonForeground {
            get { return (Brush)GetValue( CloseButtonForegroundProperty ); }
            set { SetValue( CloseButtonForegroundProperty, value ); }
        }
        public Brush            MaximizeButtonBackground {
            get { return (Brush)GetValue( MaximizeButtonBackgroundProperty ); }
            set { SetValue( MaximizeButtonBackgroundProperty, value ); }
        }
        public Brush            MaximizeButtonForeground {
            get { return (Brush)GetValue( MaximizeButtonForegroundProperty ); }
            set { SetValue( MaximizeButtonForegroundProperty, value ); }
        }
        public Brush            MinimizeButtonBackground {
            get { return (Brush)GetValue( MinimizeButtonBackgroundProperty ); }
            set { SetValue( MinimizeButtonBackgroundProperty, value ); }
        }
        public Brush            MinimizeButtonForeground {
            get { return (Brush)GetValue( MinimizeButtonForegroundProperty ); }
            set { SetValue( MinimizeButtonForegroundProperty, value ); }
        }
        public Brush            RestoreButtonBackground {
            get { return (Brush)GetValue( RestoreButtonBackgroundProperty ); }
            set { SetValue( RestoreButtonBackgroundProperty, value ); }
        }
        public Brush            RestoreButtonForeground {
            get { return (Brush)GetValue( RestoreButtonForegroundProperty ); }
            set { SetValue( RestoreButtonForegroundProperty, value ); }
        }
        public ImageSource      IconImageSource {
            get { return (ImageSource)GetValue( IconImageSourceProperty ); }
            set { SetValue( IconImageSourceProperty, value ); }
        }


        private void SetVisibilityWindowButtons() {

            if( window != null && btnMaximize != null && btnMinimize != null && btnRestore != null ) { 
                switch( window.WindowState ) {
                    case WindowState.Maximized:
                        btnMaximize.Visibility  = Visibility.Collapsed;
                        btnMinimize.Visibility  = Visibility.Visible;
                        btnRestore.Visibility   = Visibility.Visible;
                        break;

                    case WindowState.Minimized:
                        btnMaximize.Visibility  = Visibility.Visible;
                        btnMinimize.Visibility  = Visibility.Collapsed;
                        btnRestore.Visibility   = Visibility.Visible;
                        break;

                    case WindowState.Normal:
                        btnMaximize.Visibility  = Visibility.Visible;
                        btnMinimize.Visibility  = Visibility.Visible;
                        btnRestore.Visibility   = Visibility.Collapsed;
                        break;
                }
            }
        }


        private void CaptionBar_Loaded          ( object sender, RoutedEventArgs e ) {

            Binding                 binding;


            window                          = Window.GetWindow( this );
            if( window == null ) throw new InvalidOperationException( "Unable to reference host window." );
            window.SizeChanged              += Window_SizeChanged;

            window.CommandBindings.Add( new CommandBinding( SystemCommands.CloseWindowCommand,
                                                            CloseWindow_Executed, CloseWindow_CanExecute ) );

            window.CommandBindings.Add( new CommandBinding( SystemCommands.MaximizeWindowCommand,
                                                            MaximizeWindow_Executed, MaximizeWindow_CanExecute ) );

            window.CommandBindings.Add( new CommandBinding( SystemCommands.MinimizeWindowCommand,
                                                            MinimizeWindow_Executed, MinimizeWindow_CanExecute ) );

            window.CommandBindings.Add( new CommandBinding( SystemCommands.RestoreWindowCommand,
                                                            RestoreWindow_Executed, RestoreWindow_CanExecute ) );

            window.CommandBindings.Add( new CommandBinding( SystemCommands.ShowSystemMenuCommand,
                                                            ShowSystemMenu_Executed, ShowSystemMenu_CanExecute ) );


            if( String.IsNullOrEmpty( Title ) ) {
                binding                     = new Binding();
                binding.Source              = window;
                binding.Path                = new PropertyPath( Window.TitleProperty );
                binding.Mode                = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding( this, CaptionBar.TitleProperty, binding );
            }

            if( IconImageSource == null ) {
                binding                     = new Binding();
                binding.Source              = window;
                binding.Path                = new PropertyPath( Window.IconProperty );
                binding.Mode                = BindingMode.TwoWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding( this, CaptionBar.IconImageSourceProperty, binding );
            }

            btnClose                        = this?.Template.FindName( nameof(btnClose),    this )  as Button;
            btnIcon                         = this?.Template.FindName( nameof(btnIcon),     this )  as Button;
            btnMaximize                     = this?.Template.FindName( nameof(btnMaximize), this )  as Button;
            btnMinimize                     = this?.Template.FindName( nameof(btnMinimize), this )  as Button;
            btnRestore                      = this?.Template.FindName( nameof(btnRestore),  this )  as Button;

            btnClose!.CornerRadius          = new CornerRadius( 5d );
            btnIcon!.CornerRadius           = new CornerRadius( 5d );
            btnMaximize!.CornerRadius       = new CornerRadius( 5d );
            btnMinimize!.CornerRadius       = new CornerRadius( 5d );
            btnRestore!.CornerRadius        = new CornerRadius( 5d );

            SetVisibilityWindowButtons();
        }
        

        private void CloseWindow_CanExecute     ( object sender, CanExecuteRoutedEventArgs e ) {
            
            e.CanExecute = true;
        }
        private void CloseWindow_Executed       ( object sender, ExecutedRoutedEventArgs e ) {

            SystemCommands.CloseWindow( window );
        }
        private void MaximizeWindow_CanExecute  ( object sender, CanExecuteRoutedEventArgs e ) {
            
            e.CanExecute = true;
        }
        private void MaximizeWindow_Executed    ( object sender, ExecutedRoutedEventArgs e ) {

            SystemCommands.MaximizeWindow( window );
        }
        private void MinimizeWindow_CanExecute  ( object sender, CanExecuteRoutedEventArgs e ) {
            
            e.CanExecute = true;
        }
        private void MinimizeWindow_Executed    ( object sender, ExecutedRoutedEventArgs e ) {

            SystemCommands.MinimizeWindow( window );
        }
        private void RestoreWindow_CanExecute   ( object sender, CanExecuteRoutedEventArgs e ) {

            e.CanExecute = true;
        }
        private void RestoreWindow_Executed     ( object sender, ExecutedRoutedEventArgs e ) {

            SystemCommands.RestoreWindow( window );
        }
        private void ShowSystemMenu_CanExecute  ( object sender, CanExecuteRoutedEventArgs e ) {

            e.CanExecute = true;
        }
        private void ShowSystemMenu_Executed    ( object sender, ExecutedRoutedEventArgs e ) {

            if( btnIcon != null ) { 
                SystemCommands.ShowSystemMenu( window, btnIcon.PointToScreen( new Point() ) );
            }
        }
        private void Window_SizeChanged         ( object sender, SizeChangedEventArgs e ) {

            SetVisibilityWindowButtons();
        }
    }
}