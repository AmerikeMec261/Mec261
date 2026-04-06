using UnityEngine;

public class SecondCameraController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera _secondCamera;

    [Header("Settings")]
    [SerializeField] private float _zoomStep = 5f;

    private bool _isLocked = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _secondCamera.gameObject.SetActive(!_secondCamera.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _isLocked = !_isLocked;
        }

        if (!_secondCamera.gameObject.activeSelf) { return; }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { ZoomIn(); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { ZoomOut(); }
    }

    public void ZoomIn()
    {
        _secondCamera.orthographicSize = Mathf.Max(1f, _secondCamera.orthographicSize - _zoomStep);
    }

    public void ZoomOut()
    {
        _secondCamera.orthographicSize += _zoomStep;
    }

    public void UpdateCameraPosition(Vector3 targetPosition)
    {
        if (_isLocked) { return; }

        Vector3 newCameraPosition = new Vector3(targetPosition.x, _secondCamera.transform.position.y, targetPosition.z);
        _secondCamera.transform.position = newCameraPosition;
    }
}