using TMPro;
using UnityEngine;

namespace Utils
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueView;

        public void Render(float value)
        {
            var seconds = value;
            var minutes = (int)(seconds / 60f);
            var secs = (int)(seconds % 60f);
            var millisecs = (int)((seconds * 1000f) % 1000f);

            var formattedTime = string.Format("{0:00}:{1:00}:{2:00}",
                minutes,
                secs,
                millisecs / 10);

            _valueView.text = formattedTime;
        }
    }
}
