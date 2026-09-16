using UnityEngine;

public class BasicEnemieRanged : Enemy
{
    // Animator
    private static int attackHash = Animator.StringToHash("Attack");
    private static int idleHash = Animator.StringToHash("Idle");
    private static int chaseHash = Animator.StringToHash("Chase");

    // References
    private Animator animator;

    [Header("Ranged Attack")]
    //[SerializeField] private float minimumAttackDistance = 3f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private bool attackFinished;

    protected override void Awake()
    {
        base.Awake();
        attackStrategy = new RangedAttackStrategy();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        UpdateAnimator();
    }

    protected override void ChaseState()
    {
        if (player == null)
            return;

        distance = Vector3.Distance(
            transform.position,
            player.position
        );

        
        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if (distance > detectionRange)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    //protected override void AttackState()
    //{

    //}
    public void ExecuteRangeAttack()
    {

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

    public void AttackAnimationFinished()
    {
        attackFinished = true;
    }

    public void ShootProjectile()
    {
        if (projectile == null || projectileSpawnPoint == null)
            return;

        Instantiate(
            projectile,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation
        );
    }

    void UpdateAnimator()
    {
        animator.SetBool(
            idleHash,
            currentState == EnemyState.Idle
        );

        animator.SetBool(
            chaseHash,
            currentState == EnemyState.Chase
        );

        animator.SetBool(
            attackHash,
            currentState == EnemyState.Attack
        );
    }
}
