using DG.Tweening;
using UnityEngine;

public class Gems : MonoBehaviour
{
    [SerializeField] AudioSource gemsSource;
    [SerializeField] AudioClip gemsClip;
    [SerializeField] float variationPitch = 0.1f;

    [SerializeField] float originalPitch = 1f;
    float timerInit;
    Vector3 initialPos;
    bool canDestroy = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos = transform.position;
        timerInit = 0;
        transform.DORotate(new Vector3(0, -360, 0), 5f, RotateMode.FastBeyond360).SetLoops(-1,LoopType.Restart).SetEase(Ease.Linear);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           
            Vector3 newPos = new Vector3(initialPos.x, initialPos.y * 2, initialPos.z);
            GameManager.Instance.AddGem();
            gemsSource.pitch = originalPitch + Random.Range(-variationPitch, variationPitch);
            gemsSource.PlayOneShot(gemsClip);
            transform.DOMove(initialPos = newPos, 0.8f,false).SetEase(Ease.OutBack);
            canDestroy = true;
            other.GetComponent<PlayerController>().Heal(1);
          


        }
    }

    private void Update()
    {
        DestroyGem();
    }


    void DestroyGem()
    {
        if(canDestroy)
        {
            timerInit += Time.deltaTime;

            if (timerInit > 1f)
            {
                Destroy(gameObject);
            }
        }

    }

}
