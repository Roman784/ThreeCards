using System;
using TMPro;
using UnityEngine;

namespace LevelMenu
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberView;

        public event Action OnOpenLevel;

        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetNumber(int number)
        {
            _numberView.text = number.ToString();
        }

        public void OpenLevel()
        {
            OnOpenLevel?.Invoke();
        }
    }
}
