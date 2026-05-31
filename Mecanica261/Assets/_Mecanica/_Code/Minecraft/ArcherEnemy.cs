using UnityEngine;
using NaughtyAttributes;
using System.Collections;

namespace Minecraft
{
    public class ArcherEnemy : Enemy
    {
        [Header("Attack")]
        [SerializeField, Required] private Weapon _weapon;
        [SerializeField] private float _aimTime = 0.75f;
        [SerializeField] private float _postShotWaitTime = 0.25f;

        public override IEnumerator AttackMethod()
        {
            IMovementController movementController = GetComponent<IMovementController>();
            movementController?.StopMovement();

            yield return new WaitForSeconds(_aimTime);

            _weapon.Use(_targetTransform);

            yield return new WaitForSeconds(_postShotWaitTime);

            movementController?.ResumeMovement();
        }
    }
}
