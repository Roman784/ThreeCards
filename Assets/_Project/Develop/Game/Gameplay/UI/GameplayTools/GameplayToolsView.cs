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

        [SerializeField] private FadeImage _magicStickView;
        [SerializeField] private TMP_Text _magicStickCostView;

        public event Action OnShuffleField;
        public event Action OnPickThree;

        public void ShuffleField() => OnShuffleField?.Invoke();
        public void PickThree() => OnPickThree?.Invoke();

        public void SetCosts(int shuffleCost, int magicStickCost)
        {
            _shuffleCostView.text = shuffleCost.ToString();
            _magicStickCostView.text = magicStickCost.ToString();
        }

        public void Enable()
        {
            _shuffleIconView.FadeIn();
            _magicStickView.FadeIn();
        }

        public void Disable()
        {
            _shuffleIconView.FadeOut();
            _magicStickView.FadeOut();
        }
    }
}
