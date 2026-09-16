using UnityEngine;

public class EnemyHitbox : DamageHitbox
{
    protected override void DealDamage(IDamageable target)
    {
        target.TakeDamage(damage);
    }
}
