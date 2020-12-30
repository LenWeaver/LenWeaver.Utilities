using System;

namespace LenWeaver.Utilities {

    public interface IDatabaseParameters {
        char                            ClosingDateQuote            { get; }
        char                            ClosingStringQuote          { get; }
        char                            NameValueDelimiter          { get; }
        char                            OpeningDateQuote            { get; }
        char                            OpeningStringQuote          { get; }

        string                          DateFormat                  { get; }
        string                          DateTimeFormat              { get; }
        string                          TimeFormat                  { get; }

        string                          CreateDatabaseTemplate      { get; }
        string                          CreateTableTemplate         { get; }
        string                          DeleteCommandTemplate       { get; }
        string                          InsertCommandTemplate       { get; }
        string                          NameValueTemplate           { get; }
        string                          NullTemplate                { get; }
        string                          UpdateCommandTemplate       { get; }

        DatabaseType                    DatabaseType                { get; }

        StringCharacterSubstitute       EscapedCharacters           { get; }
    }
}