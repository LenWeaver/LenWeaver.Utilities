using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace LenWeaver.Utilities {

    public sealed class ExceptionBuilder<TException> where TException : Exception {

        private bool                helpLinkAdded       = false;
        private bool                hResultAdded        = false;

        public  int                 LineNumber          { get; }

        public  TException          Exception           { get; }

        public  string              MemberName          { get; }
        public  string              FilePath            { get; }


        internal ExceptionBuilder                       ( TException exception, string memberName, string filePath, int lineNumber ) {

            Exception           = exception;
            MemberName          = memberName;
            FilePath            = filePath;
            LineNumber          = lineNumber;
        }


        public ExceptionBuilder<TException> AddData     ( string name, object? value ) {

            Exception.Data.Add( name, value );

            return this;
        }
        public ExceptionBuilder<TException> SetHelpLink ( string helpLink ) {

            Debug.Assert( helpLinkAdded, $"Help Link already added by {nameof(ExceptionBuilder)}." );

            Exception.HelpLink  = helpLink;
            helpLinkAdded       = true;

            return this;
        }
        public ExceptionBuilder<TException> SetHResult  ( int hResult ) {

            Debug.Assert( hResultAdded, $"HResult already set by {nameof(ExceptionBuilder)}." );

            Exception.HResult   = hResult;
            hResultAdded        = true;

            return this;
        }

        public static implicit operator TException      ( ExceptionBuilder<TException> eb ) => eb.Exception;
    }


    public static class ExceptionBuilder {

        public static ExceptionBuilder<TException> Create<TException>( string message, Exception? inner = null, [CallerMemberName] string memberName = "",
                                                [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0 ) where TException : Exception {

            Type?    type       = typeof(TException);

            // Preferred constructor signatures in order
            Type[][]? sigs = new[] {
                new[] { typeof(string), typeof(Exception) },
                new[] { typeof(string) },
                Type.EmptyTypes
            };

            ConstructorInfo? ctor = sigs
                .Select( sig => type.GetConstructor( sig ) )
                .FirstOrDefault( c => c != null );

            if( ctor == null ) {
                throw new MissingMethodException( $"No usable constructor found for exception type {type.FullName}." );
            }

            object? instance = ctor.GetParameters().Length switch {
                2 => ctor.Invoke( new object?[] { message, inner } ),
                1 => ctor.Invoke( new object?[] { message } ),
                0 => ctor.Invoke( null ),
                _ => throw new InvalidOperationException( "Unexpected constructor signature." )
            };

            TException? ex = (TException)instance!;

            // Try to set caller info if the exception exposes properties
            TrySetProperty( ex, "MemberName",   memberName );
            TrySetProperty( ex, "FilePath",     filePath );
            TrySetProperty( ex, "LineNumber",   lineNumber );

            return new ExceptionBuilder<TException>( ex, memberName, filePath, lineNumber );
        }

        private static void TrySetProperty              ( object obj, string name, object? value ) {

            PropertyInfo? prop = obj.GetType().GetProperty( name, BindingFlags.Public | BindingFlags.Instance );

            if( prop?.CanWrite == true ) prop.SetValue( obj, value );
        }
    }
}