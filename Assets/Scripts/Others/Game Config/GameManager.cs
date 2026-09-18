using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public int gems { get; private set; }

    
    public bool isInteract { get; private set; }
    public Action<int> onGemsChange;
    public Action<int> onLifeChange;

    private Dictionary<string, MonsterAbility> unlockedAbilities =
        new Dictionary<string, MonsterAbility>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }




    public void AddGem()
    {
        gems++;

        onGemsChange?.Invoke(gems);
    }

    public bool HasEnoughGems()
    {
        return gems >= 10;
    }

   

    public void UpdateLife(int life)
    {
        onLifeChange?.Invoke(life);
    }


    public void UnlockAbility(MonsterAbility ability)
    {
        if(!unlockedAbilities.ContainsKey(ability.AbilityName))
        {
            unlockedAbilities.Add(ability.AbilityName, ability);
            Debug.Log("Habilidad desbloqueada: " +  ability.AbilityName);
        }
    }

    public bool HasAbility(string abilityName)
    {
        return unlockedAbilities.ContainsKey(abilityName);
    }

    public MonsterAbility GetAbility(string abilityName)
    { 
        if(unlockedAbilities.TryGetValue(abilityName, out MonsterAbility abilitiy))
        {
            return abilitiy;
        }
        return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" ||
            scene.name == "LostScene" ||
            scene.name == "WinScene")
        {
            ResetAll();
        }
    }

    public void ChangeScene(String nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void ResetAll()
    {
        gems = 0;

        unlockedAbilities.Clear();

        onGemsChange?.Invoke(gems);

        Debug.Log("Progreso reiniciado.");

    }
}
