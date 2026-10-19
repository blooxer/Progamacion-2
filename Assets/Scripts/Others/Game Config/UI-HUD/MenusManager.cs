using UnityEngine;


public class MenusManager : MonoBehaviour
{
    public GameObject[] Tabs;
    

    public void SwitchToTab(int TabInt)
    {
        foreach (GameObject go in Tabs)
        {
            go.SetActive(false);
        }
        Tabs[TabInt].SetActive(true);
    }

    #region Buttons Funcs
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


    #endregion

}
