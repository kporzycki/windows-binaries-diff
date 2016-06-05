using System;
using System.Collections.Generic;
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
                    TypesChangeset = GetTypesChangeset(originalModule, newModule).ToArray()
                };

                return template(viewModel);
            }
        }

        private IEnumerable<ChangeSet<TypeDefinition>> GetTypesChangeset(ModuleDefinition originalModule, ModuleDefinition newModule)
        {
            if (Settings.CompareClasses)
                yield return GetTypesChangeset(originalModule, newModule, t => t.IsClass, "Classes");
            if (Settings.CompareEnums)
                yield return GetTypesChangeset(originalModule, newModule, t => t.IsEnum, "Enums");
            if (Settings.CompareInterfaces)
                yield return GetTypesChangeset(originalModule, newModule, t => t.IsInterface, "Interfaces");
            if (Settings.CompareValueTypes)
                yield return GetTypesChangeset(originalModule, newModule, t => t.IsValueType, "ValueType");
        }

        private static ChangeSet<TypeDefinition> GetTypesChangeset(ModuleDefinition originalModule, ModuleDefinition newModule, Func<TypeDefinition, bool> typePredicate, string name)
        {
            var classesChangesetComputer =
                new ChangeSetComputer<TypeDefinition>(new TypeDefinitionEqualityComparer());

            return classesChangesetComputer.GetChangeSet(name,
                originalModule.Types.Where(typePredicate).ToArray(),
                newModule.Types.Where(typePredicate).ToArray());
        }

        private ComparisonResult[] CompareMetadata(AssemblyDefinition originalAssembly, AssemblyDefinition newAssembly)
        {
            var originalEntryPoint = originalAssembly.EntryPoint != null ? originalAssembly.EntryPoint.Name : Settings.NoEntryPointError;
            var newEntryPoint = newAssembly.EntryPoint != null ? newAssembly.EntryPoint.Name : Settings.NoEntryPointError;
            return new[]
            {
                ComparisonResult.CompareValues("Entry point name", originalEntryPoint, newEntryPoint),
            };
        }

        public Settings Settings { get; set; }
    }
}
