using ScriptAnimations;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BonusWhirlpoolTransitionView : MonoBehaviour
    {
        [SerializeField] private Image _progressView;
        [SerializeField] private PulsationAnimator _pulsation;

        public event Action OnOpenPopUp;

        public void SetProgress(float value)
        {
            _progressView.fillAmount = value;
        }

        public void OpenPopUp()
        {
            OnOpenPopUp?.Invoke();
        }

        public void StartPulse()
        {
            _pulsation.Pulse(gameObject.transform, int.MaxValue);
        }
    }
}
