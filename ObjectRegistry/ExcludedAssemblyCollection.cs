using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public class ExcludedAssemblyCollection : UniqueCollection<string> {


        internal ExcludedAssemblyCollection() : base() {}


        public void AddRange    ( string[] assemblyNamespaces ) => base.inner.AddRange  ( assemblyNamespaces );


        public override string ToString() => $"Count: {Count}.";
    }
}