using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public interface ITypeConversionService {

        int             Count           { get; }

        void            Register        ( ITypeHandler handler );

        bool            CanHandle       ( Type clrType );
        bool            CanHandle       ( string typeName );

        ITypeHandler    GetHandler      ( Type clrType );
        ITypeHandler    GetHandler      ( string typeName );

        T?              ConvertTo<T>    ( object? dbValue );
        object?         ConvertTo       ( Type targetType, object? dbValue );

        string          GetTypeName     ( Type clrType );
        Type?           ResolveType     ( string typeName );
    }
}