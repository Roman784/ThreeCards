using TMPro;
using UnityEngine;

namespace LevelMenu
{
    public class LevelButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberView;

        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetNumber(int number)
        {
            _numberView.text = number.ToString();
        }
    }
}
