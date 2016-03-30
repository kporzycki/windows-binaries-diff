using PeNet;

namespace PeDiff.PeComparator
{
    class PeViewModel
    {
        public PeFile OriginalPeFile { get; set; }
        public PeFile NewPeFile { get; set; }
        public ChangeSet<PeFile.ExportFunction> ExportFunctionChangeset { get; set; }
    }
}