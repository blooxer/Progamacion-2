using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
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

    public virtual void TakeHeal(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}

    
