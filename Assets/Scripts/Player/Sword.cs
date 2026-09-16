using UnityEngine;

public class Sword : DamageHitbox
{
    protected override void DealDamage(IDamageable target)
    {
        target.TakeDamage(damage);
        Debug.Log("se hizo " + damage + " a " + target);
    }


}
