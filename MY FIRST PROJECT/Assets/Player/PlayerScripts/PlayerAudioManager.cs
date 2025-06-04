using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource loopAudioSource;      // Para sons de andar/correr
    public AudioSource sfxAudioSource;       // Para ataque e morte

    [Header("Mixer de Áudio")]
    public AudioMixerGroup outputMixerGroup;

    [Header("Clipes de Áudio")]
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip attackClip;
    public AudioClip deathClip;

    [Header("Pitch Individual")]
    [Range(0.5f, 2f)] public float walkPitch = 1f;
    [Range(0.5f, 2f)] public float runPitch = 1f;
    [Range(0.5f, 2f)] public float attackPitch = 1f;
    [Range(0.5f, 2f)] public float deathPitch = 1f;

    [Header("Volume Geral")]
    [Range(0f, 1f)] public float masterVolume = 1f;

    void Awake()
    {
        if (loopAudioSource == null)
        {
            loopAudioSource = gameObject.AddComponent<AudioSource>();
            loopAudioSource.loop = true;
        }

        if (sfxAudioSource == null)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource.loop = false;
        }

        if (outputMixerGroup != null)
        {
            loopAudioSource.outputAudioMixerGroup = outputMixerGroup;
            sfxAudioSource.outputAudioMixerGroup = outputMixerGroup;
        }
    }

    public void PlayWalkLoop()
    {
        PlayLoop(walkClip, walkPitch);
    }

    public void StopWalkLoop()
    {
        StopLoop(walkClip);
    }

    public void PlayRunLoop()
    {
        PlayLoop(runClip, runPitch);
    }

    public void StopRunLoop()
    {
        StopLoop(runClip);
    }

    public void PlayAttack()
    {
        PlaySFX(attackClip, attackPitch);
    }

    public void PlayDeath()
    {
        PlaySFX(deathClip, deathPitch);
    }

    private void PlayLoop(AudioClip clip, float pitch)
    {
        if (clip == null) return;

        if (loopAudioSource.clip != clip || !loopAudioSource.isPlaying)
        {
            loopAudioSource.clip = clip;
            loopAudioSource.volume = masterVolume;
            loopAudioSource.pitch = pitch;
            loopAudioSource.Play();
        }
    }

    private void StopLoop(AudioClip clip)
    {
        if (loopAudioSource.clip == clip && loopAudioSource.isPlaying)
        {
            loopAudioSource.Stop();
            loopAudioSource.clip = null;
        }
    }

    private void PlaySFX(AudioClip clip, float pitch)
    {
        if (clip == null) return;

        sfxAudioSource.pitch = pitch;
        sfxAudioSource.PlayOneShot(clip, masterVolume);
    }
}