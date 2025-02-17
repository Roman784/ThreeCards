using Currencies;
using UnityEngine;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private ChipsCounter _chipsCounter;

        public void Init()
        {
            _chipsCounter.Init();
        }
    }
}
