using UnityEngine;

namespace Gameplay
{
    public class SlotView : MonoBehaviour
    {
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
    }
}
