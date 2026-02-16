using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LenWeaver.Utilities {

    public static class ControlExtensions {


        public static void PerformClick( this Control c ) {

            c.RaiseEvent( new RoutedEventArgs() );
        }
    }
}