using System.Collections.Generic;
using UnityEngine;

public class CombatArea : MonoBehaviour
{
    [SerializeField] List<BasicEnemiy> enemies = new List<BasicEnemiy>();
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

        foreach (BasicEnemiy enemy in enemies)
        {
            enemy.OnEnemyDeath += EnemyDefeated;
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
    public void EnemyDefeated(BasicEnemiy enemy)
    {
        enemies.Remove(enemy);

        Debug.Log("Enemigos restantes: " + enemies.Count);

        if (enemies.Count == 0)
        {
            EndCombat();
        }
    }
}
