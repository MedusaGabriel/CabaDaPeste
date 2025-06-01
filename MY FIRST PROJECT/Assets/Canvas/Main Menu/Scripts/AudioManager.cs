using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip, AudioMixerGroup mixerGroup = null)
    {
        GameObject tempGO = new GameObject("TempAudio");
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;

        if (mixerGroup != null)
            aSource.outputAudioMixerGroup = mixerGroup;

        aSource.Play();
        Destroy(tempGO, clip.length);
    }
}