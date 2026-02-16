using System;
using System.Windows;

namespace LenWeaver.Utilities {

    public class ConnectionStringDialog {

        public Window?              Owner           { get; set; }


        public bool? ShowDialog() {

            ConnectionStringDialogWindow        csdw    = new();

            csdw.Owner = Owner;

            return csdw.ShowDialog();
        }
    }
}