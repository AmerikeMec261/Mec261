using UnityEngine;
using System.Collections.Generic;

namespace Minecraft
{
    public class Sword : Weapon
    {
        [SerializeField] private float _range = 1.75f;
        [SerializeField] private Vector3 _boxCenter = new Vector3(0f, 0f, 0.875f);
        [SerializeField] private Vector3 _boxSize = new Vector3(0.9f, 1.25f, 1.75f);

        [Header("Debug")]
        [SerializeField] private bool _drawAttackDebug = true;
        [SerializeField] private float _debugDuration = 0.25f;
        [SerializeField] private Color _debugColor = Color.red;

        private Vector3 _debugStart;
        private Vector3 _debugEnd;
        private Vector3 _debugCenter;
        private Vector3 _debugSize;
        private Quaternion _debugRotation;
        private bool _hasDebugAttack;
        private float _debugEndTime;
        private IDamagable _owner;

        private void Awake()
        {
            _owner = GetComponentInParent<IDamagable>();
        }

        protected override void UseWeapon(Vector3 direction)
        {
            GetComponentInParent<IWeaponAnimationReceiver>()?.PlaySwing();

            Vector3 attackDirection = direction.normalized;
            Quaternion rotation = Quaternion.LookRotation(attackDirection, Vector3.up);
            Vector3 attackStart = AttackPoint.position;
            Vector3 attackEnd = attackStart + attackDirection * _range;
            Vector3 boxCenter = attackStart + rotation * _boxCenter;
            Vector3 halfExtents = _boxSize * 0.5f;

            DrawAttackDebug(attackStart, attackEnd, boxCenter, _boxSize, rotation);

            Collider[] colliders = Physics.OverlapBox(boxCenter, halfExtents, rotation);
            HashSet<IDamagable> damagedTargets = new HashSet<IDamagable>();

            DamageColliders(colliders, damagedTargets);
        }

        private void DamageColliders(Collider[] colliders, HashSet<IDamagable> damagedTargets)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                IDamagable damagable = colliders[i].GetComponentInParent<IDamagable>();

                if (damagable == _owner) { continue; }

                if (damagable != null && damagedTargets.Add(damagable))
                {
                    DamageTarget(colliders[i]);
                }
            }
        }

        private void DrawAttackDebug(Vector3 attackStart, Vector3 attackEnd, Vector3 boxCenter, Vector3 boxSize, Quaternion rotation)
        {
            if (!_drawAttackDebug) { return; }

            _debugStart = attackStart;
            _debugEnd = attackEnd;
            _debugCenter = boxCenter;
            _debugSize = boxSize;
            _debugRotation = rotation;
            _debugEndTime = Time.time + _debugDuration;
            _hasDebugAttack = true;

            Debug.DrawLine(_debugStart, _debugEnd, _debugColor, _debugDuration);
        }

        private void OnDrawGizmos()
        {
            if (!_hasDebugAttack || Time.time > _debugEndTime) { return; }

            Gizmos.color = _debugColor;
            Gizmos.DrawLine(_debugStart, _debugEnd);
            Matrix4x4 previousMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(_debugCenter, _debugRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _debugSize);
            Gizmos.matrix = previousMatrix;
        }
    }
}
