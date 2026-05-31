using UnityEngine;

public interface ILooker
{
    void SetLookTarget(Transform lookTarget);
    void SetLookPosition(Vector3 lookPosition);
    void ClearLookTarget();
}
