using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Dependencies")]
    [SerializeField] private RectTransform _healthBarContainer;
    [SerializeField] private Slider _healthBar;

    [Header("Settings")]
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private float _waypointReachDistance = 0.1f;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private int _currentWaypointIndex;

    private void FixedUpdate()
    {
        MoveToWayPoint();       
    }

    private void LateUpdate()
    {
        BillboardRotation();
    }

    private void BillboardRotation()
    {
        Vector3 lookPosition = Camera.main.transform.position;
        lookPosition.y = _healthBarContainer.position.y;

        _healthBarContainer.LookAt(lookPosition);
        _healthBarContainer.Rotate(0f, 180f, 0f);
    }

    private void MoveToWayPoint()
    {
        if (_waypoints.Count == 0) { return; }

        Transform currentWaypoint = _waypoints[_currentWaypointIndex];
        Vector3 targetPosition = currentWaypoint.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        Vector3 direction = targetPosition - transform.position;
        if (direction != Vector3.zero) { transform.forward = direction.normalized; }

        float distanceToWaypoint = Vector3.Distance(transform.position, targetPosition);
        if (distanceToWaypoint <= _waypointReachDistance) { _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count; }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UpdateHealthBar();

        if (_currentHealth <= 0) { Death(); }
    }

    public void UpdateHealthBar()
    {
        _healthBar.value = _currentHealth / _maxHealth;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}