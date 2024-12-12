using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
  [SerializeField] private Transform _target;
  [SerializeField] private TilemapRenderer _tilemapRenderer;
  [SerializeField] private Camera  _mainCamera;
  private float _smoothTime = 0.25f;
  private Vector3 _velocity = Vector3.zero;
  private float _leftLimit;
  private float _rightLimit;
  private float _topLimit;
  private float _bottomLimit;
  private float _OffSetYValue = 1f;

  private void Start()
  {
    CalculateCameraLimit();
  }

  private void Update()
  {
    float clampedX = Mathf.Clamp(_target.transform.position.x, _leftLimit, _rightLimit);
    float clampedY = Mathf.Clamp(_target.transform.position.y, _bottomLimit, _topLimit);
    Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);

    transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref _velocity, _smoothTime);
  }

  private void CalculateCameraLimit()
  {
    Bounds bounds = _tilemapRenderer.bounds;
    float camHeight = _mainCamera.orthographicSize;
    float camWidth = camHeight * _mainCamera.aspect;
    _leftLimit = bounds.min.x + camWidth;
    _rightLimit = bounds.max.x - camWidth;
    _bottomLimit = (bounds.min.y + camHeight) + _OffSetYValue;
    _topLimit = (bounds.max.y - camHeight) - _OffSetYValue;
  }
}
