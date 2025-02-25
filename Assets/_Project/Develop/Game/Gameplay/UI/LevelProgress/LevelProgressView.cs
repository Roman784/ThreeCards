using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNumberView;

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumberView.text = levelNumber.ToString();
        }

        public void FillProgressBar(float progress)
        {
            
        }
    }
}
