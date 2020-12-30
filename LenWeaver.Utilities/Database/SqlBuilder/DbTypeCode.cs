using System;

namespace LenWeaver.Utilities {

    public enum DbTypeCode {
        Empty,          //Indicates an unused state.
        Object,
        DBNull,
        Boolean,
        Char,
        SByte,
        Byte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Single,
        Double,
        Decimal,
        DateTime,
        Date,
        Time,
        String,
        Binary
    }
}