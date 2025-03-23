using UnityEngine;

namespace CameraUtils
{
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] private Vector2 _horizontalResolution = new Vector2(1920, 1080);
        [SerializeField] private Vector2 _verticalResolution = new Vector2(1080, 1920);

        [SerializeField] private float _horizontalCameraSize = 8.5f;
        [SerializeField] private float _verticalCameraSize = 10f;

        [SerializeField] private float _horizontalCameraPosition = -0.75f;
        [SerializeField] private float _verticalCameraPosition = 0;

        private Camera _camera;

        void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            UpdateCameraSize();
        }

        void UpdateCameraSize()
        {
            float aspectRatio = Screen.width / Screen.height;

            float horizontalAspect = _horizontalResolution.x / _horizontalResolution.y;
            float verticalAspect = _verticalResolution.x / _verticalResolution.y;

            float t = Mathf.InverseLerp(verticalAspect, horizontalAspect, aspectRatio);
            float newPositionY = Mathf.Lerp(_verticalCameraPosition, _horizontalCameraPosition, t);

            _camera.orthographicSize = Mathf.Lerp(_verticalCameraSize, _horizontalCameraSize, t);
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }
}
