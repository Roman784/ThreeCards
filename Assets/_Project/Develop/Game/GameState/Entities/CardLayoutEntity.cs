using System;
using System.Collections.Generic;

namespace GameState
{
    [Serializable]
    public class CardLayoutEntity : Entity
    {
        public List<CardEntity> Cards;
    }
}
