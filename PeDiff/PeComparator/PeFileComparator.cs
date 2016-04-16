using System;
using System.IO;
using System.Linq;
using DotLiquid;
using PeDiff.PeComparator;
using PeNet;

namespace PeDiff
{
    public class PeFileComparator : IFileComparator
    {
        private readonly Template _template;

        static PeFileComparator()
        {
            DotLiquidHelpers.RegisterViewModel(typeof(PeViewModel));
            DotLiquidHelpers.RegisterViewModel(typeof(PeFile));
        }

        public PeFileComparator()
        {
            _template = Template.Parse(new StreamReader("res/PeTemplate.html").ReadToEnd());
        }

        public void CompareFiles(string originalFileName, string newFileName, string resultFileName)
        {
            var result = CompareFiles(originalFileName, newFileName);
            File.WriteAllText(resultFileName, result);
        }

        public string CompareFiles(string originalFileName, string newFileName)
        {
            var originalPe = new PeFile(originalFileName);
            var newPe = new PeFile(newFileName);
            var exportFunctionChangesetComputer =
                new ChangeSetComputer<PeFile.ExportFunction>(new PeExportFunctionComparator());

            var viewModel = new PeViewModel
            {
                OriginalPeFile = originalPe,
                NewPeFile = newPe,
                MetadataComparison = CompareMetadata(originalPe, newPe),
                ExportFunctionChangeset = exportFunctionChangesetComputer.GetChangeSet(originalPe.ExportedFunctions,
                    newPe.ExportedFunctions)
            };

            return _template.Render(Hash.FromAnonymousObject(viewModel));
        }


        private static ComparisonResult[] CompareMetadata(PeFile originalPe, PeFile newPe)
        {
            return new[]
            {
                ComparisonResult.CompareValues("FileSize", originalPe.FileSize, newPe.FileSize),
                ComparisonResult.CompareValues("Is32Bit", originalPe.Is32Bit, newPe.Is32Bit),
                ComparisonResult.CompareValues("Is64Bit", originalPe.Is64Bit, newPe.Is64Bit),
                ComparisonResult.CompareValues("IsExe", originalPe.IsEXE, newPe.IsEXE),
                ComparisonResult.CompareValues("IsDll", originalPe.IsDLL, newPe.IsDLL),
                ComparisonResult.CompareValues("IsSigned", originalPe.IsSigned, newPe.IsSigned)
            };
        }
    }
}
