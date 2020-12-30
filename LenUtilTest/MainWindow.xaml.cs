using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using LenWeaver.Utilities;


namespace LenUtilTest {

    public partial class MainWindow : Window {

        private CountryCode[]           countryCodes;
        private SpecialDate[]           specialDates        = { new SpecialDate( new DateTime( 2020, 5, 2 ), "Testing", SpecialDateType.Holiday ),
                                                                new SpecialDate( new DateTime( 2020, 5, 27 ), "Whatever Day", SpecialDateType.Anniversary ),
                                                                new SpecialDate( new DateTime( 2020, 5, 11 ), "D. Smith B-Day", SpecialDateType.Birthday ) };

        public MainWindow() {

            InitializeComponent();

            Loaded              += MainWindow_Loaded;
            ContentRendered     += MainWindow_ContentRendered;

            countryCodes        = CountryCode.Load();
        }


        private void MainWindow_ContentRendered ( object sender, EventArgs e ) {}
        private void MainWindow_Loaded          ( object sender, RoutedEventArgs e ) {}
        
        private void btnButton_Click            ( object sender, RoutedEventArgs e ) {

            SqlBuilder      sql;


            cboCategory.ItemsSource = countryCodes;

            sql = new SqlBuilder( SqlAction.CreateTable, "GolfCourse" );
            sql.Fields.Add( "GolfCourseID", typeof(int), SqlField.PrimaryKey | SqlField.AutoNumber );
            sql.ToString();
        }

        private void mvMonth_MonthChanged( object sender, PropertyChangedValEventArgs<int> e ) {

            DayView         dv;
            MonthView       mv;


            try {
                mv = sender as MonthView;
                if( mv != null ) {
                    foreach( SpecialDate sd in specialDates ) {
                        if( sd.Date.Month == e.NewValue ) {
                            dv                  = mv.Days[sd.Date];

                            if( dv != null ) {
                                dv.Foreground   = Brushes.Red;
                                dv.FontWeight   = FontWeights.Bold;
                                dv.ToolTip      = sd.Description;
                            }
                        }
                    }
                }
            }
            catch( Exception ex ) {
                ErrorMessage.Show( ex );
            }
        }
    }
}