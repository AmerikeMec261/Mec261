using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DragAim : MonoBehaviour
{
    public Cannonball shooter;
    public Transform firePoint;

    LineRenderer line;
    Camera cam;
    bool dragging;

    void Start()
    {
        cam = Camera.main;
        line=GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) dragging=true;
        if (Input.GetMouseButtonUp(0)) Shoot();

        if (!dragging) return;

        line.enabled = true;
        Ray ray=cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);

        Vector3 dir=(hit.point - firePoint.position).normalized;
        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, firePoint.position);
        
        shooter.SetDirection(dir);
    }

    void Shoot()
    {
        if (!dragging) return;
        dragging = false;
        line.enabled = false;
        shooter.Shoot();
        shooter.PrepareToShoot();
    }
}
