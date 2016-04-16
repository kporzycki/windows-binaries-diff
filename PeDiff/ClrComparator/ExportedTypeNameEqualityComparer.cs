using System.Collections.Generic;
using Mono.Cecil;

namespace PeDiff.ClrComparator
{
    public class ExportedTypeNameEqualityComparer : IEqualityComparer<ExportedType>
    {
        public bool Equals(ExportedType x, ExportedType y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(ExportedType obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}