using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BonusWhirlpoolTransitionView : MonoBehaviour
    {
        [SerializeField] private Image _progressView;

        public event Action OnOpenPopUp;

        public void SetProgress(float value)
        {
            _progressView.fillAmount = value;
        }

        public void OpenPopUp()
        {
            OnOpenPopUp?.Invoke();
        }
    }
}
