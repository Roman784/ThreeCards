using DG.Tweening;
using Gameplay;
using R3;
using System.Collections.Generic;
using Utils;
using static GameplayServices.CardMatchingService;

namespace GameplayServices
{
    public class CardPlacingService
    {
        private List<Slot> _slots = new();
        private bool _canPlaceCard;

        private CardMatchingService _matchingService;

        private Subject<Card> _cardPlacedSub = new();
        private Subject<Card> _cardReadyToPlacedSub = new();
        public Observable<Card> OnCardPlaced => _cardPlacedSub;
        public Observable<Card> OnCardReadyToPlaced => _cardReadyToPlacedSub;

        public CardPlacingService(List<Slot> slots)
        {
            _slots = slots;
            _canPlaceCard = true;
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
                        _cardPlacedSub.OnNext(card);
                    });
                    _cardReadyToPlacedSub.OnNext(card);
                    break;
                }
            }
        }

        public void ShiftCards()
        {
            _canPlaceCard = false;

            DOVirtual.DelayedCall(0.5f, () =>
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
            });
        }
    }
}
