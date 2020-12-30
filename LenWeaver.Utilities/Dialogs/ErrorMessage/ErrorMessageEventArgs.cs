using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class ErrorMessageEventArgs : EventArgs {

        public Exception?   Exception    { get; set; }
    }
}