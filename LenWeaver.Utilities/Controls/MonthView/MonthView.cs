using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

    public delegate void PropertyChangedValEventHandler<T>  ( object sender, PropertyChangedValEventArgs<T>  e ) where T : struct;
    public delegate void PropertyChangingValEventHandler<T> ( object sender, PropertyChangingValEventArgs<T> e ) where T : struct;

    public delegate void PropertyChangedRefEventHandler<T>  ( object sender, PropertyChangedRefEventArgs<T>  e ) where T : class;
    public delegate void PropertyChangingRefEventHandler<T> ( object sender, PropertyChangingRefEventArgs<T> e ) where T : class;


    [DefaultEvent( nameof(DateChanged) )]
    [DefaultProperty( nameof(Days) )]
    public class MonthView : Control {

        protected   const           int                     DaysInWeek                      =  7;
        protected   const           int                     MonthsInYear                    = 12;
        protected   const           int                     MaxDaysInCalendar               = 42;   //Life, the universe and everything.
        protected   const           int                     YearsInDecade                   = 10;


        public static readonly      double                  DefaultLabelFontSize            = 12d;
        public static readonly      FontStyle               DefaultLabelFontStyle           = FontStyles.Normal;
        public static readonly      FontWeight              DefaultLabelFontWeight          = FontWeights.Normal;
        public static readonly      FontFamily              DefaultLabelFontFamily          = FontExtensions.SegoeUI;

        protected                   DateTime                displayDate                     = DateTime.Today;
        protected                   DateTime?               selectedDate                    = DateTime.Today;

        protected                   Button?                 btnDecadeNext                   = null;
        protected                   Button?                 btnDecadePrev                   = null;
        protected                   Button?                 btnMonthNext                    = null;
        protected                   Button?                 btnMonthPrev                    = null;
        protected                   Button?                 btnNext                         = null;
        protected                   Button?                 btnNextYear                     = null;
        protected                   Button?                 btnPrevious                     = null;
        protected                   Button?                 btnPrevYear                     = null;
        protected                   Button?                 btnEnteredYear                  = null;
        protected                   Grid?                   grdMonthList                    = null;
        protected                   Grid?                   grdMonthView                    = null;
        protected                   Grid?                   grdYearList                     = null;
        protected                   Hyperlink?              lnkMonth                        = null;
        protected                   Hyperlink?              lnkYear                         = null;
        protected                   Span?                   spnDays                         = null;
        protected                   TextBlock?              tbDecadeYear                    = null;
        protected                   TextBlock?              tbMonthListYear                 = null;
        protected                   TextBlock?              tbMonthYear                     = null;
        protected                   TextBox?                txtEnteredYear                  = null;
        protected                   UniformGrid?            grdDays                         = null;

        protected                   string[]                dayOfWeekNames;
        protected                   string[]                monthNames;
        protected                   Button?[]               monthList                       = new Button?[MonthsInYear];
        protected                   Button?[]               yearList                        = new Button?[YearsInDecade];
        protected                   DayView?[]              dayViewItems                    = new DayView[MaxDaysInCalendar];
        protected                   TextBlock?[]            daysOfWeek                      = new TextBlock[DaysInWeek];
        protected                   TextBlock?[]            monthsOfYear                    = new TextBlock[MonthsInYear];
        
        public                      DayViewCollection       Days                            = new DayViewCollection();


        //public delegate void PropertyChangedValEventHandler<T>  ( object sender, PropertyChangedValEventArgs<T>  e ) where T : struct;
        //public delegate void PropertyChangingValEventHandler<T> ( object sender, PropertyChangingValEventArgs<T> e ) where T : struct;

        //public delegate void PropertyChangedRefEventHandler<T>  ( object sender, PropertyChangedRefEventArgs<T>  e ) where T : class;
        //public delegate void PropertyChangingRefEventHandler<T> ( object sender, PropertyChangingRefEventArgs<T> e ) where T : class;


        #region Static Members
        public static readonly      DependencyProperty      CornerRadiusProperty =
                                    DependencyProperty.Register( nameof(CornerRadius), typeof(CornerRadius), typeof(MonthView),
                                                                 new PropertyMetadata( new CornerRadius( 0d ) ) );

        public static readonly      DependencyProperty      SelectionModeProperty =
                                    DependencyProperty.Register( nameof(SelectionMode), typeof(MonthViewSelectionMode), typeof(MonthView),
                                                                 new PropertyMetadata( MonthViewSelectionMode.Single ) );

        public static readonly      DependencyProperty      FirstDayOfWeekProperty =
                                    DependencyProperty.Register( nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(MonthView),
                                                                 new PropertyMetadata( DayOfWeek.Sunday,
                                                                                       FirstDayOfWeek_Changed ) );

        public static readonly      DependencyProperty      DaysBackgroundProperty =
                                    DependencyProperty.Register( nameof(DaysBackground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.White ) );

        public static readonly      DependencyProperty      DaysBorderBrushProperty =
                                    DependencyProperty.Register( nameof(DaysBorderBrush), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      DaysBorderThicknessProperty =
                                    DependencyProperty.Register( nameof(DaysBorderThickness), typeof(Thickness), typeof(MonthView),
                                                                 new PropertyMetadata( new Thickness( 0d ) ) );

        public static readonly      DependencyProperty      DaysCornerRadiusProperty =
                                    DependencyProperty.Register( nameof(DaysCornerRadius),
                                                                 typeof(CornerRadius?), typeof(MonthView),
                                                                 new PropertyMetadata( null ) );

        public static readonly      DependencyProperty      DaysForegroundProperty =
                                    DependencyProperty.Register( nameof(DaysForeground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      DaysMarginProperty =
                                    DependencyProperty.Register( nameof(DaysMargin), typeof(Thickness), typeof(MonthView),
                                                                 new PropertyMetadata( new Thickness( 0d ) ) );

        public static readonly      DependencyProperty      DaysPaddingProperty =
                                    DependencyProperty.Register( nameof(DaysPadding), typeof(Thickness), typeof(MonthView),
                                                                 new PropertyMetadata( new Thickness( 0d ) ) );

        public static readonly      DependencyProperty      ShowPreviousNextButtonsProperty =
                                    DependencyProperty.Register( nameof(ShowPreviousNextButtons), typeof(bool), typeof(MonthView),
                                                                 new PropertyMetadata( true ) );

        public static readonly      DependencyProperty      ShowTargetMonthOnlyProperty =
                                    DependencyProperty.Register( nameof(ShowTargetMonthOnly), typeof(bool), typeof(MonthView),
                                                                 new PropertyMetadata( false ) );

        public static readonly      DependencyProperty      MonthListForegroundProperty =
                                    DependencyProperty.Register( nameof(MonthListForeground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      DependencyProperty      MonthListBackgroundProperty =
                                    DependencyProperty.Register( nameof(MonthListBackground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.White ) );

        public static readonly      DependencyProperty      YearListBackgroundProperty =
                                    DependencyProperty.Register( nameof(YearListBackground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.White ) );

        public static readonly      DependencyProperty      YearListForegroundProperty =
                                    DependencyProperty.Register( nameof(YearListForeground), typeof(Brush), typeof(MonthView),
                                                                 new PropertyMetadata( Brushes.Black ) );

        public static readonly      RoutedEvent             DateChangedEvent = 
                                    EventManager.RegisterRoutedEvent( nameof(DateChanged), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangedValEventHandler<DateTime>), typeof(MonthView) );

        public static readonly      RoutedEvent             DateChangingEvent =
                                    EventManager.RegisterRoutedEvent( nameof(DateChanging), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangingValEventHandler<DateTime>), typeof(MonthView) );

        public static readonly      RoutedEvent             MonthChangedEvent =
                                    EventManager.RegisterRoutedEvent( nameof(MonthChanged), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangedValEventHandler<int>), typeof(MonthView) );

        public static readonly      RoutedEvent             MonthChangingEvent =
                                    EventManager.RegisterRoutedEvent( nameof(MonthChanging), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangingValEventHandler<int>), typeof(MonthView) );

        public static readonly      RoutedEvent             YearChangedEvent =
                                    EventManager.RegisterRoutedEvent( nameof(YearChanged), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangedValEventHandler<int>), typeof(MonthView) );

        public static readonly      RoutedEvent             YearChangingEvent =
                                    EventManager.RegisterRoutedEvent( nameof(YearChanging), RoutingStrategy.Bubble,
                                                                      typeof(PropertyChangingValEventHandler<int>), typeof(MonthView) );
        #endregion


        static MonthView() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(MonthView), new FrameworkPropertyMetadata( typeof(MonthView) ) );
        }

        public MonthView() : base() {

            dayOfWeekNames  = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames;
            monthNames      = DateTimeFormatInfo.CurrentInfo.MonthNames;

            Loaded          += MonthView_Loaded;    
        }


        public bool                     ShowPreviousNextButtons {
            get { return (bool)GetValue( ShowPreviousNextButtonsProperty ); }
            set { SetValue( ShowPreviousNextButtonsProperty, value ); }
        }
        public bool                     ShowTargetMonthOnly {
            get { return (bool)GetValue( ShowTargetMonthOnlyProperty ); }
            set { SetValue( ShowTargetMonthOnlyProperty, value ); }
        }
        public DayOfWeek                FirstDayOfWeek {
            get { return (DayOfWeek)GetValue( FirstDayOfWeekProperty ); }
            set { SetValue( FirstDayOfWeekProperty, value ); }
        }
        public MonthViewSelectionMode   SelectionMode {
            get { return (MonthViewSelectionMode)GetValue( SelectionModeProperty ); }
            set { SetValue( SelectionModeProperty, value ); }
        }
        public CornerRadius             CornerRadius {
            get { return (CornerRadius)GetValue( CornerRadiusProperty ); }
            set { SetValue( CornerRadiusProperty, value ); }
        }
        public CornerRadius?            DaysCornerRadius {
            get { return (CornerRadius?)GetValue( DaysCornerRadiusProperty ); }
            set { SetValue( DaysCornerRadiusProperty, value ); }
        }
        public DateTime                 DisplayDate {
            get => displayDate;
            set {
                if( value > DateTime.MinValue ) {
                    displayDate    = value;

                    RecalculateCalendar( displayDate );
                }
            }
        }
        public DateTime?                SelectedDate {
            get => selectedDate;
            set {
                bool            cancel;

                DateTime?       oldDate;


                cancel              = RaiseChangingEvents( value, selectedDate );
                if( !cancel ) {
                    oldDate         = selectedDate;
                    selectedDate    = value;

                    RaiseChangedEvents( selectedDate, oldDate );
                }
            }
        }
        public Thickness                DaysBorderThickness {
            get { return (Thickness)GetValue( DaysBorderThicknessProperty ); }
            set { SetValue( DaysBorderThicknessProperty, value ); }
        }
        public Thickness                DaysMargin {
            get { return (Thickness)GetValue( DaysMarginProperty ); }
            set { SetValue( DaysMarginProperty, value ); }
        }
        public Thickness                DaysPadding {
            get { return (Thickness)GetValue( DaysPaddingProperty ); }
            set { SetValue( DaysPaddingProperty, value ); }
        }
        public Brush                    DaysBackground {
            get { return (Brush)GetValue( DaysBackgroundProperty ); }
            set { SetValue( DaysBackgroundProperty, value ); }
        }
        public Brush                    DaysBorderBrush {
            get { return (Brush)GetValue( DaysBorderBrushProperty ); }
            set { SetValue( DaysBorderBrushProperty, value ); }
        }
        public Brush                    DaysForeground {
            get { return (Brush)GetValue( DaysForegroundProperty ); }
            set { SetValue( DaysForegroundProperty, value ); }
        }
        public Brush                    MonthListBackground {
            get { return (Brush)GetValue( MonthListBackgroundProperty ); }
            set { SetValue( MonthListBackgroundProperty, value ); }
        }
        public Brush                    MonthListForeground {
            get { return (Brush)GetValue( MonthListForegroundProperty ); }
            set { SetValue( MonthListForegroundProperty, value ); }
        }
        public Brush                    YearListBackground {
            get { return (Brush)GetValue( YearListBackgroundProperty ); }
            set { SetValue( YearListBackgroundProperty, value ); }
        }
        public Brush                    YearListForeground {
            get { return (Brush)GetValue( YearListForegroundProperty ); }
            set { SetValue( YearListForegroundProperty, value ); }
        }


        public event PropertyChangedValEventHandler<int>        MonthChanged {
            add     => AddHandler( MonthChangedEvent,       value );
            remove  => RemoveHandler( MonthChangedEvent,    value );
        }
        public event PropertyChangingValEventHandler<int>       MonthChanging {
            add     => AddHandler( MonthChangingEvent,      value );
            remove  => RemoveHandler( MonthChangingEvent,   value );
        }
        public event PropertyChangedValEventHandler<int>        YearChanged {
            add     => AddHandler( YearChangedEvent,        value );
            remove  => RemoveHandler( YearChangedEvent,     value );
        }
        public event PropertyChangingValEventHandler<int>       YearChanging {
            add     => AddHandler( YearChangingEvent,       value );
            remove  => RemoveHandler( YearChangingEvent,    value );
        }
        public event PropertyChangedRefEventHandler<DayView>    DateChanged {
            add     => AddHandler( DateChangedEvent,        value );
            remove  => RemoveHandler( DateChangedEvent,     value );
        }
        public event PropertyChangingRefEventHandler<DayView>   DateChanging {
            add     => AddHandler( DateChangingEvent,       value );
            remove  => RemoveHandler( DateChangingEvent,    value );
        }


        public void ClearSelected() {

            DayView     dv;


            for( int index = 0; index < Days.Count; index++ ) {
                dv = Days[index];

                if( dv.IsChecked ) dv.IsChecked = false;
            }
        }
        public void MonthNext() {
            
            if( DisplayDate.Year < 9999 ) DisplayDate = DisplayDate.AddMonths( 1 );
        }
        public void MonthPrevious() {
            
            if( DisplayDate.Year > 1 ) DisplayDate = DisplayDate.AddMonths( -1 );
        }
        public void Today() {

            SelectedDate = DateTime.Today;
        }

        public DayView[] GetSelectedItems() {

            List<DayView>   result  = new List<DayView>();


            for( int index = 0; index < Days.Count; index++ ) {
                if( Days[index].IsChecked ) {
                    result.Add( Days[index] );
                }
            }

            return result.ToArray();
        }

        protected void RecalculateCalendar( DateTime? displayMonth = null ) {

            int         dayOfWeekIndex;
            int         startDayOfWeek;

            DateTime    date;

            DayView     dv;


            ClearSelected();

            date                        = displayMonth ?? DisplayDate;
            displayDate                 = date;
            date                        = new DateTime( displayDate.Year, displayDate.Month, 1 );

            if( lnkMonth != null && lnkYear != null && spnDays != null ) {
                lnkMonth.Inlines.Clear();
                lnkMonth.Inlines.Add( monthNames[date.Month - 1] );

                UpdateSelectedDays();

                lnkYear.Inlines.Clear();
                lnkYear.Inlines.Add( date.Year.ToString() );
            }

            startDayOfWeek                      = (int)FirstDayOfWeek - (int)date.DayOfWeek;

            for( int index = 0; index < daysOfWeek.Length; index++ ) {
                dayOfWeekIndex                  = index + (int)FirstDayOfWeek;

                if( dayOfWeekIndex > 6 ) dayOfWeekIndex -= 7;

                daysOfWeek[index]!.Foreground   = Foreground;
                daysOfWeek[index]!.Text         = dayOfWeekNames[dayOfWeekIndex];
            }

            date = date.AddDays( startDayOfWeek );
            Days.Clear();

            for( int index = 0; index < MaxDaysInCalendar; index++ ) {
                dv                              = dayViewItems[index]!;
                dv.SetParentAndDate( this, date );

                dv.Visibility = !ShowTargetMonthOnly || date.Month == displayDate.Month ? Visibility.Visible : Visibility.Hidden;

                if( dv.Visibility == Visibility.Visible ) {
                    Days.Add( dv );

                    if( date == DisplayDate ) {
                        dv.IsChecked = true;
                    }
                }

                date = date.AddDays( 1 );
            }
        }

        private void InitializeMonthListUIItems() {

            grdMonthList = (Grid?)this.Template?.FindName( nameof( grdMonthList ), this );

            for( int index = 0; index < MonthsInYear; index++ ) {
                monthList[index] = (Button?)this.Template?.FindName( $"btnMonthList{index:00}", this );

                if( monthList[index] != null ) {
                    monthList[index]!.FontSize  = Width < 260 ? 11d : 12d;
                    monthList[index]!.Content   = monthNames[index];
                    monthList[index]!.Click     += MonthList_Click;
                }
            }

            btnMonthNext    = (Button?)this.Template?.FindName( nameof( btnMonthNext ),     this );
            btnMonthPrev    = (Button?)this.Template?.FindName( nameof( btnMonthPrev ),     this );
            tbMonthListYear = (TextBlock?)this.Template?.FindName( nameof(tbMonthListYear), this );

            if( btnMonthNext != null && btnMonthPrev != null ) {
                btnMonthNext.Click += btnMonthNext_Click;
                btnMonthPrev.Click += btnMonthPrev_Click;
            }
        }
        private void InitializeMonthViewUIItems() {

            for( int index = 0; index < DaysInWeek; index++ ) {
                daysOfWeek[index]           = (TextBlock?)this.Template?.FindName( $"tb{index:00}", this );
            }

            for( int index = 0; index < MaxDaysInCalendar; index++ ) {
                dayViewItems[index]             = (DayView?)this.Template?.FindName( $"dv{index:00}", this );
                dayViewItems[index]!.Checked    += DayView_Checked;
                dayViewItems[index]!.Click      += DayView_Click;
                dayViewItems[index]!.Unchecked  += DayView_Unchecked;
            }

            grdMonthView                    = (Grid?)this.Template?.FindName( nameof( grdMonthView ), this );
            btnPrevious                     = (Button?)this.Template?.FindName( nameof( btnPrevious ), this );
            btnNext                         = (Button?)this.Template?.FindName( nameof( btnNext ), this );
            tbMonthYear                     = (TextBlock?)this.Template?.FindName( nameof( tbMonthYear ), this );
            lnkMonth                        = (Hyperlink?)this.Template?.FindName( nameof( lnkMonth ), this );
            lnkYear                         = (Hyperlink?)this.Template?.FindName( nameof( lnkYear ), this );
            spnDays                         = (Span?)this.Template?.FindName( nameof( spnDays ), this );

            if( btnPrevious != null && btnNext != null ) {
                if( ShowPreviousNextButtons ) {
                    btnPrevious.Click       += btnPrevious_Click;
                    btnNext.Click           += btnNext_Click;
                }
                else {
                    btnPrevious.Visibility  = Visibility.Collapsed;
                    btnNext.Visibility      = Visibility.Collapsed;
                }
            }

            if( lnkMonth != null && lnkYear != null ) {
                lnkMonth.Click              += lnkMonth_Click;
                lnkYear.Click               += lnkYear_Click;
            }
        }
        private void InitializeYearListUIItems() {

            grdYearList                     = (Grid?)this.Template?.FindName( nameof(grdYearList),          this );

            btnDecadeNext                   = (Button?)this.Template?.FindName( nameof(btnDecadeNext),      this );
            btnDecadePrev                   = (Button?)this.Template?.FindName( nameof(btnDecadePrev),      this );
            btnEnteredYear                  = (Button?)this.Template?.FindName( nameof(btnEnteredYear),     this );
            tbDecadeYear                    = (TextBlock?)this.Template?.FindName( nameof(tbDecadeYear),    this );
            txtEnteredYear                  = (TextBox?)this.Template?.FindName( nameof(txtEnteredYear),    this );

            btnDecadeNext!.Click            += btnDecadeNext_Click;
            btnDecadePrev!.Click            += btnDecadePrev_Click;
            btnEnteredYear!.Click           += btnEnteredYear_Click;

            for( int index = 0; index < YearsInDecade; index++ ) {
                yearList[index]             = (Button?)this.Template?.FindName( $"btnYearList{index:00}",   this );
                if( yearList[index] != null ) {
                    yearList[index]!.Click  += YearList_Click;
                }
            }
        }
        private void RaiseChangedEvents( DateTime? newDate, DateTime? oldDate ) {

            PropertyChangedValEventArgs<int>        changedIntArgs;
            PropertyChangedValEventArgs<DateTime>   changedArgs;


            if( newDate != null && oldDate != null ) {
                if( newDate.Value.Year != oldDate.Value.Year ) {
                    changedIntArgs                  = new PropertyChangedValEventArgs<int>();
                    changedIntArgs.RoutedEvent      = YearChangedEvent;
                    changedIntArgs.NewValue         = newDate.Value.Year;
                    changedIntArgs.OldValue         = oldDate.Value.Year;

                    RaiseEvent( changedIntArgs );
                }

                if( newDate.Value.Year != oldDate.Value.Year || newDate.Value.Month != oldDate.Value.Month ) {
                    changedIntArgs                  = new PropertyChangedValEventArgs<int>();
                    changedIntArgs.RoutedEvent      = MonthChangedEvent;
                    changedIntArgs.NewValue         = newDate.Value.Month;
                    changedIntArgs.OldValue         = oldDate.Value.Month;

                    RaiseEvent( changedIntArgs );
                }

                if( newDate.Value.Date != oldDate.Value.Date ) {
                    changedArgs                     = new PropertyChangedValEventArgs<DateTime>();
                    changedArgs.RoutedEvent         = DateChangedEvent;
                    changedArgs.NewValue            = newDate.Value;
                    changedArgs.OldValue            = oldDate.Value;

                    RaiseEvent( changedArgs );
                }
            }
            else {
                if( newDate == null ^ oldDate == null ) {
                    changedIntArgs                  = new PropertyChangedValEventArgs<int>();
                    changedIntArgs.RoutedEvent      = YearChangedEvent;
                    changedIntArgs.NewValue         = newDate?.Year ?? 0;
                    changedIntArgs.OldValue         = oldDate?.Year ?? 0;

                    RaiseEvent( changedIntArgs );


                    changedIntArgs                  = new PropertyChangedValEventArgs<int>();
                    changedIntArgs.RoutedEvent      = MonthChangedEvent;
                    changedIntArgs.NewValue         = newDate?.Month ?? 0;
                    changedIntArgs.OldValue         = oldDate?.Month ?? 0;

                    RaiseEvent( changedIntArgs );


                    changedArgs                     = new PropertyChangedValEventArgs<DateTime>();
                    changedArgs.RoutedEvent         = DateChangedEvent;
                    changedArgs.NewValue            = newDate ?? DateTime.MinValue;
                    changedArgs.OldValue            = oldDate ?? DateTime.MinValue;

                    RaiseEvent( changedArgs );
                }
            }
        }
        private void UpdateSelectedDays() {

            int     selectedDays    = 0;


            if( Days.Count == MaxDaysInCalendar ) {
                for( int index = 0; index < MaxDaysInCalendar; index++ ) {
                    if( Days[index].IsChecked ) selectedDays++;
                }

                if( selectedDays > 0 && spnDays != null ) {
                    spnDays.Inlines.Clear();
                    spnDays.Inlines.Add( GetSelectedDays() );
                }
            }
        }
        
        private bool RaiseChangingEvents( DateTime? newDate, DateTime? oldDate ) {

            bool                                    cancelled       = false;

            PropertyChangingValEventArgs<int>       changingIntArgs;
            PropertyChangingValEventArgs<DateTime>  changingArgs;


            if( newDate != null && oldDate != null ) {
                if( newDate.Value.Year != oldDate.Value.Year ) {
                    changingIntArgs                 = new PropertyChangingValEventArgs<int>();
                    changingIntArgs.RoutedEvent     = YearChangingEvent;
                    changingIntArgs.NewValue        = newDate.Value.Year;
                    changingIntArgs.OldValue        = oldDate.Value.Year;

                    RaiseEvent( changingIntArgs );

                    cancelled                       = changingIntArgs.Cancel;
                }

                if( !cancelled && newDate.Value.Year != oldDate.Value.Year || newDate.Value.Month != oldDate.Value.Month ) {
                    changingIntArgs                 = new PropertyChangingValEventArgs<int>();
                    changingIntArgs.RoutedEvent     = MonthChangingEvent;
                    changingIntArgs.NewValue        = newDate.Value.Month;
                    changingIntArgs.OldValue        = oldDate.Value.Month;

                    RaiseEvent( changingIntArgs );

                    cancelled                       = changingIntArgs.Cancel;
                }

                if( !cancelled && newDate.Value.Date != oldDate.Value.Date ) {
                    changingArgs                    = new PropertyChangingValEventArgs<DateTime>();
                    changingArgs.RoutedEvent        = DateChangingEvent;
                    changingArgs.NewValue           = newDate.Value;
                    changingArgs.OldValue           = oldDate.Value;

                    RaiseEvent( changingArgs );

                    cancelled                       = changingArgs.Cancel;
                }
            }
            else {
                if( newDate == null ^ oldDate == null ) {
                    changingIntArgs                 = new PropertyChangingValEventArgs<int>();
                    changingIntArgs.RoutedEvent     = YearChangingEvent;
                    changingIntArgs.NewValue        = newDate?.Year ?? 0;
                    changingIntArgs.OldValue        = oldDate?.Year ?? 0;

                    RaiseEvent( changingIntArgs );

                    cancelled                       = changingIntArgs.Cancel;

                    if( !cancelled ) {
                        changingIntArgs             = new PropertyChangingValEventArgs<int>();
                        changingIntArgs.RoutedEvent = MonthChangingEvent;
                        changingIntArgs.NewValue    = newDate?.Month ?? 0;
                        changingIntArgs.OldValue    = oldDate?.Month ?? 0;

                        RaiseEvent( changingIntArgs );

                        cancelled                   = changingIntArgs.Cancel;
                    }

                    if( !cancelled ) {
                        changingArgs                = new PropertyChangingValEventArgs<DateTime>();
                        changingArgs.RoutedEvent    = DateChangingEvent;
                        changingArgs.NewValue       = newDate ?? DateTime.MinValue;
                        changingArgs.OldValue       = oldDate ?? DateTime.MinValue;

                        RaiseEvent( changingArgs );

                        cancelled                   = changingArgs.Cancel;
                    }
                }
            }


            return cancelled;
        }
        private string GetSelectedDays() {

            DayView             dv;
            StringBuilder       sb  = new StringBuilder();


            for( int index = 0; index < MaxDaysInCalendar; index++ ) {
                dv = Days[index];

                if( dv.IsChecked ) {
                    if( sb.Length > 0 ) sb.Append( ", ");

                    sb.Append( dv.Date.Day.ToString() );
                }
            }

            return sb.ToString();
        }

        private void MonthView_Loaded               ( object sender, RoutedEventArgs e ) {
            
            if( this.Template != null ) {
                InitializeMonthViewUIItems();
                InitializeMonthListUIItems();
                InitializeYearListUIItems();

                RecalculateCalendar();
            }
        }

        private void DayView_Click                  ( object sender, RoutedEventArgs e ) {}
        private void DayView_Checked                ( object sender, RoutedEventArgs e ) {

            DayView?    day     = sender as DayView;


            if( SelectionMode == MonthViewSelectionMode.Single ) {
                for( int index = 0; index < Days.Count; index++ ) {
                    Days[index].IsChecked = Object.ReferenceEquals( Days[index], day );
                }
            }

            if( day != null ) SelectedDate = day.Date;

            UpdateSelectedDays();
        }
        private void DayView_Unchecked              ( object sender, RoutedEventArgs e ) {}

        private void MonthList_Click                ( object sender, RoutedEventArgs e ) {

            int             monthIndex;
            int             year;

            Button          btn;
            

            btn                         = (Button)sender;
            monthIndex                  = Int32.Parse( btn.Name.Substring( btn.Name.Length - 2, 2 ) );
            year                        = Int32.Parse( tbMonthListYear!.Text );
            DisplayDate                 = new DateTime( year, monthIndex + 1, displayDate.Day );

            if( grdMonthList != null && grdYearList != null && grdMonthView != null ) {
                grdMonthList.Visibility = Visibility.Collapsed;
                grdYearList.Visibility  = Visibility.Collapsed;
                grdMonthView.Visibility = Visibility.Visible;
            }
        }
        private void YearList_Click                 ( object sender, RoutedEventArgs e ) {

            int             lowYear;
            int             year;

            string[]        range;

            Button          btn;


            btn                         = (Button)sender;
            if( btn.Content is string str ) {
                if( str.Contains( '-' ) ) {
                    range               = str.Split( '-' );

                    if( range.Length == 2 ) {
                        lowYear         = Int32.Parse( range[0] );

                        for( int index = 0; index < 10; index++ ) {
                            yearList[index]!.Content    = (lowYear + index).ToString();
                        }
                    }
                }
                else {
                    year    = Int32.Parse( str );

                    DisplayDate = new DateTime( year, displayDate.Month, displayDate.Day );

                    grdMonthView!.Visibility        = Visibility.Visible;
                    grdYearList!.Visibility         = Visibility.Collapsed;
                    grdMonthList!.Visibility        = Visibility.Collapsed;
                }
            }
        }

        private void btnDecadePrev_Click            ( object sender, RoutedEventArgs e ) {

            int             year;

            string          s;


            s                       = tbDecadeYear!.Text;
            if( s.Contains( '-' ) ) {
                year                = Int32.Parse( s.Substring( 0, 4 ) );
                year -= 100;

                tbDecadeYear!.Text = $"{year:0000} - {year + 99:0000}";

                for( int index = 0; index < 10; index++ ) {
                    yearList[index]!.Content    = $"{year + (index * 10):0000} - {year + (index * 10) + 9:0000}";
                }
            }
            else {
                year                = Int32.Parse( s );
                year -= 10;

                tbDecadeYear!.Text  = year.ToString();

                for( int index = 0; index < 10; index++ ) {
                    yearList[index]!.Content    = (year + index).ToString();
                }
            }
        }
        private void btnDecadeNext_Click            ( object sender, RoutedEventArgs e ) {

            int             year;

            string          s;


            s                       = tbDecadeYear!.Text;
            if( s.Contains( '-' ) ) {
                year                = Int32.Parse( s.Substring( 0, 4 ) );
                year += 100;

                tbDecadeYear!.Text = $"{year:0000} - {year + 99:0000}";

                for( int index = 0; index < 10; index++ ) {
                    yearList[index]!.Content    = $"{year + (index * 10):0000} - {year + (index * 10) + 9:0000}";
                }
            }
            else {
                year                = Int32.Parse( s );
                year += 10;

                tbDecadeYear!.Text  = year.ToString();

                for( int index = 0; index < 10; index++ ) {
                    yearList[index]!.Content    = (year + index).ToString();
                }
            }
        }
        private void btnEnteredYear_Click           ( object sender, RoutedEventArgs e ) {

            int     year;


            if( txtEnteredYear != null && Int32.TryParse( txtEnteredYear.Text, out year ) ) {
                DisplayDate                = new DateTime( year, displayDate.Month, displayDate.Day );

                grdMonthView!.Visibility    = Visibility.Visible;
                grdMonthList!.Visibility    = Visibility.Collapsed;
                grdYearList!.Visibility     = Visibility.Collapsed;
            }
        }
        private void btnMonthNext_Click             ( object sender, RoutedEventArgs e ) {

            int     year;


            if( tbMonthListYear != null ) {
                year = Int32.Parse( tbMonthListYear.Text );
                year++;

                tbMonthListYear.Text    = year.ToString();
            }
        }
        private void btnMonthPrev_Click             ( object sender, RoutedEventArgs e ) {

            int     year;


            if( tbMonthListYear != null ) {
                year = Int32.Parse( tbMonthListYear.Text );
                year--;

                tbMonthListYear.Text    = year.ToString();
            }
        }
        private void btnNext_Click                  ( object sender, RoutedEventArgs e ) {

            MonthNext();
        }
        private void btnPrevious_Click              ( object sender, RoutedEventArgs e ) {

            MonthPrevious();
        }
        private void lnkYear_Click                  ( object sender, RoutedEventArgs e ) {

            int         startOfCentury;

            
            if( grdMonthList != null && grdYearList != null && grdMonthView != null ) {
                startOfCentury                  = ((displayDate.Year / 100) * 100);
                tbDecadeYear!.Text              = $"{startOfCentury:0000} - {startOfCentury + 99:0000}";

                for( int index = 0; index < 10; index++ ) {
                    yearList[index]!.Content    = $"{startOfCentury + (index * 10):0000} - {startOfCentury + (index * 10) + 9:0000}";
                }

                grdMonthView.Visibility         = Visibility.Collapsed;
                grdYearList.Visibility          = Visibility.Visible;
                grdMonthList.Visibility         = Visibility.Collapsed;
            }
        }
        private void lnkMonth_Click                 ( object sender, RoutedEventArgs e ) {

            tbMonthListYear!.Text           = displayDate.Year.ToString();

            if( grdMonthList != null && grdYearList != null && grdMonthView != null ) {
                grdMonthView.Visibility     = Visibility.Collapsed;
                grdYearList.Visibility      = Visibility.Collapsed;
                grdMonthList.Visibility     = Visibility.Visible;
            }
        }

        private static void FirstDayOfWeek_Changed  ( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

            if( d is MonthView mv ) {
                mv.RecalculateCalendar();
            }
        }
    }
}