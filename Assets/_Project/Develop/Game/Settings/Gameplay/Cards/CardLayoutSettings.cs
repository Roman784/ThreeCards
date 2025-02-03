using System;
using System.Linq;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CardLayoutSettings", menuName = "Game Settings/Cards/New Card Layout Settings")]
    public class CardLayoutSettings : ScriptableObject
    {
        [System.Serializable]
        public class CardColumn
        {
            public int CardCount;
        }

        [field: SerializeField] public int LevelNumber {  get; private set; }
        [field: SerializeField] public Vector2Int CardSpreadRange { get; private set; }
        [field: SerializeField] public CardColumn[] CardColumns { get; private set; }

        public int ColumnCount => CardColumns.Length;

        public void SetLevel(int newLevelNumber)
        {
            LevelNumber = newLevelNumber;
        }

        public int GetMaxColumnLength()
        {
            int maxLength = 0;
            foreach (CardColumn cardColumn in CardColumns)
            {
                if (cardColumn.CardCount > maxLength)
                {
                    maxLength = cardColumn.CardCount;
                }
            }

            return maxLength;
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
