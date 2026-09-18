using Unity.VectorGraphics;
using UnityEngine;

public class DoorPortalBasic : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            GameManager.Instance.ChangeScene(sceneName);
        }
      
    }

}
