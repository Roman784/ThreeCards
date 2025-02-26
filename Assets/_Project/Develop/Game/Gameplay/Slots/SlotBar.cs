using GameplayServices;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class SlotBar : MonoBehaviour
    {
        [SerializeField] private float _spacing;

        private SlotsSettings _slotsSettings;
        private SlotFactory _slotFactory;

        private List<Slot> _slots = new();
        public IEnumerable<Slot> Slots => _slots;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, SlotFactory slotFactory)
        {
            _slotsSettings = settingsProvider.GameSettings.SlotsSettings;
            _slotFactory = slotFactory;
        }

        public bool ContainsCard(Card card)
        {
            foreach (var slot in _slots)
            {
                if (slot.Card == card)
                    return true;
            }
            return false;
        }

        public List<Slot> CreateSlots()
        {
            for (int i = 0; i < _slotsSettings.Count; i++)
            {
                CreateSlot();
            }

            ArrangeSlots();

            return _slots;
        }

        private void CreateSlot()
        {
            var newSlot = _slotFactory.Create();
            _slots.Add(newSlot);
        }

        private void ArrangeSlots()
        {
            var totalWidth = (_slots.Count - 1) * _spacing;
            var startX = -totalWidth / 2f;

            for (int i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var position = new Vector3(startX + _spacing * i, transform.position.y, transform.position.z);

                slot.SetParent(transform);
                slot.SetPosition(position);
            }
        }
    }
}
