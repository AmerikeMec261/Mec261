using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Minecraft
{
    public class Spawner : MonoBehaviour
    {
        [Header("Spawn")]
        [FormerlySerializedAs("_prefab")]
        [Tooltip("Prefab created by this spawner.")]
        [SerializeField, Required] private GameObject _spawnPrefab;
        [Tooltip("Optional parent assigned to spawned objects.")]
        [SerializeField] private Transform _spawnRoot;
        [Tooltip("Horizontal radius where objects can spawn.")]
        [SerializeField] private float _spawnRadius = 10f;
        [Tooltip("Seconds between spawn attempts.")]
        [SerializeField] private float _spawnInterval = 3f;
        [Tooltip("Maximum number of active spawned objects.")]
        [SerializeField] private int _maxActiveSpawns = 5;
        [FormerlySerializedAs("_prewarmEnemies")]
        [Tooltip("Number of objects spawned when the scene starts.")]
        [SerializeField] private int _prewarmSpawnCount;

        [Header("Ground")]
        [Tooltip("Distance checked upward when looking for ground.")]
        [SerializeField] private float _groundCheckUpDistance = 10f;
        [Tooltip("Distance checked downward when looking for ground.")]
        [SerializeField] private float _groundCheckDownDistance = 25f;
        [Tooltip("Layers accepted as spawn ground.")]
        [SerializeField] private LayerMask _groundLayer;
        [Tooltip("Tag accepted as spawn ground.")]
        [SerializeField, Tag] private string _groundTag = "Ground";
        [Tooltip("Random positions tested per spawn attempt.")]
        [SerializeField] private int _spawnPointAttempts = 10;

        private readonly List<GameObject> _activeSpawns = new List<GameObject>();
        private float _nextSpawnTime;

        private void Awake()
        {
            if (_groundLayer.value == 0) { _groundLayer = LayerMask.GetMask(_groundTag); }
        }

        private void Start()
        {
            PrewarmSpawns();
            _nextSpawnTime = Time.time + _spawnInterval;
        }

        private void Update()
        {
            CleanDestroyedSpawns();

            if (Time.time < _nextSpawnTime) { return; }

            _nextSpawnTime = Time.time + _spawnInterval;

            if (_activeSpawns.Count >= _maxActiveSpawns) { return; }

            TrySpawn();
        }

        private bool TrySpawn()
        {
            for (int i = 0; i < _spawnPointAttempts; i++)
            {
                if (TryGetSpawnPosition(out Vector3 spawnPosition))
                {
                    SpawnAt(spawnPosition);
                    return true;
                }
            }

            return false;
        }

        private void PrewarmSpawns()
        {
            for (int i = 0; i < _prewarmSpawnCount; i++)
            {
                if (_activeSpawns.Count >= _maxActiveSpawns) { return; }
                if (!TrySpawn()) { return; }
            }
        }

        private void SpawnAt(Vector3 spawnPosition)
        {
            GameObject spawnedObject = Instantiate(_spawnPrefab, spawnPosition, Quaternion.identity, _spawnRoot);
            _activeSpawns.Add(spawnedObject);
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
