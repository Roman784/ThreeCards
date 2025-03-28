using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using R3;

namespace BonusWhirlpool
{
    public class WhirlpoolCard
    {
        private Card _card;

        private Vector2 _radius;
        private float _flightSpeed;
        private float _trajectoryAngleOffset;
        private float _rotationSpeed;
        private Vector2 _positionOffset;

        private float _angle;

        public event Action<WhirlpoolCard> OnCardPlaced;

        public Card Card => _card;

        public WhirlpoolCard(Card card, Vector2 radius, float flightSpeed, float trajectoryAngleOffset, 
                             float rotationSpeed, Vector2 positionOffset)
        {
            _card = card;
            _radius = radius;
            _flightSpeed = flightSpeed;
            _trajectoryAngleOffset = trajectoryAngleOffset;
            _rotationSpeed = rotationSpeed;
            _positionOffset = positionOffset;

            _angle = UnityEngine.Random.Range(0, 360);

            card.OnCardPlaced.Subscribe(_ => OnCardPlaced?.Invoke(this));

            _card.PutDown();
        }

        public void Move()
        {
            _angle += _flightSpeed * Time.deltaTime;

            var x = Mathf.Sin(_angle) * _radius.x;
            var y = Mathf.Cos(_angle) * _radius.y;

            var radOffset = _trajectoryAngleOffset * Mathf.Deg2Rad;
            var rotatedX = x * Mathf.Cos(radOffset) - y * Mathf.Sin(radOffset);
            var rotatedY = x * Mathf.Sin(radOffset) + y * Mathf.Cos(radOffset);

            var position = new Vector2(rotatedX, rotatedY) + _positionOffset;
            _card.SetPosition(position);
            _card.Rotate(-Vector3.forward * _rotationSpeed);
        }
    }
}
