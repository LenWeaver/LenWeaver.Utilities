using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    /// <summary>Indicates that the attributed class is under active development and may be incomplete or subject to change.</summary>
    /// <remarks>Apply this attribute to classes that are not yet finalized or are still being implemented.
    /// This can help communicate to other developers that the class's API, behaviour, or implementation may change and
    /// that it should not be considered stable for production use.</remarks>
    [AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
    public sealed class UnderConstructionAttribute : Attribute {

        public  string?     Developer       { get; set; }
        public  string?     ToDo            { get; set; }


        public UnderConstructionAttribute() {}
    }
}