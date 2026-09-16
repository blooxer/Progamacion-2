using UnityEngine;

public abstract class MonsterAbility 
{
   public string AbilityName {  get; private set; }

    protected MonsterAbility(string abilityName)
    {
        AbilityName = abilityName;
    }

    public abstract void Activate(PlayerController player);
}
