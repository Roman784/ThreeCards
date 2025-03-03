using System;

namespace Gameplay
{
    public class BonusSlotView : SlotView
    {
        public event Action OnCreate;

        public void Create()
        {
            OnCreate?.Invoke();
        }
    }
}
