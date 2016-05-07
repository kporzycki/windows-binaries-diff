using System.Collections.Generic;
using Mono.Cecil;

namespace PeDiff.ClrComparator
{
    internal class TypeDefinitionEqualityComparer : IEqualityComparer<TypeDefinition>
    {
        public bool Equals(TypeDefinition x, TypeDefinition y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(TypeDefinition obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}