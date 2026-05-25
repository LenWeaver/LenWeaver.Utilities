using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class NumericUpDownValueChangedCommandParameters {

        public decimal          OldValue    { get; }
        public decimal          NewValue    { get; }

        public NumericUpDown    Source      { get; }


        public NumericUpDownValueChangedCommandParameters( decimal oldValue, decimal newValue, NumericUpDown source ) {

            OldValue    = oldValue;
            NewValue    = newValue;
            Source      = source;
        }
    }
}