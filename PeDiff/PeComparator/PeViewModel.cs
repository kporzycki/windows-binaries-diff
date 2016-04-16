using PeNet;

namespace PeDiff.PeComparator
{
    public class PeViewModel
    {
        public PeFile OriginalPeFile { get; set; }
        public PeFile NewPeFile { get; set; }
        public ChangeSet<string> ExportFunctionChangeset { get; set; }
        public ComparisonResult[] MetadataComparison { get; set; }
    }
}