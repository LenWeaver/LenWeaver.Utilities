using System;

namespace LenWeaver.Utilities {

    public class SqlServerParameters : IDatabaseParameters {

        private StringCharacterSubstitute?  escapedCharacters       = null;


        public char                         ClosingDateQuote        => '\'';
        public char                         ClosingStringQuote      => '\'';
        public char                         OpeningDateQuote        => '\'';
        public char                         OpeningStringQuote      => '\'';

        public DatabaseType                 DatabaseType            => DatabaseType.SQLServer;

        public string                       DateFormat              => DateTimeHelpers.ISO8601DateFormat;
        public string                       DateTimeFormat          => DateTimeHelpers.ISO8601DateTimeFormat;
        public string                       TimeFormat              => DateTimeHelpers.ISO8601TimeFormat;

        public string                       CreateDatabaseTemplate  => "CREATE DATABASE @DATABASE_NAME@";
        public string                       CreateTableTemplate     => "CREATE TABLE @TABLE_NAME@";

        public string                       NullTemplate            => "NULL";
        public string                       ReturnsIdentityTemplate => "SELECT @@IDENTITY";

        public string                       DeleteCommandTemplate   => "DELETE FROM @TABLE_NAME@ WHERE";
        public string                       InsertCommandTemplate   => "INSERT INTO @TABLE_NAME@ (@TABLE_FIELDS@) VALUES (@TABLE_VALUES@)";
        public string                       UpdateCommandTemplate   => "UPDATE @TABLE_NAME@ SET @NAME_VALUE_PAIRS@ WHERE";


        public StringCharacterSubstitute    EscapedCharacters {
            get {
                if( escapedCharacters == null ) {
                    escapedCharacters = new StringCharacterSubstitute();

                    escapedCharacters.Add( '\'', "''" );
                }

                return escapedCharacters;
            }
        }
    }
}