﻿using System;
using System.IO;
using System.Reflection;
using DotLiquid;

namespace PeDiff.ClrComparator
{
    public class ClrFileComparator : IFileComparator
    {
        private readonly Template _template;

        static ClrFileComparator()
        {
            DotLiquidHelpers.RegisterViewModel(typeof(ClrViewModel));
            DotLiquidHelpers.RegisterViewModel(typeof(Assembly));
        }

        public ClrFileComparator()
        {
            _template = Template.Parse(new StreamReader("res/ClrTemplate.html").ReadToEnd());
        }

        public void CompareFiles(string originalFileName, string newFileName, string resultFileName)
        {
            var result = CompareFiles(originalFileName, newFileName);
            File.WriteAllText(resultFileName, result);
        }

        public string CompareFiles(string originalFileName, string newFileName)
        {
            var originalAssembly = Assembly.LoadFile(originalFileName);
            var newAssembly = Assembly.LoadFile(newFileName);
            var classesChangesetComputer = new ChangeSetComputer<Type>(new TypeNameEqualityComparer());

            var viewModel = new ClrViewModel
            {
                OriginalAssembly = originalAssembly,
                NewAssembly = newAssembly,
                ClassesChangeset = classesChangesetComputer.GetChangeSet(originalAssembly.ExportedTypes, newAssembly.ExportedTypes)
            };

            return _template.Render(Hash.FromAnonymousObject(viewModel));
        }
    }
}
