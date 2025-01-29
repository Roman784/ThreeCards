using Gameplay;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class CardFactory : PlaceholderFactory<Card>
    {
        public Card Create(Vector2 position)
        {
            Card newCard =  base.Create();
            newCard.transform.position = position;

            return newCard;
        }
    }
}
