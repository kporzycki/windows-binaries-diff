using System;
using System.Linq;

namespace PeDiff
{
    class DotLiquidHelpers
    {
        public static void RegisterViewModel(Type rootType)
        {
            rootType
                .Assembly
                .GetTypes()
                .Where(t => t.Namespace == rootType.Namespace)
                .ToList()
                .ForEach(RegisterSafeTypeWithAllProperties);
        }

        public static void RegisterSafeTypeWithAllProperties(Type type)
        {
            //Template.RegisterSafeType(type,
            //    type
            //        .GetProperties()
            //        .Select(p => p.Name)
            //        .ToArray());
        }
    }
}