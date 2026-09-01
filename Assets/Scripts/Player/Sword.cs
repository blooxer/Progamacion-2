using UnityEngine;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{

    List<IDamageable> alreadyHit = new List<IDamageable>();
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null && !alreadyHit.Contains(damageable ))
        {
          
            damageable.TakeDamage(1);
            alreadyHit.Add(damageable );
        }    
    }

   
}
