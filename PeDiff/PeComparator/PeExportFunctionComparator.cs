using System.Collections.Generic;
using PeNet;

namespace PeDiff
{
    class PeExportFunctionComparator : IEqualityComparer<PeFile.ExportFunction>
    {
        public bool Equals(PeFile.ExportFunction x, PeFile.ExportFunction y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(PeFile.ExportFunction obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}