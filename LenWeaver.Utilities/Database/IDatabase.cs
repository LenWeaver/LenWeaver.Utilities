using System;
using System.Data;


namespace LenWeaver.Utilities {
    
    public interface IDatabase {
        IDbConnection       Connection              { get; }


        void                ExecuteNonQuery         ( IDbTransaction transaction, string command );
        void                ExecuteNonQuery         ( IDbTransaction transaction, params string[] commands );
        void                ExecuteNonQuery         ( string command );
        void                ExecuteNonQuery         ( params string[] commands );

        object              ExecuteScalar           ( IDbTransaction transaction, string command );
        object              ExecuteScalar           ( string command );

        IDbTransaction      BeginTransaction        ();


        T                   ExecuteScalar<T>        ( IDbTransaction transaction, string command, T defaultValue );
        T                   ExecuteScalar<T>        ( string command, T defaultValue );

        DataSet             Execute                 ( IDbTransaction transaction, string command );
        DataSet             Execute                 ( IDbTransaction transaction, params string[] commands );
        DataSet             Execute                 ( string command );
        DataSet             Execute                 ( params string[] commands );
    }
}