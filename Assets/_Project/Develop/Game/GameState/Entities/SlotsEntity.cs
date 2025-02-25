using System;
using System.Collections.Generic;

namespace GameState
{
    [Serializable]
    public class SlotsEntity : Entity
    {
        public List<SlotEntity> Slots;
    }
}
