using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class InstanceControllerEventArgs : EventArgs {

        public  byte[]  Data        { get; internal set; }


        internal InstanceControllerEventArgs( byte[] data ) {

            Data = data;
        }
    }
}