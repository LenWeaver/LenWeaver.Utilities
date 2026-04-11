using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public static class ColorThemes {

        private static IColorTheme?         blue        = null;
        private static IColorTheme?         dark        = null;
        private static IColorTheme?         light       = null;
        private static IColorTheme?         white       = null;

        
        static ColorThemes() {}


        public static IColorTheme           Current     { get; set; }   = White;

        public static IColorTheme           Blue {
            get {
                if( blue == null ) blue = new BlueColorTheme();

                return blue;
            }
        }
        public static IColorTheme           Dark {
            get {
                if( dark == null ) dark = new DarkColorTheme();

                return dark;
            }
        }
        public static IColorTheme           Light {
            get {
                if( light == null ) light = new LightColorTheme();

                return light;
            }
        }
        public static IColorTheme           White {
            get {
                if( white == null ) white = new WhiteColorTheme();

                return white;
            }
        }

        public static IEnumerable<IColorTheme> ForEach() {

            yield return Blue;
            yield return Dark;
            yield return Light;
            yield return White;
        }
    }
}