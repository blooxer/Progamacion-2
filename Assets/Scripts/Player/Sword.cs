using UnityEngine;

public class Sword : MonoBehaviour
{
   [SerializeField] int damage = 1;
    [SerializeField] LayerMask enemyLayer;
    private void OnTriggerEnter(Collider other)
    {
        
        IDamageable damageable = other.GetComponent<IDamageable>();

        if ((enemyLayer.value & (1 << other.gameObject.layer)) == 0)
            return;
        if (damageable != null )
        {
            damageable.TakeDamage(damage);
        }

    }


}
