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

        [SerializeField] private BonusSlotView _bonusSlotView;
        private Slot _bonusSlot;

        private SlotsSettings _slotsSettings;
        private SlotFactory _slotFactory;

        private List<Slot> _slots = new();
        public IEnumerable<Slot> Slots => _slots;
        public BonusSlotView BonusSlotView => _bonusSlotView;
        public bool IsBonusSlotCreated => _bonusSlot.IsDestroyed;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, SlotFactory slotFactory)
        {
            _slotsSettings = settingsProvider.GameSettings.SlotsSettings;
            _slotFactory = slotFactory;

            _bonusSlot = _slotFactory.Create(_bonusSlotView);
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

            return _slots;
        }

        public void CreateBonusSlot()
        {
            if (IsBonusSlotCreated) return;

            _bonusSlot.Destroy();
            CreateSlot();
        }

        public bool HasEmptySlot()
        {
            foreach (var slot in _slots)
            {
                if (!slot.HasCard)
                    return true;
            }

            return false;
        }

        public bool HasAnyCard()
        {
            foreach (var slot in _slots)
            {
                if (slot.HasCard)
                    return true;
            }

            return false;
        }

        private void CreateSlot()
        {
            var newSlot = _slotFactory.Create();
            _slots.Add(newSlot);

            ArrangeSlots();
        }

        private void ArrangeSlots()
        {
            var slots = new List<Slot>(_slots);

            if (!_bonusSlot.IsDestroyed)
                slots.Add(_bonusSlot);

            var totalWidth = (slots.Count - 1) * _spacing;
            var startX = -totalWidth / 2f;

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var position = new Vector3(startX + _spacing * i, transform.position.y, transform.position.z);

                slot.SetParent(transform);
                slot.SetPosition(position);
            }
        }
    }
}
