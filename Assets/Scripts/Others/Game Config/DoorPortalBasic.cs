
using UnityEngine;

public class DoorPortalBasic : MonoBehaviour, IInteractable
{
    public string sceneName;
    [SerializeField] AudioSource portalSource;

    public void Interact(GameObject interactor)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player") )
        //{
        //    GameManager.Instance.ChangeScene(sceneName);
        //}
      
    }
    private void Update()
    {
        ActivePortalSounds();
    }
    void ActivePortalSounds()
    {
        if (this.gameObject.activeInHierarchy)
        {
            portalSource.Play();
        }
        else return;
    }

}
