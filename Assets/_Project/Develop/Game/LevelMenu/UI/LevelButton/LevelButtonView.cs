using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelMenu
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberView;
        [SerializeField] private Image _backgroundView;

        public event Action OnOpenLevel;

        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetNumber(int number)
        {
            _numberView.text = number.ToString();
        }

        public void Fill(bool isPassed)
        {
            _backgroundView.gameObject.SetActive(isPassed);
        }

        public void OpenLevel()
        {
            OnOpenLevel?.Invoke();
        }
    }
}
