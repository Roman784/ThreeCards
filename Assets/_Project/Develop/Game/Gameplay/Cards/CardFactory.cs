using Audio;
using Gameplay;
using Settings;
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
        private AudioPlayer _audioPlayer;
        private CardAudioSettings _audioSettings;

        [Inject]
        private void Construct(GameplayTools gameplayTools, SlotBar slotBar, 
                               AudioPlayer audioPlayer, ISettingsProvider settingsProvider)
        {
            _gameplayTools = gameplayTools;
            _slotBar = slotBar;
            _audioPlayer = audioPlayer;
            _audioSettings = settingsProvider.GameSettings.AudioSettings.CardAudioSettings;
        }

        public Card Create(bool isBombs, CardPlacingService placingService)
        {
            CardView view = base.Create();
            Card card = new Card(view, isBombs, placingService, _slotBar, _audioPlayer, _audioSettings);

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
