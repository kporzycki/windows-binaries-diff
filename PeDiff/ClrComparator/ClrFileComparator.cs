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
            using (var typeDefinitionPartialStreamReader = new StreamReader("res/ClrTypeDefinitionPartial.html"))
            {
                var typeDefinitionPartial = Handlebars.Compile(typeDefinitionPartialStreamReader);
                Handlebars.RegisterTemplate("typeDefinition", typeDefinitionPartial);
                var template = Handlebars.Compile(File.ReadAllText("res/ClrTemplate.html"));
                var originalModule = ModuleDefinition.ReadModule(originalFileName);
                var newModule = ModuleDefinition.ReadModule(newFileName);
                var originalAssembly = originalModule.Assembly;
                var newAssembly = newModule.Assembly;
                var viewModel = new ClrViewModel
                {
                    OriginalAssembly = originalAssembly,
                    NewAssembly = newAssembly,
                    MetadataComparison = CompareMetadata(originalAssembly, newAssembly),
                    TypesChangeset = new[]
                    {
                        GetTypesChangeset(originalModule, newModule, t => t.IsClass, "Classes"),
                        GetTypesChangeset(originalModule, newModule, t => t.IsEnum, "Enums"),
                        GetTypesChangeset(originalModule, newModule, t => t.IsInterface, "Interfaces"),
                        GetTypesChangeset(originalModule, newModule, t => t.IsValueType, "ValueType")
                    }
                };

                return template(viewModel);
            }
        }

        private static ChangeSet<TypeDefinition> GetTypesChangeset(ModuleDefinition originalModule, ModuleDefinition newModule, Func<TypeDefinition, bool> typePredicate, string name)
        {
            var classesChangesetComputer =
                new ChangeSetComputer<TypeDefinition>(new TypeDefinitionEqualityComparer());

            return classesChangesetComputer.GetChangeSet(name,
                originalModule.Types.Where(typePredicate).ToArray(),
                newModule.Types.Where(typePredicate).ToArray());
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
