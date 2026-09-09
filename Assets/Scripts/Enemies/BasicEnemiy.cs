using System;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemiy : Character
{
    public event Action<BasicEnemiy> OnEnemyDeath;
  



    // refs for animator
    private static readonly int attackHash = Animator.StringToHash("Attack");
    private static readonly int idlekHash = Animator.StringToHash("Idle");
    private static readonly int chaseHash = Animator.StringToHash("Chase");
    private static readonly int waitingHash = Animator.StringToHash("Waiting");


    // refs
    NavMeshAgent agent;
    Transform player;
    Animator animator;
    EnemyGroup enemyGroup;

    protected override void Die()
    {
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    [Header("Detection")]
    [SerializeField] float attackRange = 1.35f;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float distance;

    [SerializeField] bool attackFinished;

    // State Configs
    enum EnemyState
    {
        Idle, Chase, Attack, Waiting
    }

    [Header("State")]
    [SerializeField] EnemyState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyGroup = GetComponentInParent<EnemyGroup>();
        animator = GetComponent<Animator>();
        
    }
     void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
    
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        currentHealth = maxHealth;


        ChangeState(EnemyState.Idle);
    }

    public void AttackAnimationFinished() // function for animation event
    {
        attackFinished = true;
        if (enemyGroup != null)
        {
            enemyGroup.FinishAttack(this);
        }
    }

    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Waiting:
                WaitingState();
                break;

        }
        Debug.Log(currentState);

        animator.SetBool(idlekHash, currentState == EnemyState.Idle);
        animator.SetBool(chaseHash, currentState == EnemyState.Chase);
        animator.SetBool(attackHash, currentState == EnemyState.Attack);
        animator.SetBool(waitingHash, currentState == EnemyState.Waiting);
    }

    #region State configs
    void IdleState()
    {
        agent.isStopped = true;

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    void WaitingState()
    {
        agent.isStopped = true;

        distance = Vector3.Distance(transform.position, player.position);



        if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
            return;
        }


        if (enemyGroup == null || enemyGroup.CanAttack(this))
        {
            ChangeState(EnemyState.Attack);
        }

    }

    void ChaseState()
    {
        agent.isStopped = false;


        agent.SetDestination(player.position);

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {

            if (enemyGroup == null || enemyGroup.CanAttack(this)) // if this enemy can attack change this state to attack otherwise, it will wait its turn
            {
                ChangeState(EnemyState.Attack);
            }
            else
            {
                ChangeState(EnemyState.Waiting);
            }
        }
        else if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    void AttackState()
    {
        agent.isStopped = true;

        if (enemyGroup != null && !enemyGroup.CanAttack(this)) // if this enemy can not attack change its state for waiting state
        {
            ChangeState(EnemyState.Waiting);
        }
        ;

        enemyGroup.StartAttack(this); // start the attack and change the bool "CanAttack"

        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        distance = Vector3.Distance(transform.position, player.position);

        if (!attackFinished) return;

        if (distance > attackRange && distance <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
        }
        else if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
        }
        else { attackFinished = false; }
    }

    void ChangeState(EnemyState newState)
    {
        currentState = newState;
        if (newState == EnemyState.Attack)
        {
            attackFinished = false;
        }
    }

    #endregion



}
