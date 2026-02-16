using System;
using System.Collections.Generic;
using System.Text;


namespace LenWeaver.Utilities {

    public class CommandLineParser {

        private string[] args;


        public CommandLineParser( string[] args ) {

            this.args = args;
        }

        public override string ToString() => String.Join<string>( ' ', args );
    }
}