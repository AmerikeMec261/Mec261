using UnityEngine;

public class FireDirector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera _targetCamera;
    [SerializeField] private Turret _turret;

    [Header("Settings")]
    [SerializeField] private LayerMask _terrainLayer;

    private void Update()
    {
        Ray mouseRay = _targetCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, _terrainLayer))
        {
            _turret.AimAtTarget(hitInfo.point);
        }
    }
}