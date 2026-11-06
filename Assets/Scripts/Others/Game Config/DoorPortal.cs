using UnityEngine;

public class DoorPortal : MonoBehaviour, IInteractable
{
    public string sceneName;

    public void Interact(GameObject interactor)
    {
        if (interactor.CompareTag("Player") && GameManager.Instance.HasEnoughGems())
        {
            GameManager.Instance.ChangeScene(sceneName);
        }
        else { Debug.Log("Te faltan gemas!"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
