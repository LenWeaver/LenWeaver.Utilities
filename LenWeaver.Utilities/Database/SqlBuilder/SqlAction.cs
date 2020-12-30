using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public enum SqlAction {
        CreateDatabase,
        CreateTable,

        Delete,
        Insert,
        Select,
        Update
    }
}