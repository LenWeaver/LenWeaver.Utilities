using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace LenWeaver.Utilities {


    internal class AnimationValues {

        public double               CheckedLeft             { get; set; }
        public double               CheckedTop              { get; set; }
        public double               UncheckedLeft           { get; set; }
        public double               UncheckedTop            { get; set; }

        public Brush                CheckedBackground       { get; set; }
        public Brush                UncheckedBackground     { get; set; }


        public AnimationValues() {
            
            CheckedLeft                 = 0d;
            CheckedTop                  = 0d;
            UncheckedLeft               = 0d;
            UncheckedTop                = 0d;

            CheckedBackground           = Brushes.Black;
            UncheckedBackground         = Brushes.White;
        }


        public bool                 IsValid {
            get => !(Double.IsNaN( CheckedLeft ) || Double.IsNaN( CheckedTop ) ||
                     Double.IsNaN( UncheckedLeft ) || Double.IsNaN( UncheckedTop ));
        }
    }
}