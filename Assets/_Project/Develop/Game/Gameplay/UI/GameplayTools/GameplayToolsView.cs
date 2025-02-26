using ScriptAnimations;
using System;
using UnityEngine;

namespace UI
{
    public class GameplayToolsView : MonoBehaviour
    {
        [SerializeField] private FadeImage _shuffleIconView;

        public event Action OnShuffleField;

        public void Enable()
        {
            _shuffleIconView.FadeIn();
        }

        public void Disable()
        {
            _shuffleIconView.FadeOut();
        }

        public void ShuffleField()
        {
            OnShuffleField?.Invoke();
        }
    }
}
