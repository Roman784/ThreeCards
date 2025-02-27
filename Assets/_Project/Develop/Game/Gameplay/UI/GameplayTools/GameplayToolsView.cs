using ScriptAnimations;
using Settings;
using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameplayToolsView : MonoBehaviour
    {
        [SerializeField] private FadeImage _shuffleIconView;
        [SerializeField] private TMP_Text _shuffleCostView;

        [SerializeField] private FadeImage _magicStickIconView;
        [SerializeField] private TMP_Text _magicStickCostView;

        [SerializeField] private FadeImage _restartLevelIconView;

        public event Action OnShuffleField;
        public event Action OnPickThree;
        public event Action OnRestartLevel;

        public void ShuffleField() => OnShuffleField?.Invoke();
        public void PickThree() => OnPickThree?.Invoke();
        public void RestartLevel() => OnRestartLevel?.Invoke();

        public void SetCosts(int shuffleCost, int magicStickCost)
        {
            _shuffleCostView.text = shuffleCost.ToString();
            _magicStickCostView.text = magicStickCost.ToString();
        }

        public void Enable()
        {
            _shuffleIconView.FadeIn();
            _magicStickIconView.FadeIn();
            _restartLevelIconView.FadeIn();
        }

        public void Disable()
        {
            _shuffleIconView.FadeOut();
            _magicStickIconView.FadeOut();
            _restartLevelIconView.FadeOut();
        }
    }
}
