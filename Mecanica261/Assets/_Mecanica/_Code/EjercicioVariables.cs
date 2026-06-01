using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class EjercicioVariables : MonoBehaviour
{
    //Variable de velocidad
    [SerializeField] private float _movementSpeed = 5f;

    // Nivel actual del jugador
    public int PlayerLevel;

    //Daño base
    protected int _baseDamage = 10;

    //Vida actual del jugador
    private int _currentHealth;

    //Referencia a Rigidbody
    [SerializeField] private Rigidbody _rigidbody;

    //El jugador sigue vivo
    private bool _isAlive;

    //Indice de guardado
    internal int _saveIndex;

    //Rango de ataque
  [SerializeField] private int _attackRange;

    //Nombre actual del jugador
    public string PlayerName;

    //Velocidad de movimiento
    protected float _moveSpeed;

    //Referencia a MeshRenderer
    private MeshRenderer _meshRenderer;

    //Volumen del juego
    [Tooltip("Controla el volumen del juego")]
    [SerializeField] private float _masterVolume = 1f;

    //Puede atacar
    private bool _canAttack;

    //Instancia global del GameManager
    public static GameManager Instance;

    //Posicion del jugador
    private Vector3 position;

    //Jugadores permitidos
    public int MaxPlayers;

    //Distancia de deteccion enemiga
    [SerializeField] protected float _detectionDistance = 10f;

    //Referencia AudioSource
    [SerializeField] private AudioSource _audioSource;
}
