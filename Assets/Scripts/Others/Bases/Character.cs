using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int currentHealth;
    public int CurrentHealth => currentHealth;
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }


    public virtual void TakeDamage(int dmg)
    {
        currentHealth-=dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

        Destroy(gameObject);
    }

}

    
