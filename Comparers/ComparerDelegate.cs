using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public delegate int ComparerDelegate( object? x, object? y );
    public delegate int ComparerDelegate<T>( T?x, T?y );
}