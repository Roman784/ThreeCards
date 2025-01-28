using System;
using System.Collections.Generic;
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
                    if (CardLayoutSettings[i].Level == CardLayoutSettings[j].Level)
                    {
                        throw new ArgumentException($"The levels are repetitive: {CardLayoutSettings[i].Level}");
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
