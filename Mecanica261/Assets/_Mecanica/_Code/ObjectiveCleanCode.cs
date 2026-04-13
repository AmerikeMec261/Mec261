using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectiveCleanCode : MonoBehaviour
{
    [Header("Configuración del movimiento")]
    public float _velocity = 5f;
    public float _Rightlimit = 10f;
    public float _Leftlimit = 10f;

    [Header("Sistema de Puntos")]
    public int _quantityPoints = 10;
    public Score _score;

    private bool _movingAuto = true;
    private bool _movingShoot = false;
    public Vector2 _direction;

    private void Start()
    {
        _score = FindFirstObjectByType < "Score" >;
        if( _score == null )
        {
            Debug.LogError("AutoEnemy: No se encontro el script Score en la escena", this);
        }
    }

    void Update()
    {
        if(_movingAuto)
        {
            Vector2 directionMov = (_direction == Vector2.right || _direction == Vector2.zero) ? Vector2.right : Vector2.left;
            transform.Translate(directionMov*_velocity*Time.deltaTime);

            if ((directionMov > 0 && transform.position.x >= _Rightlimit) || (directionMov < 0 && transform.position >= _Leftlimit))
            {
                DeactivateTarget();
            }
        }

       if(_movingShoot)
        {
            transform.Translate(_direction*_velocity*Time.deltaTime);

            if ((_direction > 0 && transform.position.x >= _Rightlimit) || (_direction < 0 && transform.position >= _Leftlimit))
            {
                DeactivateTarget();
            }

        }
    }

    public void StartAutoMovement(bool _right=true)
    {
        _movingAuto = true;
        _movingShoot=false;
        _direction = Vector2.zero;
    }

    public void ShootFrom(string side)
    {
        _movingAuto=false;
        _movingShoot = true;
        _direction = Vector2.zero;
    }

    private void DeactivateTarget()
    {
        _movingAuto = false;
        _movingShoot = false;
        gameObject.SetActive(false);
    }


}
