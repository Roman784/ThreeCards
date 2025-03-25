using Gameplay;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BonusWhirlpool
{
    public class BonusWhirlpoolSlotBar : SlotBar
    {
        [Space]

        [SerializeField] private float _verticalSpacing;
        private int _countInRow = 4;

        private BonusWhirlpoolSlotsSettings _settings;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider)
        {
            _settings = settingsProvider.GameSettings.BonusWhirlpoolSlotsSettings;
            _countInRow = _settings.CountInRow;
        }

        public override List<Slot> CreateSlots()
        {
            for (int i = 0; i < _settings.Count; i++)
            {
                CreateSlot();
            }

            return _slots;
        }

        protected override void ArrangeSlots()
        {
            var slots = new List<Slot>(_slots);

            int rowCount = Mathf.CeilToInt((float)slots.Count / _countInRow);
            var totalWidth = (_countInRow - 1) * _spacing;
            var totalHeight = (rowCount - 1) * _verticalSpacing;

            var startX = -totalWidth / 2f;
            var startY = totalHeight / 2f;

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                int row = i / _countInRow;
                int col = i % _countInRow;

                var position = new Vector3(startX + _spacing * col, startY - _verticalSpacing * row, 0) 
                    + transform.position;

                slot.SetParent(transform);
                slot.SetPosition(position);
            }
        }
    }
}
