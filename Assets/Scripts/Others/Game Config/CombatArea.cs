using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatArea : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<GameObject> barriers = new List<GameObject>();

    bool combatStarted = false;

   
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && !combatStarted)
        {
            StartCombat();
       
        }
    }

    private void Start()
    {
        enemies.AddRange(GetComponentsInChildren<Enemy>());
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(false);
        }
       
    }
    private void Update()
    {

    }
    void StartCombat()
    {
        combatStarted = true;
        Debug.Log("Enemigos restantes: " + enemies.Count);

        foreach (Enemy enemy in enemies)
        {
            enemy.onEnemyDeath += EnemyDefeated;
        }

        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(true);
        }
    }

    void EndCombat()
    {
        foreach (GameObject barrier in barriers)
        {
            barrier.SetActive(false);
        }

        Debug.Log("¡Área completada!");
    }
    private void EnemyDefeated(Enemy enemy)
    {
        enemy.onEnemyDeath -= EnemyDefeated;
        enemies.Remove(enemy);

        Debug.Log("Enemigos restantes: " + enemies.Count);

        if (enemies.Count == 0)
        {
            EndCombat();
        }
    }
}
