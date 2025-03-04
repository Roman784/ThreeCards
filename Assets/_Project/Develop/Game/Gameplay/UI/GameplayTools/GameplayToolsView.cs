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
        [SerializeField] private Transform _shuffleChipsIconView;

        [Space]

        [SerializeField] private FadeImage _magicStickIconView;
        [SerializeField] private TMP_Text _magicStickCostView;
        [SerializeField] private Transform _magicStickChipsIconView;

        [Space]

        [SerializeField] private FadeImage _restartLevelIconView;

        [Space]

        [SerializeField] private GameObject _byeEffectPrefab;

        private Camera _camera;

        public event Action OnShuffleField;
        public event Action OnPickThree;
        public event Action OnRestartLevel;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void ShuffleField() => OnShuffleField?.Invoke();
        public void PickThree() => OnPickThree?.Invoke();
        public void RestartLevel() => OnRestartLevel?.Invoke();

        public void CreateShufflingPurchaseEffect() => CreateByeEffect(_shuffleChipsIconView.position);
        public void CreateMagicStickPurchaseEffect() => CreateByeEffect(_magicStickChipsIconView.position);

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

        private void CreateByeEffect(Vector2 iconPosition)
        {
            Vector2 position = _camera.ScreenToWorldPoint(iconPosition);
            Instantiate(_byeEffectPrefab, position, Quaternion.identity);
        }
    }
}
