using UnityEngine;
using UnityEngine.Audio;

public class LowHealthAudio : MonoBehaviour
{
  [SerializeField] AudioMixer audioMixer;
  [SerializeField] int lowHealthThreshold = 1;
  [SerializeField] float normalCutoff = 22000f;
  [SerializeField] float lowHealthCutoff = 1200f;
  


    private void Start()
    {
        GameManager.Instance.onLifeChange += UpdateAudio;

        UpdateAudio(GameManager.Instance.actualLife);
    }

    void UpdateAudio(int life)
    {
        float cutoff = life <= lowHealthThreshold ? lowHealthCutoff : normalCutoff;

        audioMixer.SetFloat("MusicLowPass", cutoff);
    }


    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.onLifeChange -= UpdateAudio;
        }
    }
}
