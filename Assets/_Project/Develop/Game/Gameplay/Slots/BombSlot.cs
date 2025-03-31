using R3;

namespace Gameplay
{
    public class BombSlot : Slot
    {
        private BombSlotView _view;

        public BombSlot(BombSlotView view) : base(view)
        {
            _view = view;
        }
    }
}
