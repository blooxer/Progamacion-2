using System;
using DG.Tweening;
using UnityEngine;

public class Crystals :  Character
{
    public event Action<Crystals> OnCrystalsDamaged;
    public bool isBroken {  get; private set; }
    public bool isRepairing {  get; private set; }
    [SerializeField] Collider hurtbox;
    [SerializeField] Material material;
    [SerializeField] Light lightPoint;


    Animator animator;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material.DOColor(Color.white * 5f, "_EmissionColor", 10f).SetEase(Ease.Linear);
        animator = GetComponent<Animator>();

        DesactivateCrystal();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimations();
    }

   

    public override void TakeDamage(int dmg)
    {
        if (isBroken || isRepairing)
            return;
        base.TakeDamage(dmg);
        
    }
  
    protected override void Die()
    {
        if (isBroken)
            return;
        
        isBroken = true;
        lightPoint.DOIntensity(0, 1f).SetEase(Ease.Linear);
        material.DOColor(Color.black * -5f, "_EmissionColor", 10f).SetEase(Ease.Linear);
        OnCrystalsDamaged?.Invoke(this);

    }

    public void Repair(int amount)
    {
        if (!isBroken)
            return;
        isRepairing = true;

        TakeHeal(amount);

        if(currentHealth >= maxHealth)
        {
            isBroken = false;
            isRepairing = false;
            lightPoint.DOIntensity(8, 1f).SetEase(Ease.Linear);
            material.DOColor(Color.white * 5f, "_EmissionColor", 10f).SetEase(Ease.Linear);
        }
    }
    void UpdateAnimations()
    {
        animator.SetBool("Broken",isBroken);
    }


    public void DesactivateCrystal()
    {
        if(isBroken)
        {hurtbox.enabled = false;

            material.DOColor(Color.black * -5f, "_EmissionColor", 1f).SetEase(Ease.Linear);
        }
    }

}
