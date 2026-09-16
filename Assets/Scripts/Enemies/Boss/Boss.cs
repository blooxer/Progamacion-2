
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] GameObject chest;
    protected override void Die()
    {
        if (!GameManager.Instance.HasAbility("DoubleJump"))
       { chest.gameObject.SetActive(true); }
        else { chest.gameObject.SetActive(false);}
      
        Destroy(gameObject);
    }
}
