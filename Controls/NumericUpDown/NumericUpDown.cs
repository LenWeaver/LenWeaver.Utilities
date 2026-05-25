using System;
using System.Collections.Generic;
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
    
    [TemplatePart( Name = "PART_TextBox",               Type = typeof(TextBox) )]
    [TemplatePart( Name = "PART_DecreaseButton",        Type = typeof(RepeatButton) )]
    [TemplatePart( Name = "PART_IncreaseButton",        Type = typeof(RepeatButton) )]
    public class NumericUpDown : Control {

        private RepeatButton        PART_DecreaseButton     = null!;
        private RepeatButton        PART_IncreaseButton     = null!;

        private TextBox             PART_TextBox            = null!;


        static NumericUpDown() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(NumericUpDown), new FrameworkPropertyMetadata( typeof(NumericUpDown) ) );
        }


        #region Dependency Property Declarations
        public static readonly  DependencyProperty  InitialRepeatDelayProperty =
                                DependencyProperty.Register( nameof(InitialRepeatDelay), typeof(int),
                                                             typeof(NumericUpDown), new PropertyMetadata( 500 ) );

        public static readonly  DependencyProperty  LargeStepProperty =
                                DependencyProperty.Register( nameof(LargeStep), typeof(decimal),
                                                             typeof(NumericUpDown), new PropertyMetadata( 10.0m ) );

        public static readonly  DependencyProperty  MaximumValueProperty =
                                DependencyProperty.Register( nameof(MaximumValue), typeof(decimal),
                                                             typeof(NumericUpDown), new PropertyMetadata( 1_000_000m,
                                                                                   propertyChangedCallback: MinimumMaximumValue_Changed,
                                                                                       coerceValueCallback: MaximumValue_CoerceValue ) );

        public static readonly  DependencyProperty  MinimumButtonWidthProperty =
                                DependencyProperty.Register( nameof(MinimumButtonWidth), typeof(double),
                                                             typeof(NumericUpDown), new PropertyMetadata( 24d ) );

        public static readonly  DependencyProperty  MinimumValueProperty =
                                DependencyProperty.Register( nameof(MinimumValue), typeof(decimal),
                                                             typeof(NumericUpDown), new PropertyMetadata( -100.0m,
                                                                                 propertyChangedCallback: MinimumMaximumValue_Changed,
                                                                                     coerceValueCallback: MinimumValue_CoerceValue ) );

        public static readonly  DependencyProperty  NegativeForegroundProperty =
                                DependencyProperty.Register( nameof(NegativeForeground), typeof(Brush),
                                                             typeof(NumericUpDown), new PropertyMetadata( Brushes.Red ) );

        public static readonly  DependencyProperty  PositiveForegroundProperty =
                                DependencyProperty.Register( nameof(PositiveForeground), typeof(Brush),
                                                             typeof(NumericUpDown), new PropertyMetadata( Brushes.Black ) );

        public static readonly  DependencyProperty  RepeatIntervalDelayProperty =
                                DependencyProperty.Register( nameof(RepeatIntervalDelay), typeof(int),
                                                             typeof(NumericUpDown), new PropertyMetadata( 200 ) );

        public static readonly  DependencyProperty  SmallStepProperty =
                                DependencyProperty.Register( nameof(SmallStep), typeof(decimal),
                                                             typeof(NumericUpDown), new PropertyMetadata( 1.0m ) );

        public static readonly  DependencyProperty  ValueProperty =
                                DependencyProperty.Register( nameof(Value), typeof(decimal),
                                                             typeof(NumericUpDown), new PropertyMetadata( 0.0m,
                                                                                 propertyChangedCallback: Value_Changed,
                                                                                     coerceValueCallback: Value_CoerceValue ) );

        public static readonly  DependencyProperty  StringFormatProperty =
                                DependencyProperty.Register( nameof(StringFormat), typeof(string),
                                                             typeof(NumericUpDown), new PropertyMetadata( String.Empty ) );

        public static readonly  DependencyPropertyKey IsNegativePropertyKey =
                                DependencyProperty.RegisterReadOnly( nameof(IsNegative), typeof(bool),
                                                                     typeof(NumericUpDown), new PropertyMetadata( false ) );

        public static readonly  DependencyPropertyKey IsZeroPropertyKey =
                                DependencyProperty.RegisterReadOnly( nameof(IsZero), typeof(bool),
                                                                     typeof(NumericUpDown), new PropertyMetadata( true ) );

        public static readonly  DependencyProperty  IsNegativeProperty  = IsNegativePropertyKey.DependencyProperty;
        public static readonly  DependencyProperty  IsZeroProperty      = IsZeroPropertyKey.DependencyProperty;

        #endregion
        #region Event and Command declarations
        public static readonly  DependencyProperty  ValueChangedCommandProperty =
                                DependencyProperty.Register( nameof(ValueChangedCommand), typeof(ICommand), typeof(NumericUpDown), null );

        private event RoutedPropertyChangedEventHandler<decimal>?       valueChanged    = null;
        #endregion


        public NumericUpDown() : base() {}


        public bool             IsNegative {
            get => (bool)GetValue( IsNegativeProperty );
            private set => SetValue( IsNegativePropertyKey, value );
        }
        public bool             IsZero {
            get => (bool)GetValue( IsZeroProperty );
            private set => SetValue( IsZeroPropertyKey, value );
        }

        public int              InitialRepeatDelay {
            get => (int)GetValue( InitialRepeatDelayProperty );
            set => SetValue( InitialRepeatDelayProperty, value );
        }
        public int              RepeatIntervalDelay {
            get => (int)GetValue( RepeatIntervalDelayProperty );
            set => SetValue( RepeatIntervalDelayProperty, value );
        }

        public double           MinimumButtonWidth {
            get => (double)GetValue( MinimumButtonWidthProperty );
            set => SetValue( MinimumButtonWidthProperty, value );
        }

        public decimal          LargeStep {
            get => (decimal)GetValue( LargeStepProperty );
            set => SetValue( LargeStepProperty, value );
        }
        public decimal          MaximumValue {
            get => (decimal)GetValue( MaximumValueProperty );
            set => SetValue( MaximumValueProperty, value );
        }
        public decimal          MinimumValue {
            get => (decimal)GetValue( MinimumValueProperty );
            set => SetValue( MinimumValueProperty, value );
        }
        public decimal          SmallStep {
            get => (decimal)GetValue( SmallStepProperty );
            set => SetValue( SmallStepProperty, value );
        }
        public decimal          Value {
            get => (decimal)GetValue( ValueProperty );
            set => SetValue( ValueProperty, value );
        }

        public string           StringFormat {
            get => (string)GetValue( StringFormatProperty );
            set => SetValue( StringFormatProperty, value );
        }

        public Brush            NegativeForeground {
            get => (Brush)GetValue( NegativeForegroundProperty );
            set => SetValue( NegativeForegroundProperty, value );
        }
        public Brush            PositiveForeground {
            get => (Brush)GetValue( PositiveForegroundProperty );
            set => SetValue( PositiveForegroundProperty, value );
        }


        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged {
            add     => valueChanged += value;
            remove  => valueChanged -= value;
        }

        public ICommand         ValueChangedCommand {
            get => (ICommand)GetValue( ValueChangedCommandProperty );
            set => SetValue( ValueChangedCommandProperty, value );
        }


        public override void    OnApplyTemplate                     () {

            base.OnApplyTemplate();


            PART_TextBox                    = GetTemplateChild( nameof(PART_TextBox) )          as TextBox ??
                                                                throw new InvalidOperationException( $"Unable to reference {nameof(PART_TextBox)}." );

            PART_DecreaseButton             = GetTemplateChild( nameof(PART_DecreaseButton) )   as RepeatButton ??
                                                                throw new InvalidOperationException( $"Unable to reference {nameof(PART_DecreaseButton)}." );

            PART_IncreaseButton             = GetTemplateChild( nameof(PART_IncreaseButton) )   as RepeatButton ??
                                                                throw new InvalidOperationException( $"Unable to reference {nameof(PART_IncreaseButton)}." );

            PART_DecreaseButton.Click       += PART_DecreaseButton_Click;
            PART_IncreaseButton.Click       += PART_IncreaseButton_Click;

            this.PreviewMouseWheel          += NumericUpDown_PreviewMouseWheel;
            this.PreviewKeyDown             += NumericUpDown_PreviewKeyDown;


            UpdateDisplay();
        }


        public void             UpdateDisplay                       () {

            string      mask    = StringFormat;


            if( mask.StartsWith( "{}" ) && mask.Length > 2 ) mask = mask.Substring( 2 );
            mask                = String.IsNullOrWhiteSpace( mask ) ? "{0}" : mask;

            PART_TextBox?.Text  = String.Format( mask, Value );
        }


        internal protected void OnValueChanged( decimal oldValue, decimal newValue ) {

            NumericUpDownValueChangedCommandParameters  cmdArgs     = new( oldValue, newValue, this );
            RoutedPropertyChangedEventArgs<decimal>     args        = new( oldValue, newValue );


            valueChanged?.Invoke( this, args );
            if( ValueChangedCommand?.CanExecute( cmdArgs ) ?? false) {
                ValueChangedCommand.Execute( cmdArgs );
            }
        }

        private void            NumericUpDown_PreviewKeyDown        ( object sender, KeyEventArgs e ) {
            
            if( e.Key == Key.Up ) {
                Value += (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;

                e.Handled = true;
            }
            else if( e.Key == Key.Down ) {
                Value -= (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;

                e.Handled = true;
            }
        }
        private void            NumericUpDown_PreviewMouseWheel     ( object sender, MouseWheelEventArgs e ) {
            
            NumericUpDown       n   = (NumericUpDown)sender;

            if( n.IsKeyboardFocusWithin ) {
                if( e.Delta > 0 ) {
                    Value += (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;
                }
                else {
                    Value -= (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;
                }

                e.Handled = true;
            }
        }
        private void            PART_DecreaseButton_Click           ( object sender, RoutedEventArgs e ) {

            Value -= (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;
        }
        private void            PART_IncreaseButton_Click           ( object sender, RoutedEventArgs e ) {

            Value += (Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl )) ? LargeStep : SmallStep;
        }

        private static void     MinimumMaximumValue_Changed         ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            NumericUpDown       nud     = (NumericUpDown)d;


            nud.CoerceValue( ValueProperty );
        }
        private static void     Value_Changed                       ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            NumericUpDown       nud     = (NumericUpDown)d;
            


            nud.IsNegative              = nud.Value < 0;
            nud.IsZero                  = nud.Value == 0;
            
            nud.OnValueChanged( (decimal)e.OldValue, (decimal)e.NewValue );
            nud.UpdateDisplay();
        }

        private static object   MaximumValue_CoerceValue            ( DependencyObject d, object value ) {

            decimal         max     = (decimal)value;
            NumericUpDown   nud     = (NumericUpDown)d;


            return max < nud.MinimumValue ? nud.MinimumValue : max;
        }
        private static object   MinimumValue_CoerceValue            ( DependencyObject d, object value ) {

            decimal         min     = (decimal)value;
            NumericUpDown   nud     = (NumericUpDown)d;


            return min > nud.MaximumValue ? nud.MaximumValue : min;
        }
        private static object   Value_CoerceValue                   ( DependencyObject d, object value ) {

            decimal         val     = (decimal)value;
            NumericUpDown   nud     = (NumericUpDown)d;


            return Math.Clamp( val, nud.MinimumValue, nud.MaximumValue );
        }
    }
}