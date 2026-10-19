using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonsBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        
    }
}
