using Mono.Cecil;

namespace PeDiff.ClrComparator
{
    public class ClrViewModel
    {
        public AssemblyDefinition OriginalAssembly { get; set; }
        public AssemblyDefinition NewAssembly { get; set; }
        public ChangeSet<string> ClassesChangeset { get; set; }
        public ComparisonResult[] MetadataComparison { get; set; }
    }
}