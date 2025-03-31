using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CardLayoutsSettings", menuName = "Game Settings/Cards/New Card Layouts Settings")]
    public class CardLayoutsSettings : ScriptableObject
    {
        [field: SerializeField] public List<CardLayoutSettings> CardLayoutSettings {  get; private set; }

        [field: SerializeField] public int MinColumnCount { get; private set; }
        [field: SerializeField] public int MaxColumnCount { get; private set; }
        [field: SerializeField] public float MinColumnSpacing { get; private set; }
        [field: SerializeField] public float MaxColumnSpacing { get; private set; }
        [field: SerializeField] public float StepBetweenCards { get; private set; }
        [field: SerializeField] public Vector2 ColumnsOffset { get; private set; }

        public int LayoutsCount => CardLayoutSettings.Count;

        public CardLayoutSettings GetLayout(int levelNumber)
        {
            foreach (var layout in CardLayoutSettings)
            {
                if (layout.LevelNumber == levelNumber)
                {
                    return layout;
                }
            }

            throw new ArgumentNullException($"The card layout with level number {levelNumber} are not exist.");
        }

        public bool IsLevelExist(int levelNumber)
        {
            try
            {
                GetLayout(levelNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int ClampLevelNumber(int levelNumber)
        {
            return Math.Clamp(levelNumber, 1, GetMaxLevelNumber());
        }

        public float CalculateColumnSpacing(int columnCount)
        {
            float delta = (MaxColumnSpacing - MinColumnSpacing) / (MaxColumnCount - MinColumnCount);
            float spacing = MinColumnSpacing + delta * (MaxColumnCount - columnCount);
            return Mathf.Clamp(spacing, MinColumnSpacing, MaxColumnSpacing);
        }

        private int GetMaxLevelNumber()
        {
            return CardLayoutSettings.Max(l => l.LevelNumber);
        }

        private void OnValidate()
        {
            ValidateCardLayoutsLevels();
        }

        private void ValidateCardLayoutsLevels()
        {
            for (int i = 0; i < CardLayoutSettings.Count; i++)
            {
                for(int j = i+1; j < CardLayoutSettings.Count; j++)
                {
                    if (CardLayoutSettings[i].LevelNumber == CardLayoutSettings[j].LevelNumber)
                    {
                        throw new ArgumentException($"The levels are repetitive: {CardLayoutSettings[i].LevelNumber}");
                    }
                }
            }
        }

        [ContextMenu("Set Level Quickly")]
        // Sets levels according to their index in the list.
        private void SetLevelsQuickly()
        {
            for (int i = 0; i < CardLayoutSettings.Count; i++)
            {
                CardLayoutSettings[i].SetLevel(i + 1);
            }
        }
    }
}
