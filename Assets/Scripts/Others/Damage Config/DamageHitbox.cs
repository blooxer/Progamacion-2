using UnityEngine;

public abstract class DamageHitbox : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected LayerMask targetLayer;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            DealDamage(damageable);
        }else { Debug.Log("no se detecta IDamageable"); }
    }

    protected abstract void DealDamage(IDamageable target);
}
