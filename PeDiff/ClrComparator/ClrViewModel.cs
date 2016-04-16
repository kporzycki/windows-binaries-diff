using System;
using System.Reflection;

namespace PeDiff.ClrComparator
{
    public class ClrViewModel
    {
        public Assembly OriginalAssembly { get; set; }
        public Assembly NewAssembly { get; set; }
        public ChangeSet<Type> ClassesChangeset { get; set; }
        public ComparisonResult[] MetadataComparison { get; set; }
    }
}