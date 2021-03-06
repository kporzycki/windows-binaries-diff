﻿using System.Collections.Generic;
using System.Linq;

namespace PeDiff
{
    class ChangeSetComputer<T>
    {
        private readonly IEqualityComparer<T> _comparer;

        public ChangeSetComputer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public ChangeSet<T> GetChangeSet(string name, IEnumerable<T> originalCollection, IEnumerable<T> newCollection)
        {
            var originalArray = originalCollection as T[] ?? originalCollection.ToArray();
            var newArray = newCollection as T[] ?? newCollection.ToArray();
            return new ChangeSet<T>
            {
                Name = name,
                Removed = originalArray.Except(newArray, _comparer).ToList(),
                Added = newArray.Except(originalArray, _comparer).ToList(),
                Same = originalArray.Intersect(newArray, _comparer).ToList()
            };
        }
    }
}
