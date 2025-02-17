using GameState;
using TMPro;
using UnityEngine;
using Zenject;
using R3;

namespace Currencies
{
    public class ChipsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterView;

        private ReactiveProperty<int> _chipsCount = new();
        private IGameStateProvider _gameStateProvider;

        [Inject]
        public void Construct(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
            Debug.Log("inject");
            Debug.Log(_gameStateProvider);
        }

        public void Init()
        {
            /*_chipsCount.Subscribe(value => UpdateView());
            Debug.Log(_gameStateProvider);
            Debug.Log(_gameStateProvider.GameState);
            Debug.Log(_gameStateProvider.GameState.Chips);
            Debug.Log(_gameStateProvider.GameState.Chips.Value);
            _chipsCount.Value = _gameStateProvider.GameState.Chips.Value;*/
        }

        public void Add(int value)
        {
            _chipsCount.Value += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount.Value;
        }

        private void UpdateView()
        {
            _counterView.text = _chipsCount.Value.ToString();
        }
    }
}
