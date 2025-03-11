using UnityEngine;

namespace LevelMenu
{
    public class LevelsBlockView : MonoBehaviour
    {
        public void Attach(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}
