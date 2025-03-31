using Gameplay;
using System;
using UI;
using UnityEngine;
using Zenject;

namespace GameplayServices
{
    public class CardFactory : PlaceholderFactory<CardView>
    {
        private GameplayTools _gameplayTools;
        private SlotBar _slotBar;

        [Inject]
        private void Construct(GameplayTools gameplayTools, SlotBar slotBar)
        {
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
        }

        public Card Create(bool isBombs, CardPlacingService placingService)
        {
            CardView view = base.Create();
            Card card = new Card(view, isBombs, placingService, _slotBar);

            card.SetGameplayTools(_gameplayTools);

            return card;
        }

        public Card Create(Vector2Int coordinates, bool isBombs, CardPlacingService placingService)
        {
            Card card = Create(isBombs, placingService);
            card.SetCoordinates(coordinates);

            return card;
        }

        public Card Create(Vector2 position, Vector2Int coordinates, bool isBombs, CardPlacingService placingService)
        {
            Card card = Create(coordinates, isBombs, placingService);
            card.SetPosition(position);

            return card;
        }
    }
}
