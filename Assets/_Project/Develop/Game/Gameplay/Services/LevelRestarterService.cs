using CameraUtils;
using Gameplay;
using GameplayRoot;
using GameRoot;
using R3;
using UI;

namespace GameplayServices
{
    public class LevelRestarterService
    {
        private FieldService _fieldService;
        private ShakyCamera _shakyCamera;
        private int _currentlevelNumber;
        private BonusWhirlpoolTransition _bonusWhirlpoolTransition;

        public LevelRestarterService(FieldService fieldService, ShakyCamera shakyCamera,
                                     int currentLevelNumber, BonusWhirlpoolTransition bonusWhirlpoolTransition)
        {
            _fieldService = fieldService;
            _shakyCamera = shakyCamera;
            _currentlevelNumber = currentLevelNumber;
            _bonusWhirlpoolTransition = bonusWhirlpoolTransition;
        }

        public Observable<Unit> Restart()
        {
            Observable<Unit> onCompleted = null;

            _shakyCamera.Shake();
            foreach (var card in _fieldService.Cards)
            {
                if (_fieldService.IsCardExist(card))
                    onCompleted = card.Explode();
            }

            (onCompleted ?? Observable.Return(Unit.Default)).Subscribe(_ =>
            {
                var enterParams = new GameplayEnterParams(_currentlevelNumber,
                                                          _bonusWhirlpoolTransition.CurrentTimerValue);
                new SceneLoader().LoadAndRunGameplay(enterParams);
            });

            return onCompleted;
        }
    }
}
