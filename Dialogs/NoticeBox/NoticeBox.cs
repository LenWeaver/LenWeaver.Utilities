using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace LenWeaver.Utilities {

    public static class NoticeBox {

        #region Static Properties
        private static double   iconSize                                    =   32;

        public static int       MaximumButtonCount          { get; }        =    5;
        public static double    ButtonExtraWidth            { get; }        =  30d;
        public static double    ButtonHeight                { get; }        =  24d;
        public static double    IconSize {
            get => iconSize;
            set {
                if( value < 32d || value > 256d ) throw new ArgumentOutOfRangeException( nameof(IconSize) );

                iconSize = value;
            }
        }
        public static double    PixelsBetweenButtons        { get; }        =  16d;

        public static string    DefaultCancelText           { get; set; }   = "Cancel";
        public static string    DefaultOKText               { get; set; }   = "OK";
        public static string    DefaultNoneText             { get; set; }   = "None";
        public static string    DefaultNoText               { get; set; }   = "No";
        public static string    DefaultTitleText            { get; set; }   = "Attention...";
        public static string    DefaultYesText              { get; set; }   = "Yes";

        public static Style?    ButtonStyle                 { get; set; }   = null;
        #endregion


        public static NoticeBoxResult Show( string message ) {

            return Show( null, DefaultTitleText, message, NoticeBoxImage.None, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( string message, NoticeBoxImage noticeBoxImage ) {

            return Show( null, DefaultTitleText, message, noticeBoxImage, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( string message, NoticeBoxImage noticeBoxImage, params NoticeBoxButton[] btns ) {

            return Show( null, DefaultTitleText, message, noticeBoxImage, btns );
        }
        public static NoticeBoxResult Show( string title, string message ) {

            return Show( null, title, message, NoticeBoxImage.None, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( string title, string message, NoticeBoxImage noticeBoxImage ) {

            return Show( null, title, message, noticeBoxImage, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( string title, string message, params NoticeBoxButton[] btns ) {

            return Show( null, title, message, NoticeBoxImage.None, btns );
        }
        public static NoticeBoxResult Show( string title, string message, NoticeBoxImage noticeBoxImage, params NoticeBoxButton[] btns ) {

            return Show( null, title, message, noticeBoxImage, btns );
        }
        public static NoticeBoxResult Show( Window w, string message ) {

            return Show( w, DefaultTitleText, message, NoticeBoxImage.None, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( Window w, string message, NoticeBoxImage noticeBoxImage ) {

            return Show( w, DefaultTitleText, message, noticeBoxImage, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( Window w, string message, NoticeBoxImage noticeBoxImage, params NoticeBoxButton[] btns ) {

            return Show( w, DefaultTitleText, message, noticeBoxImage, btns );
        }
        public static NoticeBoxResult Show( Window w, string title, string message ) {

            return Show( w, title, message, NoticeBoxImage.Information, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( Window w, string title, string message, NoticeBoxImage noticeBoxImage ) {

            return Show( w, title, message, noticeBoxImage, NoticeBoxButton.OK );
        }
        public static NoticeBoxResult Show( Window w, string title, string message, params NoticeBoxButton[] btns ) {

            return Show( w, title, message, NoticeBoxImage.None, btns );
        }
        public static NoticeBoxResult Show( Window? w, string? title, string message, NoticeBoxImage noticeBoxImage, params NoticeBoxButton[] btns ) {

            int                                 columnIndex;

            NoticeBoxResult                     result          = NoticeBoxResult.None;

            Button                              btn;
            Separator                           sep;

            NoticeBoxInternal                   box;


            try {
                box                             = new NoticeBoxInternal();

                box.Owner                       = w;
                box.WindowStartupLocation       = w     is null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
                box.Title                       = title is null ? DefaultTitleText : title;
                box.tbMessage.Text              = message;

                if( NoticeBoxImageHelpers.IsDefined( noticeBoxImage ) && noticeBoxImage != NoticeBoxImage.None ) {
                    box.imgImage.Source         = GetNoticeBoxImage( noticeBoxImage );
                    box.imgImage.RenderSize     = new Size( IconSize, IconSize );
                    box.imgImage.Visibility     = Visibility.Visible;
                }

                columnIndex                     = 2;

                foreach( NoticeBoxButton nbb in btns ) {
                    btn                         = new Button();
                    btn.Content                 = nbb.Text;
                    btn.Tag                     = nbb;
                    btn.Click                   += ( s, e ) => {
                                                        result = ((NoticeBoxButton)((Button)s).Tag).Result;
                                                        box.Close();
                                                };

                    box.grdButtons.ColumnDefinitions.AddAuto();
                    box.grdButtons.Children.Add( btn );

                    Grid.SetColumn( btn, columnIndex );
                    Grid.SetRow( btn, 1 );

                    sep                         = new Separator();

                    box.grdButtons.ColumnDefinitions.Add( PixelsBetweenButtons );

                    Grid.SetColumn( sep, columnIndex + 1 );
                    Grid.SetRow( sep, 1 );

                    btn.Style                   = box.Resources.FindStyle( "RegularButton" );

                    columnIndex += 2;
                }

                box.ShowDialog();
            }
            catch( Exception ex ) {
                ErrorMessage.Show( ex );
            }

            return result;
        }

        private static BitmapImage? GetNoticeBoxImage( NoticeBoxImage img ) {

            string          imageName;
            string          uriString;

            BitmapImage?    result      = null;


            switch( img ) {
                case NoticeBoxImage.None:           imageName = String.Empty;               break;
                case NoticeBoxImage.Error:          imageName = "Message-Error.png";        break;
                case NoticeBoxImage.Stop:           imageName = "Message-Stop.png";         break;
                case NoticeBoxImage.Question:       imageName = "Message-Question.png";     break;
                case NoticeBoxImage.Exclamation:    imageName = "Message-Exclamation.png";  break;
                case NoticeBoxImage.Warning:        imageName = "Message-Warning.png";      break;
                case NoticeBoxImage.Information:    imageName = "Message-Information.png";  break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException( $"Argument {nameof(img)} does not contain a value from {nameof(NoticeBoxImage)} enum." );
            }

            if( !String.IsNullOrEmpty( imageName ) ) {
                uriString = $"pack://application:,,,/LenWeaver.Utilities;component/Images/{imageName}";

                result = new BitmapImage( new Uri( uriString, UriKind.RelativeOrAbsolute ) );
            }

            return result;
        }
    }
}