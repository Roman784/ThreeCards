using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public sealed class Randomizer
    {
        public static bool TryProbability(float probability)
        {
            probability = Mathf.Clamp01(probability);

            return Random.Range(0f, 1f) <= probability;
        }

        public static T GetRandomValue<T>(params T[] values)
        {
            return GetRandomValue(values.ToList());
        }

        public static T GetRandomValue<T>(T[,] values)
        {
            var valuesList = new List<T>();

            foreach (var value in values)
            {
                if (value == null) continue;
                valuesList.Add(value);
            }

            return GetRandomValue(valuesList);
        }

        public static T GetRandomValue<T>(List<T> values)
        {
            if (values.Count == 0)
                throw new System.ArgumentException("The list of random values is empty.");

            int randomIndex = Random.Range(0, values.Count);
            return values[randomIndex];
        }
    }
}
