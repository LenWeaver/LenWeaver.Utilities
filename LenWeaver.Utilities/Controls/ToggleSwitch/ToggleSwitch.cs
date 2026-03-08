using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace LenWeaver.Utilities {

    public class ToggleSwitch : ToggleButton {

        internal    AnimationValues         animate;

        protected   DoubleAnimation?        animateLeft             = null;
        protected   DoubleAnimation?        animateTop              = null;

        protected   Canvas?                 canvas                  = null;
        protected   Border?                 button                  = null;


        #region Static Dependency Members
        public static readonly      DependencyProperty      AnimateSwitchProperty =
                                    DependencyProperty.Register( nameof(AnimateSwitch), typeof(bool), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( true ) );

        public static readonly      DependencyProperty      AnimationDurationProperty =
                                    DependencyProperty.Register( nameof(AnimationDuration), typeof(double), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( 300d ) );

        public static readonly      DependencyProperty      CheckedBackgroundProperty =
                                    DependencyProperty.Register( nameof(CheckedBackground), typeof(Brush), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( Brushes.LimeGreen ) );

        public static readonly      DependencyProperty      CheckedSideProperty =
                                    DependencyProperty.Register( nameof(CheckedSide), typeof(CheckedSide), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( CheckedSide.RightOrTop ) );

        public static readonly      DependencyProperty      CornerRadiusProperty =
                                    DependencyProperty.Register( nameof(CornerRadius), typeof(CornerRadius), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( new CornerRadius( 0d ) ) );
        
        public static readonly      DependencyProperty      OrientationProperty =
                                    DependencyProperty.Register( nameof(Orientation), typeof(Orientation), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( Orientation.Horizontal ) );

        public static readonly      DependencyProperty      SwitchBackgroundProperty =
                                    DependencyProperty.Register( nameof(SwitchBackground), typeof(Brush), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( Brushes.SkyBlue ) );

        public static readonly      DependencyProperty      SwitchBorderBrushProperty =
                                    DependencyProperty.Register( nameof(SwitchBorderBrush), typeof(Brush), typeof(ToggleButton),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      SwitchBorderThicknessProperty =
                                    DependencyProperty.Register( nameof(SwitchBorderThickness), typeof(Thickness), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( new Thickness( 1d ) ) );

        public static readonly      DependencyProperty      SwitchCornerRadiusProperty =
                                    DependencyProperty.Register( nameof(SwitchCornerRadius), typeof(CornerRadius), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( new CornerRadius( 5d ) ) );

        public static readonly      DependencyProperty      UncheckedBackgroundProperty =
                                    DependencyProperty.Register( nameof(UncheckedBackground), typeof(Brush), typeof(ToggleSwitch),
                                                                 new PropertyMetadata( Brushes.OrangeRed ) );

        #endregion


        static ToggleSwitch() {
            
            DefaultStyleKeyProperty.OverrideMetadata( typeof(ToggleSwitch), new FrameworkPropertyMetadata( typeof(ToggleSwitch) ) );
        }

        public ToggleSwitch() {

            animate                 =  new AnimationValues();

            this.Checked            += ToggleSwitch_Checked;
            this.Loaded             += ToggleSwitch_Loaded;
            this.Unchecked          += ToggleSwitch_Unchecked;
        }


        public bool                 AnimateSwitch {
            get { return (bool)GetValue( AnimateSwitchProperty ); }
            set { SetValue( AnimateSwitchProperty, value ); }
        }
        public double               AnimationDuration {
            get { return (double)GetValue( AnimationDurationProperty ); }
            set { SetValue( AnimationDurationProperty, value ); }
        }
        public CheckedSide          CheckedSide {
            get { return (CheckedSide)GetValue( CheckedSideProperty ); }
            set { SetValue( CheckedSideProperty, value ); }
        }
        public Orientation          Orientation {
            get { return (Orientation)GetValue( OrientationProperty ); }
            set { SetValue( OrientationProperty, value ); }
        }
        public CornerRadius         CornerRadius {
            get { return (CornerRadius)GetValue( CornerRadiusProperty ); }
            set { SetValue( CornerRadiusProperty, value ); }
        }
        public Brush                CheckedBackground {
            get { return (Brush)GetValue( CheckedBackgroundProperty ); }
            set { SetValue( CheckedBackgroundProperty, value ); }
        }
        public Brush                SwitchBackground {
            get { return (Brush)GetValue( SwitchBackgroundProperty ); }
            set { SetValue( SwitchBackgroundProperty, value ); }
        }
        public Brush                SwitchBorderBrush {
            get { return (Brush)GetValue( SwitchBorderBrushProperty ); }
            set { SetValue( SwitchBorderBrushProperty, value ); }
        }
        public Thickness            SwitchBorderThickness {
            get { return (Thickness)GetValue( SwitchBorderThicknessProperty ); }
            set { SetValue( SwitchBorderThicknessProperty, value ); }
        }
        public CornerRadius         SwitchCornerRadius {
            get { return (CornerRadius)GetValue( SwitchCornerRadiusProperty ); }
            set { SetValue( SwitchCornerRadiusProperty, value ); }
        }
        public Brush                UncheckedBackground {
            get { return (Brush)GetValue( UncheckedBackgroundProperty ); }
            set { SetValue( UncheckedBackgroundProperty, value ); }
        }


        protected void InitializeAnimationValues() {

            double  buttonHeight;
            double  buttonWidth;
            double  height                      = this.Height;
            double  width                       = this.Width;


            if( canvas != null && button != null ) {
                animate.CheckedBackground       = CheckedBackground;
                animate.UncheckedBackground     = UncheckedBackground;

                buttonHeight                    = Double.IsNaN( button.Height ) ? 0d : button.Height;
                buttonWidth                     = Double.IsNaN( button.Width )  ? 0d : button.Width;

                if( Orientation == Orientation.Horizontal && CheckedSide == CheckedSide.LeftOrBottom ) {
                    animate.CheckedLeft         =  -1d;
                    animate.CheckedTop          =   0d;
                    animate.UncheckedLeft       =  width - buttonWidth - 1;
                    animate.UncheckedTop        =   0d;
                }
                else if( Orientation == Orientation.Horizontal && CheckedSide == CheckedSide.RightOrTop ) {
                    animate.CheckedLeft         =  width - buttonWidth - 1;
                    animate.CheckedTop          =   0d;
                    animate.UncheckedLeft       =  -1d;
                    animate.UncheckedTop        =   0d;
                }
                else if( Orientation == Orientation.Vertical && CheckedSide == CheckedSide.LeftOrBottom ) {
                    animate.CheckedLeft         = -1d;
                    animate.CheckedTop          =  height - buttonHeight;
                    animate.UncheckedLeft       = -1d;
                    animate.UncheckedTop        = -1d;
                }
                else if( Orientation == Orientation.Vertical && CheckedSide == CheckedSide.RightOrTop ) {
                    animate.CheckedLeft         = -1d;
                    animate.CheckedTop          = -1d;
                    animate.UncheckedLeft       = -1d;
                    animate.UncheckedTop        =  height - buttonHeight;
                }
                else {
                    throw new InvalidOperationException( "Unknown combination of Orientation and CheckedSide." );
                }
            }
        }
        protected void StartSwitchAnimations( bool newCheckedState ) {

            Duration            d           = new Duration( TimeSpan.FromMilliseconds( AnimationDuration ) );
            

            if( animateLeft == null && animateTop == null ) {
                animateLeft                 = new DoubleAnimation();
                animateLeft.Completed       += AnimateLeft_Completed;

                animateTop                  = new DoubleAnimation();
                animateTop.Completed        += AnimateTop_Completed;
            }

            if( !animate.IsValid ) InitializeAnimationValues();

            if( animate.IsValid ) {
                animateLeft!.From           = newCheckedState ? animate.UncheckedLeft : animate.CheckedLeft;
                animateLeft!.To             = newCheckedState ? animate.CheckedLeft : animate.UncheckedLeft;
                animateLeft!.Duration       = d;
                animateLeft!.FillBehavior   = FillBehavior.HoldEnd;

                animateTop!.From            = newCheckedState ? animate.UncheckedTop : animate.CheckedTop;
                animateTop!.To              = newCheckedState ? animate.CheckedTop : animate.UncheckedTop;
                animateTop!.Duration        = d;
                animateTop!.FillBehavior    = FillBehavior.HoldEnd;

                button!.BeginAnimation( Canvas.LeftProperty, animateLeft );
                button!.BeginAnimation( Canvas.TopProperty,  animateTop );
            }
        }


        private void AnimateColor_Completed         ( object? sender, EventArgs e ) {

            Background.BeginAnimation( SolidColorBrush.ColorProperty, null );
        }
        private void AnimateLeft_Completed          ( object? sender, EventArgs e ) {

            button!.BeginAnimation( Canvas.LeftProperty, null );

            Canvas.SetLeft( button, (IsChecked ?? true) ? animate.CheckedLeft : animate.UncheckedLeft );
        }
        private void AnimateTop_Completed           ( object? sender, EventArgs e ) {

            button!.BeginAnimation( Canvas.TopProperty, null );

            Canvas.SetTop( button, (IsChecked ?? true) ? animate.CheckedTop : animate.UncheckedTop );
        }

        private void ToggleSwitch_Checked           ( object sender, RoutedEventArgs e ) {
            
            if( button != null ) {
                if( !animate.IsValid ) {
                    InitializeAnimationValues();
                }

                if( AnimateSwitch ) {
                    StartSwitchAnimations( newCheckedState: true );
                }
                else {
                    Canvas.SetLeft( button, animate.CheckedLeft );
                    Canvas.SetTop(  button, animate.CheckedTop );
                }

                Background = animate.CheckedBackground;
            }
        }
        private void ToggleSwitch_Loaded            ( object sender, RoutedEventArgs e ) {

            double  height                      = this.Height;
            double  width                       = this.Width;


            canvas                              = this?.Template.FindName( nameof(canvas), this ) as Canvas;
            button                              = this?.Template.FindName( nameof(button), this ) as Border;

            if( button != null ) {
                button.Height                   = Orientation == Orientation.Horizontal ? Height - 1 : Width;
                button.Width                    = Orientation == Orientation.Horizontal ? Height : Width;

                InitializeAnimationValues();

                Canvas.SetLeft( button, (IsChecked ?? true) ? animate.CheckedLeft : animate.UncheckedLeft );
                Canvas.SetTop(  button, (IsChecked ?? true) ? animate.CheckedTop : animate.UncheckedTop );
            }
        }
        private void ToggleSwitch_Unchecked         ( object sender, RoutedEventArgs e ) {

            if( button != null ) {
                if( !animate.IsValid ) {
                    InitializeAnimationValues();
                }

                if( AnimateSwitch ) {
                    StartSwitchAnimations( newCheckedState: false );
                }
                else {
                    Canvas.SetLeft( button, animate.UncheckedLeft );
                    Canvas.SetTop(  button, animate.UncheckedTop );
                }
            }

            Background = animate.UncheckedBackground;
        }
    }
}