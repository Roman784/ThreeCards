using Gameplay;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class SlotFactory : PlaceholderFactory<Slot>
    {
        public Slot Create(Vector2 position)
        {
            Slot newSlot = base.Create();
            newSlot.transform.position = position;

            return newSlot;
        }
    }
}
