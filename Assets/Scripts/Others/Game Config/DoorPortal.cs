using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.ChangeScene(sceneName);
        }
    }
}
