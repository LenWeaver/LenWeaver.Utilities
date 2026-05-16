
namespace LenWeaver.Utilities {

    /// <summary>
    /// Specifies constants that represent the underlying type codes for common CLR (Common Language Runtime) types,
    /// including both standard and extended types not covered by the built-in TypeCode enumeration.
    /// </summary>
    /// <remarks>
    /// This enumeration extends the standard set of type codes defined by the .NET TypeCode enumeration to include
    /// additional types such as byte arrays, DateOnly, TimeOnly, TimeSpan, Guid, and DateTimeOffset. It is useful for
    /// scenarios that require type discrimination, serialization, or mapping between CLR types and their corresponding
    /// codes. The values are stable and can be used for persistence or interoperability purposes.
    /// !A change to this enum will necessitate a change in
    /// <see cref="LenWeaver.Utilities.TypeConversionExtensions.GetCLRTypeCode" />
    /// </remarks>
    public enum CLRTypeCode {
        Empty           =   0,      // Null reference
        Object          =   1,      // Instance that isn't a value type
        DBNull          =   2,      // Database null value
        Boolean         =   3,      // Boolean
        Char            =   4,      // Unicode character
        SByte           =   5,      // Signed 8-bit integer
        Byte            =   6,      // Unsigned 8-bit integer
        Int16           =   7,      // Signed 16-bit integer
        UInt16          =   8,      // Unsigned 16-bit integer
        Int32           =   9,      // Signed 32-bit integer
        UInt32          =  10,      // Unsigned 32-bit integer
        Int64           =  11,      // Signed 64-bit integer
        UInt64          =  12,      // Unsigned 64-bit integer
        Single          =  13,      // IEEE 32-bit float
        Double          =  14,      // IEEE 64-bit double
        Decimal         =  15,      // Decimal
        DateTime        =  16,      // DateTime
        String          =  18,      // Unicode character string
        //Entries above are taken from TypeCode.cs.
        //Entries below are new.
        ///!Microsoft stopped updating TypeCode.cs in .Net 5 and later.
        ByteArray       = 100,
        DateOnly        = 101,      //Date
        TimeOnly        = 102,      //Time
        TimeSpan        = 103,
        Guid            = 104,
        DateTimeOffset  = 105
    }
}