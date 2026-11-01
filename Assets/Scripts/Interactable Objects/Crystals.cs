using System;
using UnityEngine;

public class Crystals :  Character
{
    public event Action<Crystals> OnCrystalsDamaged;
    public bool isBroken {  get; private set; }
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public override void TakeDamage(int dmg)
    {
        if (isBroken)
            return;
        base.TakeDamage(dmg);
        
    }
  
    protected override void Die()
    {
        if (isBroken)
            return;
        
        isBroken = true;

        OnCrystalsDamaged?.Invoke(this);

    }

    public void Repair(int amount)
    {
        if (!isBroken)
            return;
        TakeHeal(amount);

        if(currentHealth >= maxHealth)
        {
            isBroken = false;
        }
    }
}
