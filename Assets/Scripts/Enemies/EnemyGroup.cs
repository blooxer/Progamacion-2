using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] List<Character> enemies = new List<Character>();

    Character attackingEnemie;


    private void Start()
    {
        enemies.AddRange(GetComponentsInChildren<Character>());

    }
    public bool CanAttack(Character enemy)
    {
        return attackingEnemie == null || attackingEnemie == enemy;
    }

    public void StartAttack(Character enemy)
    {
        if (attackingEnemie == null)
        {
            attackingEnemie = enemy;
        }
    }

    public void FinishAttack(Character enemy)
    {
        if (attackingEnemie == enemy)
        {
            attackingEnemie = null;
        }
    }
}
