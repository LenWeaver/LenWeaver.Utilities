using System;
using System.Collections.Generic;
using System.Text;

namespace LenUtilTest {

    public enum SpecialDateType {
        Birthday,
        Anniversary,
        DueDate,
        Appointment,
        Holiday
    }


    public class SpecialDate {

        public  SpecialDateType     Type            { get; internal set; }

        public  DateTime            Date            { get; internal set; }

        public  string              Description     { get; internal set; }


        public SpecialDate( DateTime date, string description, SpecialDateType type ) {

            Date            = date;
            Description     = description;
            Type            = type;
        }
    }
}