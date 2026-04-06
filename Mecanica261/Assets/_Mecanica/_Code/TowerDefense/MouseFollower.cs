using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RectTransform _targetScope;
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private SecondCameraController _cameraController;

    [Header("Settings")]
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private float _surfaceOffset = 0.1f;

    private void Update()
    {
        Ray mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _terrainLayer))
        {
            Vector3 targetPosition = hitInfo.point + hitInfo.normal * _surfaceOffset;
            Quaternion targetRotation = Quaternion.LookRotation(-hitInfo.normal, _targetCamera.transform.up);

            _targetScope.position = targetPosition;
            _targetScope.rotation = targetRotation;
        }

        _cameraController.UpdateCameraPosition(_targetScope.position);
    }
}