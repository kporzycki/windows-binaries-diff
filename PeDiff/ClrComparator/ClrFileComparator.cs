using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HandlebarsDotNet;
using Mono.Cecil;

namespace PeDiff.ClrComparator
{
    public class ClrFileComparator : IFileComparator
    {
        public void CompareFiles(string originalFileName, string newFileName, string resultFileName)
        {
            var result = CompareFiles(originalFileName, newFileName);
            File.WriteAllText(resultFileName, result);
        }

        public string CompareFiles(string originalFileName, string newFileName)
        {
            var template = Handlebars.Compile(new StreamReader("res/ClrTemplate.html").ReadToEnd());
            var originalModule = ModuleDefinition.ReadModule(originalFileName);
            var newModule = ModuleDefinition.ReadModule(newFileName);
            var originalAssembly = originalModule.Assembly;
            var newAssembly = newModule.Assembly;
            //var classesChangesetComputer = new ChangeSetComputer<string>(new ExportedTypeNameEqualityComparer());
            var classesChangesetComputer =
                new ChangeSetComputer<string>(StringComparer.InvariantCulture);

            var viewModel = new ClrViewModel
            {
                OriginalAssembly = originalAssembly,
                NewAssembly = newAssembly,
                MetadataComparison = CompareMetadata(originalAssembly, newAssembly),
                ClassesChangeset = classesChangesetComputer.GetChangeSet(originalModule.Types.Select(et => et.Name).ToArray(),
                    newModule.Types.Select(et => et.Name).ToArray())
            };

            return template(viewModel);
        }

        private static ComparisonResult[] CompareMetadata(AssemblyDefinition originalAssembly, AssemblyDefinition newAssembly)
        {
            var originalEntryPoint = originalAssembly.EntryPoint != null ? originalAssembly.EntryPoint.Name : "No Entry Point";
            var newEntryPoint = newAssembly.EntryPoint != null ? newAssembly.EntryPoint.Name : "No Entry Point";
            return new[]
            {
                ComparisonResult.CompareValues("Entry point name", originalEntryPoint, newEntryPoint),
            };
        }
    }
}
