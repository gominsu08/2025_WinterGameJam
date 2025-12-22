using System;
using UnityEngine;

namespace Work.Entities.Code
{
    public interface IDamageable
    {
        Transform Transform { get; }
        bool IsDead { get; }
        Action<IDamageable> OnDeadEvent { get; set; }
        public void TakeDamage(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0);
    }
}
