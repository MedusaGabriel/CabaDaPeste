using UnityEngine;
using UnityEngine.Audio;

public class AmbientAudioManager : MonoBehaviour
{
    public AudioClip ambientClip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public AudioMixerGroup outputMixerGroup; // Novo campo para escolher o grupo de saída

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ambientClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.playOnAwake = true;

        if (outputMixerGroup != null)
            audioSource.outputAudioMixerGroup = outputMixerGroup;
    }

    void Start()
    {
        if (ambientClip != null)
            audioSource.Play();
    }

    public void ChangeAmbient(AudioClip newClip, float? newVolume = null)
    {
        audioSource.Stop();
        audioSource.clip = newClip;

        if (newVolume.HasValue)
            audioSource.volume = newVolume.Value;

        audioSource.Play();
    }
}
