using CameraUtils;
using Gameplay;
using GameplayRoot;
using GameRoot;
using R3;

namespace GameplayServices
{
    public class LevelRestarterService
    {
        private GameplayEnterParams _gameplayEnterParams;
        private FieldService _fieldService;
        private ShakyCamera _shakyCamera;

        public LevelRestarterService(GameplayEnterParams gameplayEnterParams, FieldService fieldService, ShakyCamera shakyCamera)
        {
            _gameplayEnterParams = gameplayEnterParams;
            _fieldService = fieldService;
            _shakyCamera = shakyCamera;
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
                var sceneLoader = new SceneLoader();
                sceneLoader.LoadAndRunGameplay(_gameplayEnterParams);
            });

            return onCompleted;
        }
    }
}
