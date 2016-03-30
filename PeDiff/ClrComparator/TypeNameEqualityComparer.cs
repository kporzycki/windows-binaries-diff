using System;
using System.Collections.Generic;

namespace PeDiff.ClrComparator
{
    public class TypeNameEqualityComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Type obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}