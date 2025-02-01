using UnityEngine;

namespace Gameplay
{
    public class Card : MonoBehaviour
    {
        public bool IsInited { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }

        public void Init(Suits suit, Ranks rank)
        {
            IsInited = true;
            Suit = suit;
            Rank = rank;
        }
    }
}
