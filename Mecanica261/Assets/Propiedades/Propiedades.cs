using NUnit.Framework.Constraints;
using UnityEngine;

public class Propiedades : MonoBehaviour
{
    [SerializeField]private float _health;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField]
    private int _damage = 0;
    [SerializeField]
    private float _volume = 0f;

    public Rigidbody  _rigidBody;

    public int Health { get; } //1
    public bool IsDead => Health <= 0;  //2
    public string PlayerName { get; init; }//3
    public int Coins { get; } //4
    public float HealthPercent => (float)_health / _maxHealth * 100f;//5
    private int _speed;//6
    public int Speed => _speed; //6
    public int Damage { get { return _damage; } set { _damage = Mathf.Clamp(value, 0, 100); } }  // 7
    public static int PlayerCount { get; private set; } // 8
    public int Experience { get; private set; } //9
    [SerializeField] private int _stamina;//10
    public int Stamina => _stamina;//10
    public bool IsNear => Distance <= 0;  //11
    public int Volume { get { return _volume; } set { _volume = Mathf.Clamp(value, 0, 100); } }//12
    public int CreationDate { get; } = 0;//13
    public bool FullInventory => Inventory <= 0;//14
    public int MaxLevel { get; init; }  //15
    public float HorizontalSpeed => new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z).magnitude;  //16
    public int Energy { get; private set; }//17
    public Vector3 CurrentPosition => transform.position; //18
    public ReadInventory <Item> => _inventory; //19
    public bool Running => _rigidBody.angularVelocity.magnitude >= _runSpeed; //20




}
