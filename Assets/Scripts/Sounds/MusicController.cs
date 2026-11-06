using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [Header("Music Sources")]
    [SerializeField] AudioSource explorationSource;
    [SerializeField] AudioSource combatSource;
    [SerializeField] AudioSource voiceSource;
    [SerializeField] AudioClip voiceStartLevel;
    [SerializeField] AudioClip voiceBossLevel;

    [Header("Mixer Snashots")]
    [SerializeField] AudioMixerSnapshot explorationSnap;
    [SerializeField] AudioMixerSnapshot combatSnap;

    [Header("Transition")]
    [SerializeField] float transitionTime = 1f;
    void Start()
    {
       
        combatSource.Play();
        explorationSource.Play();

            if (GameManager.Instance.sceneName == "Level")
            {

                explorationSnap.TransitionTo(0.01f);
                voiceSource.PlayOneShot(voiceStartLevel);
            }
            else if (GameManager.Instance.sceneName == "BossLevel")
            {
                combatSnap.TransitionTo(0.01f);
                voiceSource.PlayOneShot(voiceBossLevel);
            }
        
    }


    public void ExplorationMusic()
    {
        explorationSnap.TransitionTo(transitionTime);
    }
    public void CombatMusic()
    {
        combatSnap.TransitionTo(transitionTime);

    }



}
