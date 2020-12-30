using System;
using System.Collections.Generic;


namespace LenWeaver.Utilities {


    public static class SkinManager {

        private static  ISkin?      blue        = null;


        public static ISkin Blue {
            get {
                if( blue == null ) {
                    blue = new Skin( "Blue" );
                }

                return blue;
            }
        }
    }
}