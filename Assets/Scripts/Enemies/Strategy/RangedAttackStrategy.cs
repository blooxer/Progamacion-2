using UnityEngine;

public class RangedAttackStrategy : IAttackStrategy
{
    public void Attack(Enemy enemy)
    {
        BasicEnemieRanged enemyMeele = enemy as BasicEnemieRanged;

        if (enemyMeele == null)
            return;

        enemyMeele.ExecuteRangeAttack();
       
        Debug.Log(enemy.name + " realiza un ataque a distancia");
    }
}
