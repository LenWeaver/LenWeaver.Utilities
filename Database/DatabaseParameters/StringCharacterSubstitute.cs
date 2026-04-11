using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class StringCharacterSubstitute : SimpleDictionaryBase<char,string> {

        internal StringCharacterSubstitute() : base() {}


        public void Add( char character, string substitute ) => base.inner.Add( character, substitute );
    }
}
