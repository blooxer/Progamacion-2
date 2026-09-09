using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        if (damageable != null )
        {
            damageable.TakeDamage(damage);
        }
    }
}
