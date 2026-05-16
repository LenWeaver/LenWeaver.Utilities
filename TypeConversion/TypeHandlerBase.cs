using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LenWeaver.Utilities {

    public abstract class TypeHandlerBase<T> : ITypeHandler {

        public          Type                        CLRType         { get; init; }
        public          string                      TypeName        { get; init; }
        public virtual  bool                        IsBlob          => false;

        protected       ITypeConversionService?     TypeConverter   { get; init; }


        protected TypeHandlerBase( ITypeConversionService? typeConverter) {

            CLRType         = typeof(T);
            TypeName        = CLRType.AssemblyQualifiedName!;
            TypeConverter   = typeConverter;
        }
        protected TypeHandlerBase() : this( null ) {}


        public virtual object? FromDbValue      ( object? dbValue ) {

            if( dbValue is null || dbValue is DBNull ) return default(T);
            if( dbValue is T t ) return t;

            return Convert.ChangeType( dbValue, typeof(T) );
        }
        public virtual object? ToDbValue        ( object? clrValue ) {

            if( clrValue is null ) return DBNull.Value;
            if( clrValue is T t ) return t;

            return Convert.ChangeType( clrValue, typeof(T) );
        }

        public virtual string? ToText           ( object? clrValue ) {

            if( clrValue is null ) return null;

            return Convert.ToString( clrValue, CultureInfo.InvariantCulture );
        }
        public virtual byte[]? ToBlob           ( object? clrValue )    => null;

        public virtual object? FromText         ( string? text ) {

            if( String.IsNullOrEmpty( text ) ) return default(T);

            return Convert.ChangeType( text, typeof(T), CultureInfo.InvariantCulture );
        }
        public virtual object? FromBlob         ( byte[]? blob )        => throw new NotSupportedException();
    }
}