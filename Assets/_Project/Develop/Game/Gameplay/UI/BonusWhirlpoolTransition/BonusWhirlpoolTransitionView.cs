using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BonusWhirlpoolTransitionView : MonoBehaviour
    {
        [SerializeField] private Image _progressView;

        public void SetProgress(float value)
        {
            _progressView.fillAmount = value;
        }
    }
}
