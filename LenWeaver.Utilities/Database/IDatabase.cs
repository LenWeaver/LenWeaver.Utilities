using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace LenWeaver.Utilities {
    
    public interface IDatabase {
        void    ExecuteNonQuery         ( DbTransaction transaction, params string[] commands );
        void    ExecuteNonQuery         ( params string[] commands );

        object  ExecuteScalar           ( DbTransaction transaction, string command );
        object  ExecuteScalar           ( string command );

        T       ExecuteScalar<T>        ( DbTransaction transaction, string command, T defaultValue );
        T       ExecuteScalar<T>        ( string command, T defaultValue );

        DataSet Execute                 ( DbTransaction transaction, params string[] commands );
        DataSet Execute                 ( params string[] commands );
    }
}