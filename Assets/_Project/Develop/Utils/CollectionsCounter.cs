using System.Collections;
using UnityEngine;

namespace Utils
{
    public sealed class CollectionsCounter
    {
        public static int CountOfNonNullItems(IEnumerable collection)
        {
            int count = 0;
            foreach (var item in collection)
            {
                if (item != null)
                    count += 1;
            }

            return count;
        }
    }
}
