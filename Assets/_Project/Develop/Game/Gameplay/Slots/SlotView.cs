using UnityEngine;

namespace Gameplay
{
    public class SlotView : MonoBehaviour
    {
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
