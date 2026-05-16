using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public interface ITypeConversionService {

        void            Register                    ( ITypeHandler handler );

        bool            CanHandle                   ( object clrType );
        bool            CanHandle                   ( Type clrType );
        bool            CanHandle                   ( string typeName );

        int             Count                       { get; }

        ITypeHandler    GetHandler                  ( object clrType );
        ITypeHandler    GetHandler                  ( Type clrType );
        ITypeHandler    GetHandler                  ( string typeName );

        T?              ConvertTo<T>                ( object? dbValue );
        object?         ConvertTo                   ( Type targetType, object? dbValue );

        string          GetTypeName                 ( Type clrType );
        Type?           ResolveType                 ( string typeName );
    }
}