using Gameplay;
using R3;
using System.Collections;
using UnityEngine;
using Utils;

namespace GameplayServices
{
    public class FieldAnimationService
    {
        private FieldService _fieldService;
        private CardFlippingService _cardFlippingService;

        public FieldAnimationService(FieldService fieldService, CardFlippingService cardFlippingService)
        {
            _fieldService = fieldService;
            _cardFlippingService = cardFlippingService;
        }

        public Observable<Unit> LayOutCards()
        {
            var completedSubj = new Subject<Unit>();
            Coroutines.StartRoutine(LayOutCardsRoutine(completedSubj));

            return completedSubj;
        }

        private IEnumerator LayOutCardsRoutine(Subject<Unit> completedSubj)
        {
            for (int cardI = 0; cardI < _fieldService.VerticalLength; cardI++)
            {
                for (int colunmI = 0; colunmI < _fieldService.HorizontalLength; colunmI++)
                {
                    Card card = _fieldService.GetCard(colunmI, cardI);
                    if (!_fieldService.IsCardExist(card)) continue;

                    card.PutDown();
                    card.Disable(false);

                    yield return new WaitForSeconds(0.035f);
                }
            }

            completedSubj.OnNext(Unit.Default);
            completedSubj.OnCompleted();

            yield return new WaitForSeconds(0.4f);
            _cardFlippingService.OpenFirstCards();
        }
    }
}
