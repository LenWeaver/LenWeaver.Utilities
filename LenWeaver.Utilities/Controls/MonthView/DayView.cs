using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace LenWeaver.Utilities {

    public class DayView : Control {

        protected internal  MonthView?      monthView   = null;
        protected internal  TextBlock?      tbText      = null;
        protected internal  ToggleButton?   btnToggle   = null;


        #region Dependency Property & Routed Event Declarations
        public static readonly      DependencyProperty      CornerRadiusProperty =
                                    DependencyProperty.Register( nameof(CornerRadius), typeof(CornerRadius), typeof(DayView),
                                                                 new PropertyMetadata( new CornerRadius( 0d ), CornerRadius_Changed ) );

        public static readonly      DependencyPropertyKey   DateKey =
                                    DependencyProperty.RegisterReadOnly( nameof(Date), typeof(DateTime), typeof(DayView),
                                                                         new PropertyMetadata( DateTime.Today ) );

        public static readonly      DependencyPropertyKey   IsMondayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsMonday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsTuesdayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsTuesday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsWednesdayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsWednesday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsThursdayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsThursday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsFridayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsFriday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsSaturdayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsSaturday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsSundayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsSunday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsWeekdayKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsWeekday), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyPropertyKey   IsWeekendKey =
                                    DependencyProperty.RegisterReadOnly( nameof(IsWeekend), typeof(bool), typeof(DayView),
                                                                         new PropertyMetadata( false ) );

        public static readonly      DependencyProperty      ContentProperty =
                                    DependencyProperty.Register( nameof(Content), typeof(object), typeof(DayView),
                                                                 new PropertyMetadata( null ) );

        public static readonly      DependencyProperty      IsCheckedProperty =
                                    DependencyProperty.Register( nameof(IsChecked), typeof(bool), typeof(DayView),
                                                                 new PropertyMetadata( null ) );


        public static readonly      RoutedEvent ClickEvent      = EventManager.RegisterRoutedEvent( nameof(Click), RoutingStrategy.Bubble,
                                                                                                    typeof(RoutedEventHandler), typeof(MonthView) );

        public static readonly      RoutedEvent CheckedEvent    = EventManager.RegisterRoutedEvent( nameof(Checked), RoutingStrategy.Bubble,
                                                                                                    typeof(RoutedEventHandler), typeof(MonthView) );
        
        public static readonly      RoutedEvent UncheckedEvent  = EventManager.RegisterRoutedEvent( nameof(Unchecked), RoutingStrategy.Bubble,
                                                                                                    typeof(RoutedEventHandler), typeof(MonthView) );

        private static void CornerRadius_Changed    ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            if( d is DayView dv && dv.btnToggle != null && e.NewValue != null && e.NewValue is CornerRadius cr ) {
                WPFHelpers.SetCornerRadius( dv.btnToggle, cr );
            }
        }
        #endregion


        static DayView() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(DayView), new FrameworkPropertyMetadata( typeof(DayView) ) );
        }

        public DayView() : base() {
            
            
            this.IsMonday       = Date.DayOfWeek        == DayOfWeek.Monday;
            this.IsTuesday      = Date.DayOfWeek        == DayOfWeek.Tuesday;
            this.IsWednesday    = Date.DayOfWeek        == DayOfWeek.Wednesday;
            this.IsThursday     = Date.DayOfWeek        == DayOfWeek.Thursday;
            this.IsFriday       = Date.DayOfWeek        == DayOfWeek.Friday;

            this.IsSaturday     = Date.DayOfWeek        == DayOfWeek.Saturday;
            this.IsSunday       = Date.DayOfWeek        == DayOfWeek.Sunday;

            this.IsWeekend      = IsSaturday || IsSunday;
            this.IsWeekday      = !IsWeekend;
            
            Loaded              += DayView_Loaded;
        }


        public bool             IsMonday {
            get { return (bool)GetValue( IsMondayKey.DependencyProperty ); }
            protected set { SetValue( IsMondayKey, value ); }
        }
        public bool             IsTuesday {
            get { return (bool)GetValue( IsTuesdayKey.DependencyProperty ); }
            protected set { SetValue( IsTuesdayKey, value ); }
        }
        public bool             IsWednesday {
            get { return (bool)GetValue( IsWednesdayKey.DependencyProperty ); }
            protected set { SetValue( IsWednesdayKey, value ); }
        }
        public bool             IsThursday {
            get { return (bool)GetValue( IsThursdayKey.DependencyProperty ); }
            protected set { SetValue( IsThursdayKey, value ); }
        }
        public bool             IsFriday {
            get { return (bool)GetValue( IsFridayKey.DependencyProperty ); }
            protected set { SetValue( IsFridayKey, value ); }
        }
        public bool             IsSaturday {
            get { return (bool)GetValue( IsSaturdayKey.DependencyProperty ); }
            protected set { SetValue( IsSaturdayKey, value ); }
        }
        public bool             IsSunday {
            get { return (bool)GetValue( IsSundayKey.DependencyProperty ); }
            protected set { SetValue( IsSundayKey, value ); }
        }

        public bool             IsWeekday {
            get { return (bool)GetValue( IsWeekdayKey.DependencyProperty ); }
            protected set { SetValue( IsWeekdayKey, value ); }
        }
        public bool             IsWeekend {
            get { return (bool)GetValue( IsWeekendKey.DependencyProperty ); }
            set { SetValue( IsWeekendKey, value ); }
        }

        public bool             IsChecked {
            get { return (bool)GetValue( IsCheckedProperty ); }
            set { SetValue( IsCheckedProperty, value ); }
        }

        public DateTime         Date {
            get { return (DateTime)GetValue( DateKey.DependencyProperty ); }
            protected internal set { SetValue( DateKey, value ); }
        }

        public CornerRadius     CornerRadius {
            get { return (CornerRadius)GetValue( CornerRadiusProperty ); }
            set { SetValue( CornerRadiusProperty, value ); }
        }

        public object           Content {
            get { return (object)GetValue( ContentProperty ); }
            set { SetValue( ContentProperty, value ); }
        }


        public event RoutedEventHandler Checked {
            add     =>  AddHandler( CheckedEvent, value );
            remove  =>  RemoveHandler( CheckedEvent, value );
        }
        public event RoutedEventHandler Click {
            add     =>  AddHandler( ClickEvent, value );
            remove  =>  RemoveHandler( ClickEvent, value );
        }
        public event RoutedEventHandler Unchecked {
            add     => AddHandler( UncheckedEvent, value );
            remove  => RemoveHandler( UncheckedEvent, value );
        }

        protected internal void SetParentAndDate( MonthView mv, DateTime dt ) {

            monthView           = mv;
            Date                = dt;

            if( tbText != null ) {
                tbText.Text     = dt.Day.ToString();
            }
        }

        private void DayView_Loaded         ( object sender, RoutedEventArgs e ) {

            if( tbText == null ) {
                tbText = (TextBlock?)this.Template?.FindName( nameof(tbText), this );

                if( tbText != null && Date.Date.CompareTo( DateTime.MinValue.Date ) != 0 ) {
                    tbText.Text = Date.Day.ToString();
                }
            }

            if( btnToggle == null ) {
                btnToggle = (ToggleButton?)this.Template?.FindName( "button", this );

                if( btnToggle != null ) {
                    btnToggle.Click     += btnToggle_Click;
                    btnToggle.Checked   += btnToggle_Checked;
                    btnToggle.Unchecked += btnToggle_Unchecked;

                    WPFHelpers.SetCornerRadius( btnToggle, CornerRadius );
                }
            }
        }


        private void btnToggle_Checked      ( object sender, RoutedEventArgs e ) {

            RaiseEvent( new RoutedEventArgs( CheckedEvent, this ) );
        }
        private void btnToggle_Click        ( object sender, RoutedEventArgs e ) {

            RaiseEvent( new RoutedEventArgs( ClickEvent, this ) );
        }
        private void btnToggle_Unchecked    ( object sender, RoutedEventArgs e ) {

            RaiseEvent( new RoutedEventArgs( UncheckedEvent, this ) );
        }
    }
}