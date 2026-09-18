
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text textGems;
    [SerializeField] TMP_Text textLife;

    [SerializeField] PlayerController player;

    void Start()
    {
      
        if (GameManager.Instance != null)
        {    GameManager.Instance.onGemsChange += UpdateGemText;
            GameManager.Instance.onLifeChange += UpdateLifeText;
            if(textGems != null && textLife != null)
           { UpdateGemText(GameManager.Instance.gems);
                UpdateLifeText(player.CurrentHealth);
            }
        }
    }

    private void UpdateGemText(int gems)
    {
       textGems.text = gems + "/10";
    }
    private void UpdateLifeText(int life)
    {
        textLife.text = life + "/3";
    }
    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.onGemsChange -= UpdateGemText;
            GameManager.Instance.onLifeChange -= UpdateLifeText;
        }
    }

    public void MainMenu()
    {
        GameManager.Instance.ChangeScene("MainMenu");
    }
    public void LevelOne()
    {
        GameManager.Instance.ChangeScene("Level");
    }
    public void BossLevel()
    {
        GameManager.Instance.ChangeScene("BossLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
  
}
