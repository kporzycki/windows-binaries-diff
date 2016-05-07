using System;
using System.IO;
using System.Linq;
using HandlebarsDotNet;
using PeNet;

namespace PeDiff.PeComparator
{
    public class PeFileComparator : IFileComparator
    {
        public void CompareFiles(string originalFileName, string newFileName, string resultFileName)
        {
            var result = CompareFiles(originalFileName, newFileName);
            File.WriteAllText(resultFileName, result);
        }

        public string CompareFiles(string originalFileName, string newFileName)
        {
            var template = Handlebars.Compile(new StreamReader("res/PeTemplate.html").ReadToEnd());

            var originalPe = new PeFile(originalFileName);
            var newPe = new PeFile(newFileName);
            var exportFunctionChangesetComputer =
                new ChangeSetComputer<string>(StringComparer.InvariantCulture);

            var viewModel = new PeViewModel
            {
                OriginalPeFile = originalPe,
                NewPeFile = newPe,
                MetadataComparison = CompareMetadata(originalPe, newPe),
                ExportFunctionChangeset = exportFunctionChangesetComputer.GetChangeSet(
                    "Exported functions",
                    originalPe.ExportedFunctions.Select(ef => ef.Name).ToList(),
                    newPe.ExportedFunctions.Select(ef => ef.Name).ToList())
            };

            return template(viewModel);
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
