using System;

namespace LenWeaver.Utilities {

    public interface IDatabaseParameters {
        char                            ClosingDateQuote            { get; }
        char                            OpeningDateQuote            { get; }
        char                            ClosingStringQuote          { get; }
        char                            OpeningStringQuote          { get; }

        string                          DateFormat                  { get; }
        string                          DateTimeFormat              { get; }
        string                          TimeFormat                  { get; }

        string                          CreateDatabaseTemplate      { get; }
        string                          CreateTableTemplate         { get; }
        string                          DeleteCommandTemplate       { get; }
        string                          InsertCommandTemplate       { get; }
        string                          UpdateCommandTemplate       { get; }

        string                          Name                        { get; }        //Name of the database engine

        StringCharacterSubstitute       EscapedCharacters           { get; }
    }
}