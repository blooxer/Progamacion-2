
using System.Collections.Generic;

using UnityEngine;



public class Boss : Enemy
{
    // refs for animator
    private static int shieldedHash = Animator.StringToHash("Shielded");
    private static int vulnerableHash = Animator.StringToHash("Vulnerable");
    private static int transitionHash = Animator.StringToHash("Transition");
    private static int defeatedsHash = Animator.StringToHash("Defeated");
    Animator animator;




    [Header("Boss References")]
    [SerializeField] GameObject chest;
    [SerializeField] List<GameObject> activeGOWhenDies = new List<GameObject>();
    [SerializeField] GameObject shield;
    [SerializeField] bool attackFinished;


    [SerializeField] List<Crystals> Crystals = new List<Crystals>();

    [Header("Boss Timers")]
    [SerializeField, Min(0.01f)]
    float vulnerableDuration = 5f;
    [SerializeField, Min(0.01f)]
    float crystalHealInterval = 0.25f;
    float vulnerableTimer, crystalHealTimer;

    [Header("Invoke Enemy Config")]
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] List<BasicEnemiyMeele> summonedEnemies = new List<BasicEnemiyMeele>();
    [SerializeField] BasicEnemiyMeele prefabEnemie;
    bool stateInitialized = false;

    enum BossState
    {
        Shielded,
        Vulnerable,
        Transition,
        Defeated
    }

    [Header("Boss State")]
    [SerializeField] BossState currentBossState;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        ChangeBossState(BossState.Shielded);
        foreach (GameObject GO in activeGOWhenDies)
        {
            if(GO != null)
            {
                GO.gameObject.SetActive(false);
            }
        }
        foreach (Crystals crystals in Crystals)
        {
            if (crystals != null)
                crystals.OnCrystalsDamaged += OnCrystalBroken;
        }
        SpawnEnemies();
    }

    protected override void Update()
    {
        BossStateMachine();
    }

    void BossStateMachine()
    {
        if (player == null)
            return;


        switch (currentBossState)
        {
            case BossState.Shielded:
                Shielded();
                break;

            case BossState.Vulnerable:
                Vulnerable();
                break;

            case BossState.Transition:
                Transition();
                break;

            case BossState.Defeated:
                Defeated();
                break;
        }
    }
    private void ChangeBossState(BossState newState)
    {
        if (currentBossState == newState)
            return;
        stateInitialized = true;
        currentBossState = newState;

        if(newState == BossState.Vulnerable)
        {
            vulnerableTimer = 0;
        }
        if(newState == BossState.Transition)
        {
            crystalHealTimer = 0;
        }
        if(newState == BossState.Shielded)
        {
            SpawnEnemies();
        }

        UpdateAnimator();
    }

    #region STATES
    void Shielded()
    {
        shield.SetActive(true);




        Debug.Log("escudado, no es posible dañarlo");

    }

    void Vulnerable()
    {
        shield.SetActive(false);
        vulnerableTimer += Time.deltaTime;
        if (vulnerableTimer >= vulnerableDuration)
            { ChangeBossState(BossState.Transition); }

        Debug.Log("es vulnerable, atacalo!");
    }

    void Transition()
    {
        crystalHealTimer += Time.deltaTime;
        if (crystalHealTimer < crystalHealInterval)
            return;
        crystalHealTimer = 0;

        foreach (Crystals crystal in Crystals)
        {
            if(crystal != null && crystal.isBroken)
            {
                crystal.Repair(1);
            }
        }

        if(AllCrystalsIsAtMaxHealth())
        {
            ChangeBossState(BossState.Shielded);
        }
        Debug.Log("esta cambiando de fase, no puedes dañarlo de momento");
    }


    void Defeated()
    {
        OnDestroy();
        Debug.Log("el jefe fue vencido!");
    }






    protected override void Die()
    {
        ChangeBossState(BossState.Defeated);
        if (!GameManager.Instance.HasAbility("DoubleJump"))
        { chest.gameObject.SetActive(true); }
        else { chest.gameObject.SetActive(false); }


    }

    #endregion

    #region CRYSTALS CONFIG
    private void OnCrystalBroken(Crystals crystals)
    {
        if (currentBossState != BossState.Shielded)
            return;
        if (AllCrystalsBroken())
        {
            ChangeBossState(BossState.Vulnerable);
        }
    }


    bool AllCrystalsBroken()
    {
        return Crystals.Count > 0 && Crystals.TrueForAll(Crystals => Crystals != null && Crystals.isBroken);
    }

    bool AllCrystalsIsAtMaxHealth()
    {
        return Crystals.Count > 0 && Crystals.TrueForAll(Crystals => Crystals != null && Crystals.CurrentHealth >= Crystals.MaxHealth);
    }
    private void OnDestroy()
    {
            foreach (GameObject GO in activeGOWhenDies)
            {
                if (GO != null)
                {
                    GO.gameObject.SetActive(true);
                }
            }
        foreach (Crystals crystals in Crystals)
        {
            if (crystals != null)
               { crystals.OnCrystalsDamaged -= OnCrystalBroken;
                crystals.gameObject.SetActive(false);
            }
           
        }
    }
    #endregion



    void SpawnEnemies()
    {
        if(prefabEnemie == null)
        {
            Debug.LogWarning("No se asigno un prefab enemigo");
            return;
        }

        // limpiar referencias de enemigos muertos
        summonedEnemies.RemoveAll(BasicEnemiyMeele => BasicEnemiyMeele == null);

        //evitar acumular oleadas
        if(summonedEnemies.Count > 0)
        {
            Debug.Log("quedan enemigos todavia");
            return;
        }

        foreach(Transform spawns in spawnPoints)
        {
            if (spawnPoints == null)
            {
                return;
            }
            BasicEnemiyMeele newEnemy = Instantiate(prefabEnemie,spawns.position,spawns.rotation);
            summonedEnemies.Add(newEnemy);
        }

        Debug.Log("el jefe invoco enemigos");
    }

    public override void TakeDamage(int dmg)
    {
        if (currentBossState != BossState.Vulnerable)
            return;
        base.TakeDamage(dmg);
    }

    void UpdateAnimator()
    {
        animator.SetBool(
            shieldedHash,
            currentBossState == BossState.Shielded
        );
        animator.SetBool(
            transitionHash,
            currentBossState == BossState.Transition
        );

        animator.SetBool(
            vulnerableHash,
            currentBossState == BossState.Vulnerable
        );
        animator.SetBool(
            defeatedsHash,
            currentBossState == BossState.Defeated
        );
    }


}
