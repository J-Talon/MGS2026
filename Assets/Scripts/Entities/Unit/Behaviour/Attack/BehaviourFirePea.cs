// Author: Jeremy
using Entities.Abstract;
using Entities.Projectile.ProjectileTypes;
using UnityEngine;

namespace Entities.Unit.Behaviour.Attack
{
    public class BehaviourFirePea : EntityBehaviour
    {
        public override BehaviourAction BehaviourAction { get; } = BehaviourAction.Attack;
        public override BehaviourType BehaviourType { get; } = BehaviourType.Offensive;
        public override BehaviourPriority BehaviourPriority { get; } = BehaviourPriority.Medium;

        [field: SerializeField] private Vector2 fireDirection = new Vector2(-1, 0);
        [field: SerializeField] private float attackCooldown = 3f;
        private float attackCooldownTimer;

        void Start()
        {
            attackCooldownTimer = attackCooldown;
        }

        public override bool Tick()
        {
            bool attempt = false;
            // checks attackCooldownTimer. If the countdown is greater than 0, continue counting down
            if (attackCooldownTimer > 0f) attackCooldownTimer -= Time.deltaTime;
            else
            {
                attempt = FirePea();
                attackCooldownTimer = attackCooldown;
            }
            return attempt;
        }

        private bool FirePea()
        {
            Vector3 unitPosition = unit.GetPosition();
            // spawning in between cells otherwise it may look awkward due to spawning on top of another unit. -1 to spawn above the grid
            Vector3 spawnPosition = new Vector3(unitPosition.x + fireDirection.x, unitPosition.y + fireDirection.y, -1);

            Entity spawned = EntityManager.GetInstance().GetEntity(EntityType.ProjectilePea, spawnPosition, this.unit.GetIsFriendly(), 2);
            if (spawned is PeaBullet pea)
            {
                pea.SetDirection(fireDirection);
                return true;
            }

            // No real fail condition for now
            return false;
        }
    }
}