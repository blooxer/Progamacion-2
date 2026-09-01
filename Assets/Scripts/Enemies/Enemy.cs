using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int damage = 1;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int dmg)
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

    
