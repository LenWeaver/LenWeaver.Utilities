using System;

namespace LenWeaver.Utilities {

    public enum TypeSubCode {
        Empty,          //Indicates an unused state.

        DateOnly,       //Applies only when TypeCode is TypeCode.DateTime
        TimeOnly,       //Applies only when TypeCode is TypeCode.DateTime or
                        //TypeCode is TypeCode.Empty and parameter is a TimeSpan
        Array
    }
}