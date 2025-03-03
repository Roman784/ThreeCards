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
        private Card[,] _cardsMap;
        private ShakyCamera _shakyCamera;

        public LevelRestarterService(GameplayEnterParams gameplayEnterParams, Card[,] cardsMap, ShakyCamera shakyCamera)
        {
            _gameplayEnterParams = gameplayEnterParams;
            _cardsMap = cardsMap;
            _shakyCamera = shakyCamera;
        }

        public Observable<Unit> Restart()
        {
            Observable<Unit> onCompleted = null;

            _shakyCamera.Shake();
            foreach (var card in _cardsMap)
            {
                if (card != null && !card.IsDestroyed)
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
