using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWhirlpool : MonoBehaviour
{
    [SerializeField] private Transform _rarget;
    [SerializeField] private float _radiusX;
    [SerializeField] private float _radiusY;
    [SerializeField] private float _speed;
    [SerializeField] private float _offset;

    private float _angle;

    private void Update()
    {
        _angle += _speed * Time.deltaTime;

        float x = Mathf.Sin(_angle) * _radiusX;
        float y = Mathf.Cos(_angle) * _radiusY;

        float radOffset = _offset * Mathf.Deg2Rad;
        float rotatedX = x * Mathf.Cos(radOffset) - y * Mathf.Sin(radOffset);
        float rotatedY = x * Mathf.Sin(radOffset) + y * Mathf.Cos(radOffset);

        _rarget.position = new Vector2(rotatedX, rotatedY);
    }
}
