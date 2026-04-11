using System;

namespace LenWeaver.Utilities {

    public sealed class UnknownEnumValueException<TEnum> : UnknownEnumValueException where TEnum : struct, Enum {

        internal TEnum      enumValue;
        internal object?    underlyingValue     = null;

        public string       ParameterName       { get; internal set; }  = String.Empty;


        public UnknownEnumValueException( string message, Exception inner )                     : base( message, inner ) {}
        public UnknownEnumValueException( string message, string parameterName, TEnum value )   : base( message ) {

            ParameterName   = parameterName;
            EnumValue       = value;
        }
        public UnknownEnumValueException( string message, string parameterName )                : base( message ) {

            ParameterName = parameterName;
        }
        public UnknownEnumValueException( string message, TEnum value )                         : base( message ) {

            EnumValue       = value;
        }
        public UnknownEnumValueException( string message )                                      : base( message ) {}
        public UnknownEnumValueException( TEnum value )                                         : base() {

            EnumValue       = value;
        }
        public UnknownEnumValueException()                                                      : base() {}


        public TEnum    EnumValue {
            get => enumValue;
            set {
                enumValue       = value;

                UnderlyingValue = Convert.ChangeType( value, Enum.GetUnderlyingType( typeof(TEnum) ) );
            }
        }
        public Type     UnderlyingType {
            get => typeof(TEnum).UnderlyingSystemType;
        }
        public object?  UnderlyingValue {
            get => underlyingValue;
            internal set => underlyingValue = value;
        }

        public static void ThrowIfUndefined( TEnum value, string parameterName ) {
            
            string                              message;

            UnknownEnumValueException<TEnum>    ex;


            if( !value.IsValid() ) {
                message             = String.IsNullOrWhiteSpace( parameterName )
                                    ? $"The value of type \"{typeof(TEnum).Name}\" is invalid."
                                    : $"The value of parameter \"{parameterName}\" is not a valid value of type \"{typeof(TEnum).Name}\".";

                ex                  = new UnknownEnumValueException<TEnum>( message, parameterName );
                ex.EnumValue        = value;

                throw ex;
            }
        }
        public static void ThrowIfUndefined( TEnum value ) => ThrowIfUndefined( value, String.Empty );
    }


    /// <summary>Base type used for catching all generic sub-types.</summary>
    public abstract class UnknownEnumValueException : Exception {

        public UnknownEnumValueException( string message, Exception inner ) : base( message, inner ) {}
        public UnknownEnumValueException( string message )                  : base( message ) {}
        public UnknownEnumValueException()                                  : base() {}
    }
}