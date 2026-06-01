using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Minecraft
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField, Required] private GameObject _prefab;
        [SerializeField] private Transform _spawnRoot;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private int _maxActiveSpawns = 5;

        [Header("Ground")]
        [SerializeField] private float _groundCheckUpDistance = 10f;
        [SerializeField] private float _groundCheckDownDistance = 25f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField, Tag] private string _groundTag = "Ground";
        [SerializeField] private int _spawnPointAttempts = 10;

        private readonly List<GameObject> _activeSpawns = new List<GameObject>();
        private float _nextSpawnTime;

        private void Awake()
        {
            if (_groundLayer.value == 0) { _groundLayer = LayerMask.GetMask(_groundTag); }
        }

        private void Update()
        {
            CleanDestroyedSpawns();

            if (Time.time < _nextSpawnTime) { return; }

            _nextSpawnTime = Time.time + _spawnInterval;

            if (_activeSpawns.Count >= _maxActiveSpawns) { return; }

            TrySpawn();
        }

        private void TrySpawn()
        {
            if (_prefab == null) { return; }

            for (int i = 0; i < _spawnPointAttempts; i++)
            {
                if (TryGetSpawnPosition(out Vector3 spawnPosition))
                {
                    GameObject spawnedObject = Instantiate(_prefab, spawnPosition, Quaternion.identity, _spawnRoot);
                    _activeSpawns.Add(spawnedObject);
                    return;
                }
            }
        }

        private bool TryGetSpawnPosition(out Vector3 spawnPosition)
        {
            Vector2 randomPoint = Random.insideUnitCircle * _spawnRadius;
            Vector3 rayOrigin = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit downHit, _groundCheckDownDistance) && IsGround(downHit.collider))
            {
                spawnPosition = downHit.point;
                return true;
            }

            if (Physics.Raycast(rayOrigin, Vector3.up, out RaycastHit upHit, _groundCheckUpDistance) && IsGround(upHit.collider))
            {
                spawnPosition = upHit.point;
                return true;
            }

            spawnPosition = transform.position;
            return false;
        }

        private void CleanDestroyedSpawns()
        {
            for (int i = _activeSpawns.Count - 1; i >= 0; i--)
            {
                if (_activeSpawns[i] == null)
                {
                    _activeSpawns.RemoveAt(i);
                }
            }
        }

        private bool IsGround(Collider collider)
        {
            bool isOnGroundLayer = (_groundLayer.value & (1 << collider.gameObject.layer)) != 0;
            bool hasGroundTag = collider.CompareTag(_groundTag);

            return isOnGroundLayer || hasGroundTag;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);

            Vector3 upRayEnd = transform.position + Vector3.up * _groundCheckUpDistance;
            Vector3 downRayEnd = transform.position + Vector3.down * _groundCheckDownDistance;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, upRayEnd);
            Gizmos.DrawWireSphere(upRayEnd, 0.25f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, downRayEnd);
            Gizmos.DrawWireSphere(downRayEnd, 0.25f);
        }
    }
}
