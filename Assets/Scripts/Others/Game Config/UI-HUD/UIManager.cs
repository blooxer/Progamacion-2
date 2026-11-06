
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text textGems;
    [SerializeField] Image [] imageLife;
    [SerializeField] GameObject panelInteractable;


    void Start()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGemsChange += UpdateGemText;
            GameManager.Instance.onLifeChange += UpdateLife;
            GameManager.Instance.OnInteractable += ObjInteractableDetected;
            
            
            UpdateGemText(GameManager.Instance.gems);
            UpdateLife(GameManager.Instance.actualLife);
            ObjInteractableDetected(GameManager.Instance.isInteractable);
          
          
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
            GameManager.Instance.OnInteractable -= ObjInteractableDetected;
        }
    }
    private void ObjInteractableDetected(bool isActiveGo)
    {
        if (panelInteractable != null)
        {
            panelInteractable.SetActive(isActiveGo);
        }
    }
}
