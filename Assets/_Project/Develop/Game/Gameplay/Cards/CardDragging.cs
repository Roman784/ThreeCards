using GameplayServices;
using UnityEngine;

namespace Gameplay
{
    public class CardDragging : MonoBehaviour
    {
        private Card _card;
        private SlotBar _slotBar;
        private CardPlacingService _placingService;

        private bool _canMove;
        private Vector2 _initialPosition;
        private Vector2 _touchOffset;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Init(Card card, SlotBar slotBar, CardPlacingService cardPlacingService)
        {
            _card = card;
            _slotBar = slotBar;
            _placingService = cardPlacingService;
        }

        private void OnMouseDown()
        {
            if (_card.IsClosed) return;

            _canMove = true;
            _initialPosition = _card.Position;

            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _touchOffset = _initialPosition - mousePosition;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!_canMove) return;

            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _card.SetPosition(mousePosition + _touchOffset);

            if (Vector2.Distance(mousePosition, _slotBar.BombSlot.Position) < 2f)
            {
                _card.CanDetonate = false;
                _placingService.PlaceBombCard(_card);
                _canMove = false;
                this.enabled = false;
            }
        }
    }
}
