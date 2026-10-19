using UnityEngine;

using UnityEngine.EventSystems;

using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class ButtonsEvents : ButtonsBase
{
    float originalScale = 1.0f;
    float maxTransparency = 255f;
    float initialTransparency = 0.0f;
    [SerializeField] float tolScale;
    [SerializeField] float durationToSize;
    [SerializeField] float durationToFade;
    [SerializeField] TMP_Text btnText;
    [SerializeField] TMP_Text titleText;
   
    [SerializeField] Image imgBtn;
    


    #region Buttons Events


    private void Start()
    {
        if (btnText != null && titleText != null)
        {
            titleText.DOFade(maxTransparency, 10f).SetEase(Ease.InOutSine).SetAutoKill(true);
            btnText.DOFade(maxTransparency, 20f).SetEase(Ease.InOutSine).SetAutoKill(true);
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        transform.DOScale(tolScale, durationToSize)
            .SetEase(Ease.InOutSine);
        if(imgBtn != null) 
        { imgBtn.DOFade(maxTransparency, durationToFade).SetEase(Ease.InOutSine).SetAutoKill(true); }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOScale(originalScale, durationToSize)
          .SetEase(Ease.InOutSine).SetAutoKill(true);

        if (imgBtn != null)
        { imgBtn.DOFade(initialTransparency, durationToFade).SetEase(Ease.InOutSine).SetAutoKill(true); }
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        transform.DOScale(tolScale, durationToSize)
        .SetEase(Ease.InOutSine).SetAutoKill(true);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        



        transform.DOScale(originalScale, durationToSize)
          .SetEase(Ease.InOutSine).SetAutoKill(true);
        if (imgBtn != null)
        { imgBtn.DOFade(initialTransparency, durationToFade).SetEase(Ease.InOutSine).SetAutoKill(true); }
    }
}


    #endregion
