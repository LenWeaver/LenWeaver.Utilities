using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    internal class PlacementDescriptor {

        public double   Height      { get; set; }
        public double   Left        { get; set; }
        public double   ScaleX      { get; set; }
        public double   ScaleY      { get; set; }
        public double   Top         { get; set; }
        public double   Width       { get; set; }


        public PlacementDescriptor() {

            Clear();
        }


        public void Clear() {

            Height      = 0d;
            Left        = 0d;
            ScaleX      = 1d;
            ScaleY      = 1d;
            Top         = 0d;
            Width       = 0d;
        }


        public override string ToString() => $"L:{Left:.##}, T:{Top:.##}, X:{ScaleX:.##}, Y:{ScaleY:.##}, H:{Height:.##}, W:{Width:.##}";
    }
}