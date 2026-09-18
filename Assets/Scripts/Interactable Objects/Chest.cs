
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
   
    //[SerializeField] bool playerInRange = false;


 

    //private void OnTriggerEnter(Collider other)
    //{
        
    //    if(other.CompareTag("Player") )
    //    {
    //        playerInRange = true;
        
    //        Debug.Log("esta en rango");
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;

    //        Debug.Log("no esta en rango");
    //    }
    //}

    public void Interact(GameObject interactor)
    {
        //if (!playerInRange)
        //    return;

        Debug.Log("Cofre interactuado");
        if (!GameManager.Instance.HasAbility("DoubleJump"))
        {
            GameManager.Instance.UnlockAbility(
             new DoubleJumpAbility());
        }/*else { GameManager.Instance.AddGem();Destroy(gameObject); }*/
    }
}
