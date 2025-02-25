using Gameplay;
using R3;
using System.Collections.Generic;
using Utils;

namespace GameplayServices
{
    public class CardPlacingService
    {
        private List<Slot> _slots = new();
        private bool _canPlaceCard;

        private CardMatchingService _matchingService;

        private Subject<Card> _cardPlacedSub = new();
        public Observable<Card> OnCardPlaced => _cardPlacedSub;

        public CardPlacingService(List<Slot> slots, CardMatchingService matchingService)
        {
            _slots = slots;
            _canPlaceCard = true;
            _matchingService = matchingService;

            _matchingService.OnCardsRemoved.Subscribe(_ => ShiftCards());
        }

        public void PlaceCard(Card card)
        {
            if (!_canPlaceCard) return;

            foreach (var slot in _slots)
            {
                if (!slot.HasCard)
                {
                    slot.PlaceCard(card).Subscribe(_ =>
                    {
                        _matchingService.Match();
                    });
                    _cardPlacedSub.OnNext(card);
                    break;
                }
            }
        }

        private void ShiftCards()
        {
            _canPlaceCard = false;

            Coroutines.Invoke(() =>
            {
                foreach (var slot in _slots)
                {
                    if (!slot.HasCard) continue;

                    Card card = slot.Card;
                    slot.Release();

                    foreach (var newSlot in _slots)
                    {
                        if (newSlot.HasCard) continue;

                        newSlot.PlaceCard(card);
                        break;
                    }
                }

                _canPlaceCard = true;
            }, 0.5f);
        }
    }
}
