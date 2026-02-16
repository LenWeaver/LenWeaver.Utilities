using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LenWeaver.Utilities {

    internal partial class NoticeBoxInternal : Window {

        internal NoticeBoxInternal() {

            double      maxWidth    = 0d;


            InitializeComponent();


            Dispatcher.BeginInvoke( new Action(() => {

                maxWidth = grdButtons.Children
                                     .OfType<Button>()
                                     .Max( b => b.ActualWidth );

                maxWidth += NoticeBox.ButtonExtraWidth;

                foreach( Button btn in grdButtons.Children.OfType<Button>() ) {
                    btn.Width = maxWidth;
                }

            }), DispatcherPriority.Loaded );
        }


        internal int Result {
            get {
                return -1;
            }
        }
    }
}