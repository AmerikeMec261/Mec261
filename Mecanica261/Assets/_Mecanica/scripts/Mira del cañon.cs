using UnityEngine;

public class Miradelcañon : MonoBehaviour
{
    [SerializeField] private LayerMask _FloorLayer;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, _FloorLayer))
        {
            transform.position = hit.point + new Vector3(0, 0.01f, 0);
        }
    }
}
