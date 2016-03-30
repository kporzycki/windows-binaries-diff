using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeDiff
{
    interface IFileComparator
    {
        void CompareFiles(string originalFileName, string newFileName, string resultFileName);
    }
}
