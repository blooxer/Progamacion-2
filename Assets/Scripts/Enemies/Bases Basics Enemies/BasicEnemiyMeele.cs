using UnityEngine;


public class BasicEnemiyMeele : Enemy
{
   
  



    // refs for animator
    private static  int attackHash = Animator.StringToHash("Attack");
    private static  int idleHash = Animator.StringToHash("Idle");
    private static  int chaseHash = Animator.StringToHash("Chase");
    private static  int waitingHash = Animator.StringToHash("Waiting");


    // refs
    Animator animator;
    EnemyGroup enemyGroup;

    [SerializeField] bool attackFinished;



    protected override void Awake()
    {
        base.Awake();
        attackStrategy = new MeleeAttackStrategy();
     
        animator = GetComponent<Animator>();
        
    }
     protected override void Start()
    {
      base.Start();
    }


    protected override void Update()
    {
       base.Update();
        UpdateAnimator();
    }
    public void AttackAnimationFinished() // function for animation event
    {
        attackFinished = true;
        if (enemyGroup != null)
        {
            enemyGroup.FinishAttack(this);
        }
    }

    protected override void IdleState()
    {
        base.IdleState();
    }

    protected override void ChaseState()
    {
       

        agent.isStopped = false;

        agent.SetDestination(player.position);

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange)
        {
            if (enemyGroup == null || enemyGroup.CanAttack(this))
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

    protected override void WaitingState()
    {
        if (player == null)
            return;

        agent.isStopped = true;

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

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

    //protected override void AttackState()
    //{
    //    agent.isStopped = true;
        

    //}
    public void ExecuteMeleeAttack()
    {
       
        if (enemyGroup != null && !enemyGroup.CanAttack(this))
        {
            ChangeState(EnemyState.Waiting);
            return;
        }

        if (enemyGroup != null)
        {
            enemyGroup.StartAttack(this);
        }

        // look to player
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

     
        if (!attackFinished)
            return;

        if (distance > attackRange && distance <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
        }
        else if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
        }
        else
        {
            attackFinished = false;
        }

    }

  
    void UpdateAnimator()
    {
        animator.SetBool(
            idleHash,
            currentState == EnemyState.Idle
        );
        animator.SetBool(
            attackHash,
            currentState == EnemyState.Attack
        );

        animator.SetBool(
            chaseHash,
            currentState == EnemyState.Chase
        );


        animator.SetBool(
            waitingHash,
            currentState == EnemyState.Waiting
        );
    }


}
