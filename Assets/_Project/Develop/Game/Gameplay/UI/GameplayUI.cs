using Gameplay;
using UnityEngine;

namespace UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Transform _slotsContainer;

        public void AddSlot(Slot slot)
        {
            slot.transform.SetParent(_slotsContainer.transform, false);
        }
    }
}
