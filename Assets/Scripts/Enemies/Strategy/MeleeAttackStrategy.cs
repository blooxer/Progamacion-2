using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
    public void Attack(Enemy enemy)
    {
        BasicEnemiyMeele enemyMeele = enemy as BasicEnemiyMeele;

        if(enemyMeele == null)
        return;

        enemyMeele.ExecuteMeleeAttack();
        Debug.Log(enemy.name + " realiza un ataque melee");
    }
}
