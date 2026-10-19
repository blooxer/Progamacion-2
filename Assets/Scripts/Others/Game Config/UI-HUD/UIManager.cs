
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text textGems;
    [SerializeField] Image [] imageLife;

    [SerializeField] PlayerController player;

    void Start()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGemsChange += UpdateGemText;
            GameManager.Instance.onLifeChange += UpdateLife;
            if (player != null)
            {
                UpdateGemText(GameManager.Instance.gems);
            }

            if (imageLife != null && player != null)
            {
                UpdateLife(player.CurrentHealth);
            }
        }
    }

    private void UpdateGemText(int gems)
    {
       textGems.text = gems + "/10";
    }
    private void UpdateLife(int life)
    {
       life = Mathf.Clamp(life,0,imageLife.Length);

        for(int i = 0; i < imageLife.Length; i++)
        {
            imageLife[i].gameObject.SetActive(i < life);
        }
    }
    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.onGemsChange -= UpdateGemText;
            GameManager.Instance.onLifeChange -= UpdateLife;
        }
    }
    
}
