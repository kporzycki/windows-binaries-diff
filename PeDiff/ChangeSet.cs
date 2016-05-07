using System.Collections.Generic;

namespace PeDiff
{
    public class ChangeSet<T>
    {
        public string Name { get; set; }
        public IEnumerable<T> Removed { get; set; }
        public IEnumerable<T> Added { get; set; }
        public IEnumerable<T> Same { get; set; }
    }
}