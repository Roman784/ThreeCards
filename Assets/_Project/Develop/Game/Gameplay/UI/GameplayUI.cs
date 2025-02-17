using Currencies;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private ChipsCounterView _chipsCounterView;
        private ChipsCounter _chipsCounter;

        [Inject]
        private void Construct(ChipsCounter chipsCounter)
        {
            _chipsCounter = chipsCounter;
        }

        public void BindViews()
        {
            _chipsCounter.BindView(_chipsCounterView);
        }
    }
}
