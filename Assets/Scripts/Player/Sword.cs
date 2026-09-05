using System.Collections.Generic;

using UnityEngine;

public class Sword : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        
        IDamageable damageable = other.GetComponent<IDamageable>();
        
        if (damageable != null )
        {
            damageable.TakeDamage(1);
        }

    }


}
