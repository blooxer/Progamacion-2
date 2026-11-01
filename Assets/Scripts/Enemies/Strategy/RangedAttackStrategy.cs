using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{
    public void Attack(Enemy enemy)
    {
        BasicEnemieRanged enemyRanged = enemy as BasicEnemieRanged;

        if (enemyRanged == null)
            return;

        enemyRanged.ExecuteRangeAttack();
       
        Debug.Log(enemy.name + " realiza un ataque a distancia");
    }
}
