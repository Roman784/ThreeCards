using ScriptAnimations;
using System;
using UnityEngine;

namespace UI
{
    public class GameplayToolsView : MonoBehaviour
    {
        [SerializeField] private FadeImage _shuffleIconView;
        [SerializeField] private FadeImage _magicStickView;

        public event Action OnShuffleField;
        public event Action OnPickThree;

        public void ShuffleField() => OnShuffleField?.Invoke();
        public void PickThree() => OnPickThree?.Invoke();

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
