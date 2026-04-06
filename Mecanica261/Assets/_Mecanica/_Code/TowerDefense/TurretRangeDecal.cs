using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TurretRangeDecal : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Turret _turret;
    [SerializeField] private DecalProjector _rangeDecal;

    private void Start()
    {
        UpdateRangeDecal();
    }

    public void UpdateRangeDecal()
    {
        if (_turret == null || _rangeDecal == null) { return; }

        float maxRange = _turret.GetMaxHighArcRange();
        float diameter = maxRange * 2f;

        _rangeDecal.size = new Vector3(diameter, diameter, _rangeDecal.size.z);
    }
}