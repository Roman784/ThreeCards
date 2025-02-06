using Gameplay;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class SlotFactory : PlaceholderFactory<SlotView>
    {
        public new Slot Create()
        {
            SlotView view = base.Create();
            Slot slot = new Slot(view);

            return slot;
        }

        public Slot Create(Vector2 position)
        {
            Slot slot = Create();
            slot.View.transform.position = position;

            return slot;
        }
    }
}
