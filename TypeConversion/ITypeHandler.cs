using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public interface ITypeHandler {

        bool            IsBlob          { get; }
        string          TypeName        { get; }
        Type            CLRType         { get; }

        string?         ToText          ( object? clrValue );
        byte[]?         ToBlob          ( object? clrValue );

        object?         FromDbValue     ( object? dbValue );
        object?         ToDbValue       ( object? clrValue );

        object?         FromBlob        ( byte[]? blob );
        object?         FromText        ( string? text );
    }
}