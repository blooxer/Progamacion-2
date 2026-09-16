using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public event Action<Enemy> onEnemyDeath;

    protected NavMeshAgent agent;
    protected Transform player;
    protected IAttackStrategy attackStrategy;

    [Header("Detection")]
    [SerializeField] protected float attackRange = 1.35f;
    [SerializeField] protected float detectionRange = 5f;
    protected float distance;

    // State
    protected enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Waiting
    }

    [Header("State")]
    [SerializeField] protected EnemyState currentState;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        ChangeState(EnemyState.Idle);
    }

    protected virtual void Update()
    {
        StateMachine();
    }

    protected void StateMachine()
    {
        if (player == null)
            return;

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
    }

    protected virtual void IdleState()
    {
        agent.isStopped = true;

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    protected virtual void ChaseState()
    {
        if (player == null)
            return;
        agent.isStopped = false;

        agent.SetDestination(player.position);

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    protected virtual void AttackState()
    {
        agent.isStopped = true;
        ExecuteAttack();
    }

    protected virtual void WaitingState()
    {
        agent.isStopped = true;
    }

    protected void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }
    protected override void Die()
    {
        onEnemyDeath?.Invoke(this);
        GameManager.Instance.AddGem();
        Destroy(gameObject);
    }

    protected void ExecuteAttack()
    {
        if(attackStrategy != null)
        {
            attackStrategy.Attack(this);
        }
    }
}
