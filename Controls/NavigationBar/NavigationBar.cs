using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LenWeaver.Utilities {

    public class NavigationBar : ItemsControl {

        internal static     FontFamily?         defaultFontFamily           = null;
        internal static     FontFamily?         defaultSymbolFontFamily     = null;

        protected readonly  double              DefaultOpenedWidth          = 160d;

        protected           bool                animationInProgress         = false;
        protected           double?             openedWidth                 = null;
        protected           DoubleAnimation?    aniClose                    = null;
        protected           DoubleAnimation?    aniOpen                     = null;
        protected           Border?             brdBorder                   = null;
        protected           Button?             btnClose                    = null;
        protected           Button?             btnOpen                     = null;
        protected           ContentPresenter?   cpLowerContent              = null;
        protected           ContentPresenter?   cpTitleContent              = null;
        protected           ContentPresenter?   cpUpperContent              = null;
        protected           NavigationBarItem?  selectedItem                = null;
        protected           StackPanel?         spBottomItems               = null;
        protected           StackPanel?         spItems                     = null;
        protected           ArrayList           addedBeforeTemplate         = new ArrayList();


        #region Static Properties
        public static FontFamily DefaultFontFamily {
            get {
                if( defaultFontFamily == null ) {
                    defaultFontFamily = FontExtensions.SegoeUI;
                }

                return defaultFontFamily;
            }
            set => defaultFontFamily = value;
        }
        public static FontFamily DefaultSymbolFontFamily {
            get {
                if( defaultSymbolFontFamily == null ) {
                    defaultSymbolFontFamily = FontExtensions.SegoeMDL2Assets;
                }

                return defaultSymbolFontFamily;
            }
            set => defaultSymbolFontFamily = value;
        }
        #endregion
        #region DependencyProperty Definitions
        public static readonly          DependencyProperty      AddToProperty =
                                        DependencyProperty.RegisterAttached( "AddTo", typeof(ItemLocation), typeof(NavigationBar),
                                        new PropertyMetadata( ItemLocation.Top ) );

        public static readonly          DependencyProperty      AddSeparatorAfterItemsProperty =
                                        DependencyProperty.Register( nameof(AddSeparatorAfterItems), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( true ) );

        public static readonly          DependencyProperty      AnimateOpenCloseProperty =
                                        DependencyProperty.Register( nameof(AnimateOpenClose), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( true ) );

        public static readonly          DependencyProperty      ClosedWidthProperty =
                                        DependencyProperty.Register( nameof(ClosedWidth), typeof(double), typeof(NavigationBar),
                                        new PropertyMetadata( 52d ) );

        public static readonly          DependencyProperty      ItemFontFamilyProperty =
                                        DependencyProperty.Register( nameof(ItemFontFamily), typeof(FontFamily), typeof(NavigationBar),
                                        new PropertyMetadata( DefaultFontFamily ) );

        public static readonly          DependencyProperty      ItemFontSizeProperty =
                                        DependencyProperty.Register( nameof(ItemFontSize), typeof(double), typeof(NavigationBar),
                                        new PropertyMetadata( 11d ) );

        public static readonly          DependencyProperty      ItemFontStyleProperty =
                                        DependencyProperty.Register( nameof(ItemFontStyle), typeof(FontStyle), typeof(NavigationBar),
                                        new PropertyMetadata( FontStyles.Normal ) );

        public static readonly          DependencyProperty      ItemFontWeightProperty =
                                        DependencyProperty.Register( nameof(ItemFontWeight), typeof(FontWeight), typeof(NavigationBar),
                                        new PropertyMetadata( FontWeights.Normal ) );

        public static readonly          DependencyProperty      ImageSideProperty =
                                        DependencyProperty.Register( nameof(ImageSide), typeof(ImageSide), typeof(NavigationBar),
                                        new PropertyMetadata( ImageSide.LeftSide, null, ImageSide_CoerceValue ) );

        public static readonly          DependencyProperty      ItemHeightProperty =
                                        DependencyProperty.Register( nameof(ItemHeight), typeof(double), typeof(NavigationBar),
                                        new PropertyMetadata( 28d ) );

        public static readonly          DependencyProperty      ItemForegroundProperty =
                                        DependencyProperty.Register( nameof(ItemForeground), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.Black ) );

        public static readonly          DependencyProperty      ItemMouseOverBackgroundProperty =
                                        DependencyProperty.Register( nameof(ItemMouseOverBackground), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( SystemColors.MenuHighlightBrush ) );

        public static readonly          DependencyProperty      ItemMouseOverForegroundProperty =
                                        DependencyProperty.Register( nameof(ItemMouseOverForeground), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( SystemColors.HighlightTextBrush ) );

        public static readonly          DependencyProperty      ItemSymbolPaddingProperty =
                                        DependencyProperty.Register( nameof(ItemSymbolPadding), typeof(Thickness?), typeof(NavigationBar),
                                        new PropertyMetadata( new Thickness( 1d ) ) );

        public static readonly          DependencyProperty      ItemSymbolFontFamilyProperty =
                                        DependencyProperty.Register( nameof(ItemSymbolFontFamily), typeof(FontFamily), typeof(NavigationBar),
                                        new PropertyMetadata( NavigationBar.DefaultSymbolFontFamily ) );

        public static readonly          DependencyProperty      ItemSymbolForegroundProperty =
                                        DependencyProperty.Register( nameof(ItemSymbolForeground), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.Black ) );

        public static readonly          DependencyProperty      ItemSymbolPathFillProperty =
                                        DependencyProperty.Register( nameof(ItemSymbolPathFill), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.Black ) );

        public static readonly          DependencyProperty      ItemSymbolPathStrokeProperty =
                                        DependencyProperty.Register( nameof(ItemSymbolPathStroke), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.White ) );

        public static readonly          DependencyProperty      LowerContentProperty =
                                        DependencyProperty.Register( nameof(LowerContent), typeof(object), typeof(NavigationBar),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      OpenCloseForegroundProperty =
                                        DependencyProperty.Register( nameof(OpenCloseForeground), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.Gray ) );

        public static readonly          DependencyProperty      SelectedItemIndicatorProperty =
                                        DependencyProperty.Register( nameof(SelectedItemIndicator), typeof(Brush), typeof(NavigationBar),
                                        new PropertyMetadata( Brushes.Red ) );

        public static readonly          DependencyProperty      ShowOpenCloseButtonsProperty =
                                        DependencyProperty.Register( nameof(ShowOpenCloseButtons), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( true ) );

        public static readonly          DependencyProperty      ShowSelectionIndicatorProperty =
                                        DependencyProperty.Register( nameof(ShowSelectionIndicator), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( true ) );

        public static readonly          DependencyProperty      StartOpenedProperty =
                                        DependencyProperty.Register( nameof(StartOpened), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( false ) );

        public static readonly          DependencyProperty      SuppressToolTipWhenOpenProperty =
                                        DependencyProperty.Register( nameof(SuppressToolTipWhenOpen), typeof(bool), typeof(NavigationBar),
                                        new PropertyMetadata( true ) );

        public static readonly          DependencyProperty      TitleContentProperty =
                                        DependencyProperty.Register( nameof(TitleContent), typeof(object), typeof(NavigationBar),
                                        new PropertyMetadata( null ) );

        public static readonly          DependencyProperty      UpperContentProperty =
                                        DependencyProperty.Register( nameof(UpperContent), typeof(object), typeof(NavigationBar),
                                        new PropertyMetadata( null ) );

        public static ItemLocation      GetAddTo( DependencyObject d ) {

            return (ItemLocation)d.GetValue( AddToProperty );
        }
        public static void              SetAddTo( DependencyObject d, ItemLocation itemLocation ) {

            d.SetValue( AddToProperty, itemLocation );
        }

        public delegate     void        NavigationBarRoutedEventHandler         ( object sender, NavigationBarRoutedEventArgs e );
        public delegate     void        NavigationBarCancelRoutedEventHandler   ( object sender, NavigationBarCancelRoutedEventArgs e );

        public static readonly          RoutedEvent             SelectedItemChangedEvent =
                                        EventManager.RegisterRoutedEvent( nameof(SelectedItemChanged), RoutingStrategy.Bubble,
                                        typeof(NavigationBarRoutedEventHandler), typeof(NavigationBar) );

        public static readonly          RoutedEvent             SelectedItemChangingEvent =
                                        EventManager.RegisterRoutedEvent( nameof(SelectedItemChanging), RoutingStrategy.Bubble,
                                        typeof(NavigationBarCancelRoutedEventHandler), typeof(NavigationBar) );

        private static object ImageSide_CoerceValue( DependencyObject d, object baseValue ) {
            
            return (ImageSide)(int)baseValue;
        }


        static NavigationBar() {

            DefaultStyleKeyProperty.OverrideMetadata( typeof(NavigationBar), new FrameworkPropertyMetadata( typeof(NavigationBar) ) );
        }
        #endregion


        public NavigationBar() : base() {

            Loaded += NavigationBar_Loaded;
        }


        protected override void         AddChild( object value ) {

            //base.AddChild( value );

            if( value is NavigationBarItem item ) {
                SetNavigationBarItemProperties( item );

                AddNavigationBarItem( item );
            }
        }
        protected override void         OnItemsChanged( NotifyCollectionChangedEventArgs e ) {

            UIElement?       element;


            //base.OnItemsChanged( e );

            if( e.Action == NotifyCollectionChangedAction.Reset ) {
                for( int index = 0; index < Items.Count; index++ ) {
                    element = Items[index] as UIElement;

                    if( element is NavigationBarItem item ) {
                        SetNavigationBarItemProperties( item );
                    }

                    if( element != null ) { 
                        AddNavigationBarItem( element );
                    }
                }
            }
            else if( e.Action == NotifyCollectionChangedAction.Add ) {
                for( int index = 0; index < (e?.NewItems?.Count ?? 0); index++ ) {
                    element = e?.NewItems?[index] as UIElement;

                    if( element is NavigationBarItem item ) {
                        SetNavigationBarItemProperties( item );
                    }

                    if( element != null ) { 
                        AddNavigationBarItem( element );
                    }
                }
            }
        }

        protected void                  AddNavigationBarItem( UIElement item ) {

            if( spBottomItems != null && spItems != null ) {
                if( NavigationBar.GetAddTo( item ) == ItemLocation.Top ) {
                    spItems.Children.Add( item );

                    if( AddSeparatorAfterItems ) spItems.Children.Add( new Separator() );
                }
                else {
                    spBottomItems.Children.Add( item );

                    if( AddSeparatorAfterItems ) spBottomItems.Children.Add( new Separator() );
                }
            }
            else {
                addedBeforeTemplate.Add( item );    //Window template has not been loaded yet.  Save items
                                                    //and add them to the StackPanel from NavigationBar_Loaded.
            }
        }
        protected void                  ConfigureOpenCloseButtons( ImageSide value ) {

            if( btnOpen != null && btnClose != null ) { 
                switch( value ) {
                    case ImageSide.LeftSide:
                        Grid.SetColumn( btnOpen, 0 );
                        Grid.SetColumn( btnClose, 2 );

                        btnOpen.Margin          = new Thickness( 12d, 0d, 0d, 0d );
                        btnClose.Content        = "\uE0A6";
                        btnClose.FontFamily     = new FontFamily( "Segoe MDL2 Assets" );
                        break;

                    case ImageSide.RightSide:
                        Grid.SetColumn( btnClose, 0 );
                        Grid.SetColumn( btnOpen, 2 );

                        btnOpen.Margin          = new Thickness( 0d, 0d, 12d, 0d );
                        btnClose.Content        = "\uE0AB";
                        btnClose.FontFamily     = new FontFamily( "Segoe MDL2 Assets" );
                        break;

                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException( "Unknown value specified for ImageSide property." );
                }
            }
        }
        protected void                  SetContentPresenterVisibility( bool? isOpen = null ) {

            if( cpLowerContent != null && cpTitleContent != null && cpUpperContent != null ) {
                if( isOpen ?? IsOpen ) {
                    cpLowerContent.Visibility = cpLowerContent.Content == null ? Visibility.Collapsed : Visibility.Visible;
                    cpTitleContent.Visibility = cpTitleContent.Content == null ? Visibility.Collapsed : Visibility.Visible;
                    cpUpperContent.Visibility = cpUpperContent.Content == null ? Visibility.Collapsed : Visibility.Visible;
                }
                else {
                    cpLowerContent.Visibility = cpLowerContent.Content == null ? Visibility.Collapsed : Visibility.Hidden;
                    cpTitleContent.Visibility = cpTitleContent.Content == null ? Visibility.Collapsed : Visibility.Hidden;
                    cpUpperContent.Visibility = cpUpperContent.Content == null ? Visibility.Collapsed : Visibility.Hidden;
                }
            }
        }
        protected void                  SetNavigationBarItemProperties( NavigationBarItem item ) {

            if( item.Foreground == null && ItemForeground != null )                     item.Foreground             = ItemForeground;
            if( item.FontFamily == null && ItemFontFamily != null )                     item.FontFamily             = ItemFontFamily;
            if( item.MouseOverBackground == null && ItemMouseOverBackground != null )   item.MouseOverBackground    = ItemMouseOverBackground;
            if( item.MouseOverForeground == null && ItemMouseOverBackground != null )   item.MouseOverForeground    = ItemMouseOverForeground;

            if( item.SymbolFontFamily == null && ItemSymbolFontFamily != null )         item.SymbolFontFamily       = ItemSymbolFontFamily;
            if( item.SymbolForeground == null && ItemSymbolForeground != null )         item.SymbolForeground       = ItemSymbolForeground;
            if( item.SymbolPadding == null && ItemSymbolPadding != null )               item.SymbolPadding          = ItemSymbolPadding;
            if( item.SymbolPathFill == null && ItemSymbolPathFill != null )             item.SymbolPathFill         = ItemSymbolPathFill;
            if( item.SymbolPathStroke == null && ItemSymbolPathStroke != null )         item.SymbolPathStroke       = ItemSymbolPathStroke;

            item.ParentNavigationBar    = this;
            item.FontSize               = ItemFontSize;
            item.FontStyle              = ItemFontStyle;
            item.FontWeight             = ItemFontWeight;
            item.Foreground             = ItemForeground;
            item.Height                 = ItemHeight;
        }
        protected double                CalculateOpenedWidth() {

            double          openedWidth;


            openedWidth = brdBorder?.ActualWidth ?? DefaultOpenedWidth;

            if( cpTitleContent != null && cpTitleContent.Visibility == Visibility.Visible ) {
                openedWidth = cpTitleContent.Width > openedWidth ? cpTitleContent.Width : openedWidth;
            }

            if( cpUpperContent != null && cpUpperContent.Visibility == Visibility.Visible ) {
                openedWidth = cpUpperContent.Width > openedWidth ? cpUpperContent.Width : openedWidth;
            }

            if( cpLowerContent != null && cpLowerContent.Visibility == Visibility.Visible ) {
                openedWidth = cpLowerContent.Width > openedWidth ? cpLowerContent.Width : openedWidth;
            }

            for( int index = 0; index < Items.Count; index++ ) {
                if( Items[index] is Control control ) {
                    openedWidth = control.Width > openedWidth ? control.Width : openedWidth;
                }
            }

            openedWidth += 50d;

            return openedWidth;
        }

        public bool                     AddSeparatorAfterItems {
            get { return (bool)GetValue( AddSeparatorAfterItemsProperty ); }
            set { SetValue( AddSeparatorAfterItemsProperty, value ); }
        }
        public bool                     AnimateOpenClose {
            get { return (bool)GetValue( AnimateOpenCloseProperty ); }
            set { SetValue( AnimateOpenCloseProperty, value ); }
        }
        public bool                     IsOpen {
            get { return ActualWidth > (ClosedWidth * 1.1d); }
        }
        public bool                     ShowOpenCloseButtons {
            get { return (bool)GetValue( ShowOpenCloseButtonsProperty ); }
            set { SetValue( ShowOpenCloseButtonsProperty, value ); }
        }
        public bool                     ShowSelectionIndicator {
            get => (bool)GetValue( ShowSelectionIndicatorProperty );
            set => SetValue( ShowSelectionIndicatorProperty, value );
        }
        public bool                     StartOpened {
            get { return (bool)GetValue( StartOpenedProperty ); }
            set { SetValue( StartOpenedProperty, value ); }
        }
        public bool                     SuppressToolTipWhenOpen {
            get => (bool)GetValue( SuppressToolTipWhenOpenProperty );
            set => SetValue( SuppressToolTipWhenOpenProperty, value );
        }
        public double                   ClosedWidth {
            get { return (double)GetValue( ClosedWidthProperty ); }
            set { SetValue( ClosedWidthProperty, value ); }
        }
        public double                   ItemFontSize {
            get { return (double)GetValue( ItemFontSizeProperty ); }
            set { SetValue( ItemFontSizeProperty, value ); }
        }
        public double                   ItemHeight {
            get { return (double)GetValue( ItemHeightProperty ); }
            set { SetValue( ItemHeightProperty, value ); }
        }
        public object                   LowerContent {
            get { return (object)GetValue( LowerContentProperty ); }
            set { SetValue( LowerContentProperty, value ); }
        }
        public object                   TitleContent {
            get { return (object)GetValue( TitleContentProperty ); }
            set { SetValue( TitleContentProperty, value ); }
        }
        public object                   UpperContent {
            get { return (object)GetValue( UpperContentProperty ); }
            set { SetValue( UpperContentProperty, value ); }
        }
        public ImageSide                ImageSide {
            get { return (ImageSide)GetValue( ImageSideProperty ); }
            set {
                SetValue( ImageSideProperty, value );
                ConfigureOpenCloseButtons( value );
            }
        }
        public FontStyle                ItemFontStyle {
            get { return (FontStyle)GetValue( ItemFontStyleProperty ); }
            set { SetValue( ItemFontStyleProperty, value ); }
        }
        public FontWeight               ItemFontWeight {
            get { return (FontWeight)GetValue( ItemFontWeightProperty ); }
            set { SetValue( ItemFontWeightProperty, value ); }
        }
        public Thickness?               ItemSymbolPadding {
            get => (Thickness)GetValue( ItemSymbolPaddingProperty );
            set { SetValue( ItemSymbolPaddingProperty, value ); }
        }
        public Brush                    ItemForeground {
            get { return (Brush)GetValue( ItemForegroundProperty ); }
            set { SetValue( ItemForegroundProperty, value ); }
        }
        public Brush                    ItemMouseOverBackground {
            get { return (Brush)GetValue( ItemMouseOverBackgroundProperty ); }
            set { SetValue( ItemMouseOverBackgroundProperty, value ); }
        }
        public Brush                    ItemMouseOverForeground {
            get => (Brush)GetValue( ItemMouseOverForegroundProperty );
            set => SetValue( ItemMouseOverForegroundProperty, value );
        }
        public Brush                    ItemSymbolForeground {
            get => (Brush)GetValue( ItemSymbolForegroundProperty );
            set => SetValue( ItemSymbolForegroundProperty, value );
        }
        public Brush                    ItemSymbolPathFill {
            get => (Brush)GetValue( ItemSymbolPathFillProperty );
            set => SetValue( ItemSymbolPathFillProperty, value );
        }
        public Brush                    ItemSymbolPathStroke {
            get => (Brush)GetValue( ItemSymbolPathStrokeProperty );
            set => SetValue( ItemSymbolPathStrokeProperty, value );
        }
        public Brush                    OpenCloseForeground {
            get { return (Brush)GetValue( OpenCloseForegroundProperty ); }
            set { SetValue( OpenCloseForegroundProperty, value ); }
        }
        public Brush                    SelectedItemIndicator {
            get { return (Brush)GetValue( SelectedItemIndicatorProperty ); }
            set { SetValue( SelectedItemIndicatorProperty, value ); }
        }
        public FontFamily               ItemFontFamily {
            get { return (FontFamily)GetValue( ItemFontFamilyProperty ); }
            set { SetValue( ItemFontFamilyProperty, value ); }
        }
        public FontFamily               ItemSymbolFontFamily {
            get => (FontFamily)GetValue( ItemSymbolFontFamilyProperty );
            set => SetValue( ItemSymbolFontFamilyProperty, value );
        }

        public  NavigationBarItem?      SelectedItem {
            get => selectedItem;
            set {
                NavigationBarItem?                      oldItem;
                NavigationBarRoutedEventArgs            args;
                NavigationBarCancelRoutedEventArgs      cancelArgs;

                if( !(selectedItem == null && value == null) ) {
                    cancelArgs          = new NavigationBarCancelRoutedEventArgs( SelectedItemChangingEvent, this );
                    cancelArgs.OldItem  = selectedItem;
                    cancelArgs.NewItem  = value;

                    RaiseEvent( cancelArgs );

                    if( !cancelArgs.Cancel ) {
                        if( selectedItem != null ) { 
                            selectedItem.SelectedIndicator  = Brushes.Transparent;
                        }

                        oldItem                         = selectedItem;
                        selectedItem                    = value;

                        if( selectedItem != null && ShowSelectionIndicator ) { 
                            selectedItem.SelectedIndicator  = SelectedItemIndicator;
                        }

                        args            = new NavigationBarRoutedEventArgs( SelectedItemChangedEvent, this );
                        args.OldItem    = oldItem;
                        args.NewItem    = selectedItem;

                        RaiseEvent( args );
                    }
                }
            }
        }

        public event NavigationBarRoutedEventHandler        SelectedItemChanged {
            add => AddHandler( SelectedItemChangedEvent, value );
            remove => RemoveHandler( SelectedItemChangingEvent, value );
        }
        public event NavigationBarCancelRoutedEventHandler  SelectedItemChanging {
            add => AddHandler( SelectedItemChangingEvent, value );
            remove => RemoveHandler( SelectedItemChangingEvent, value );
        }

        public void Close() {

            if( btnOpen != null && btnClose != null ) {
                btnOpen.Visibility          = ShowOpenCloseButtons ? Visibility.Visible : Visibility.Collapsed;
                btnClose.Visibility         = Visibility.Collapsed;
            }

            SetContentPresenterVisibility( isOpen: false );

            if( AnimateOpenClose && !Double.IsNaN( Width ) ) {
                if( !animationInProgress ) {
                    animationInProgress     = true;

                    aniClose                = new DoubleAnimation();
                    aniClose.From           = Width;
                    aniClose.To             = ClosedWidth;
                    aniClose.Duration       = new Duration( TimeSpan.FromMilliseconds( 300d ) );
                    aniClose.FillBehavior   = FillBehavior.HoldEnd;
                    aniClose.Completed      += aniClose_Completed!;

                    this.BeginAnimation( NavigationBar.WidthProperty, aniClose );
                }
            }
            else {
                Width                       = ClosedWidth;
            }
        }
        public void Open() {

            if( btnOpen != null && btnClose != null ) {
                btnOpen.Visibility          = Visibility.Collapsed;
                btnClose.Visibility         = ShowOpenCloseButtons ? Visibility.Visible : Visibility.Collapsed;
            }

            SetContentPresenterVisibility( isOpen: true );

            if( AnimateOpenClose ) {
                if( !animationInProgress ) {
                    animationInProgress     = true;

                    aniOpen                 = new DoubleAnimation();
                    aniOpen.From            = Width;
                    aniOpen.To              = openedWidth ?? DefaultOpenedWidth;
                    aniOpen.Duration        = new Duration( TimeSpan.FromMilliseconds( 200d ) );
                    aniOpen.FillBehavior    = FillBehavior.HoldEnd;
                    aniOpen.Completed       += aniOpen_Completed!;
                    
                    this.BeginAnimation( NavigationBar.WidthProperty, aniOpen );
                }
            }
            else { 
                Width                       = Helpers.Max( openedWidth ?? DefaultOpenedWidth, DefaultOpenedWidth );
            }
        }
        

        private void NavigationBar_Loaded   ( object sender, RoutedEventArgs e ) {
            
            brdBorder           = this?.Template.FindName( nameof(brdBorder),       this ) as Border;

            btnClose            = this?.Template.FindName( nameof(btnClose),        this ) as Button;
            btnOpen             = this?.Template.FindName( nameof(btnOpen),         this ) as Button;

            cpLowerContent      = this?.Template.FindName( nameof(cpLowerContent),  this ) as ContentPresenter;
            cpTitleContent      = this?.Template.FindName( nameof(cpTitleContent),  this ) as ContentPresenter;
            cpUpperContent      = this?.Template.FindName( nameof(cpUpperContent),  this ) as ContentPresenter;

            spItems             = this?.Template.FindName( nameof(spItems),         this ) as StackPanel;
            spBottomItems       = this?.Template.FindName( nameof(spBottomItems),   this ) as StackPanel;

            if( btnClose != null && btnOpen != null ) {
                btnClose.Click  += btnClose_Click;
                btnOpen.Click   += btnOpen_Click;
            }

            for( int index = 0; index < addedBeforeTemplate.Count; index++ ) {
                if( addedBeforeTemplate[index] is UIElement item ) {

                    if( NavigationBar.GetAddTo( item ) == ItemLocation.Top ) {
                        RemoveLogicalChild( item );

                        if( AddSeparatorAfterItems && spItems!.Children.Count == 0 ) {
                            spItems.Children.Add( new Separator() { Background = Brushes.Black } );
                        }

                        spItems!.Children.Add( item );

                        if( AddSeparatorAfterItems ) {
                            spItems.Children.Add( new Separator() { Background = Brushes.Black } );
                        }
                    }
                    else {
                        RemoveLogicalChild( item );

                        if( AddSeparatorAfterItems && spBottomItems!.Children.Count == 0 ) {
                            spBottomItems.Children.Add( new Separator() { Background = Brushes.Black } );
                        }

                        spBottomItems!.Children.Add( item );

                        if( AddSeparatorAfterItems ) {
                            spBottomItems.Children.Add( new Separator() { Background = Brushes.Black } );
                        }
                    }
                }
            }

            addedBeforeTemplate.Clear();
            ConfigureOpenCloseButtons( ImageSide );

            if( StartOpened ) {
                Open();
            }
            else {
                Close();
            }
        }

        private void aniClose_Completed     ( object sender, EventArgs e ) {

            animationInProgress = false;

            this.BeginAnimation( NavigationBar.WidthProperty, null );

            Width = ClosedWidth;
        }
        private void aniOpen_Completed      ( object sender, EventArgs e ) {

            animationInProgress = false;

            this.BeginAnimation( NavigationBar.WidthProperty, null );

            if( openedWidth == null ) { 
                openedWidth         = CalculateOpenedWidth();
            }

            Width                   = openedWidth.Value;
        }
        private void btnClose_Click         ( object sender, RoutedEventArgs e ) {

            Close();
        }
        private void btnOpen_Click          ( object sender, RoutedEventArgs e ) {

            Open();
        }
    }
}