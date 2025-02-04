using Gameplay;
using Settings;
using UI;
using Zenject;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private SlotsSettings _slotsSettings;
        private SlotFactory _slotFactory;
        private GameplayUI _ui;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, SlotFactory slotFactory)
        {
            _slotsSettings = settingsProvider.GameSettings.SlotsSettings;
            _slotFactory = slotFactory;
        }

        public void CreateSlots(GameplayUI ui)
        {
            _ui = ui;

            for (int i = 0; i < _slotsSettings.Count; i++)
            {
                CreateSlot();
            }
        }

        private void CreateSlot()
        {
            Slot newSlot = _slotFactory.Create();
            _ui.AddSlot(newSlot);
        }
    }
}
