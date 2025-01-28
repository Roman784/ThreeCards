using System;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CardLayoutSettings", menuName = "Game Settings/Cards/New Card Layout Settings")]
    public class CardLayoutSettings : ScriptableObject
    {
        [field: SerializeField] public int Level {  get; private set; }
        [field: SerializeField] public CardColumn[] CardColumns { get; private set; }

        public void SetLevel(int newLevel)
        {
            Level = newLevel;
        }

        [System.Serializable]
        public class CardColumn
        {
            public int CardCount;
        }

        private void OnValidate()
        {
            ValidateMultiplicityByThree();
        }

        private void ValidateMultiplicityByThree()
        {
            int count = 0;
            foreach (var column in CardColumns)
            {
                count += column.CardCount;
            }

            if (count % 3 != 0)
            {
                throw new ArgumentException($"The total number of cards is not a multiple of three.\nCurrent count: {count}");
            }
        }
    }
}
