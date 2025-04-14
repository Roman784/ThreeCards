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
        private SlotBar _slotBar;
        private bool _canPlaceCard;

        private Subject<Card> _cardPlacedSub = new();
        private Subject<Card> _cardReadyToPlacedSub = new();
        public Observable<Card> OnCardPlaced => _cardPlacedSub;
        public Observable<Card> OnCardReadyToPlaced => _cardReadyToPlacedSub;

        public CardPlacingService(SlotBar slotBar)
        {
            _slotBar = slotBar;
            _canPlaceCard = true;
        }

        public void PlaceCard(Card card)
        {
            if (!_canPlaceCard) return;

            foreach (var slot in _slotBar.Slots)
            {
                if (TryPlaceCard(slot, card)) break;
            }
        }

        public void PlaceBombCard(Card card)
        {
            var slot = _slotBar.BombSlot;
            TryPlaceCard(slot, card);
        }

        public void ShiftCards()
        {
            _canPlaceCard = false;

            DOVirtual.DelayedCall(0.5f, () =>
            {
                foreach (var slot in _slotBar.Slots)
                {
                    if (!slot.HasCard) continue;

                    Card card = slot.Card;
                    slot.Release();

                    foreach (var newSlot in _slotBar.Slots)
                    {
                        if (newSlot.HasCard) continue;

                        newSlot.PlaceCard(card);
                        break;
                    }
                }

                _canPlaceCard = true;
            });
        }

        private bool TryPlaceCard(Slot slot, Card card)
        {
            if (slot.HasCard) return false;

            slot.PlaceCard(card).Subscribe(_ =>
            {
                _cardPlacedSub.OnNext(card);
            });
            _cardReadyToPlacedSub.OnNext(card);

            return true;
        }
    }
}
