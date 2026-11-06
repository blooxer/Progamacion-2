using UnityEngine;
using UnityEngine.Audio;

public class UIAudio : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource hoverSource;
    [SerializeField] AudioSource clipSource;
    [SerializeField] AudioClip[] clickClips;
    [SerializeField] AudioMixerSnapshot menuSnapshot;
    
    [SerializeField] float variationPitch = 0.1f;
    
    [SerializeField] float originalPitch = 1f;
    private void Start()
    {
        menuSnapshot.TransitionTo(0.01f);
    }
    public void PlayHover()
    {
        
        hoverSource.pitch = originalPitch + Random.Range(-variationPitch, variationPitch);
        hoverSource.Play();
    }
   public void PlayClick()
    {

        if (clickClips.Length == 0)
            return;

        int randomIndex = Random.Range(0, clickClips.Length);
        clipSource.pitch = originalPitch + Random.Range(-variationPitch, variationPitch);
        clipSource.PlayOneShot(clickClips[randomIndex]);
    }
}
