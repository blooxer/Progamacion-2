using UnityEngine;

public class ProjectileHitbox : DamageHitbox
{
    [Header("Projectile")]
    [SerializeField] private float speed = 10f;

    protected override void DealDamage(IDamageable target)
    {
        target.TakeDamage(damage);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Destroy(gameObject);
        
    }
    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
