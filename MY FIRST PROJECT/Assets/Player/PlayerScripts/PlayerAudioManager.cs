using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Clipes de Áudio")]
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip attackClip;
    public AudioClip dashClip;
    public AudioClip deathClip;

    [Header("Volumes Individuais")]
    [Range(0f, 1f)] public float walkVolume = 1f;
    [Range(0f, 1f)] public float runVolume = 1f;
    [Range(0f, 1f)] public float attackVolume = 1f;
    [Range(0f, 1f)] public float dashVolume = 1f;
    [Range(0f, 1f)] public float deathVolume = 1f;

    [Header("Pitch Individual")]
    [Range(0.5f, 2f)] public float walkPitch = 1f;
    [Range(0.5f, 2f)] public float runPitch = 1f;
    [Range(0.5f, 2f)] public float attackPitch = 1f;
    [Range(0.5f, 2f)] public float dashPitch = 1f;
    [Range(0.5f, 2f)] public float deathPitch = 1f;

    [Header("Volume Geral")]
    [Range(0f, 1f)] public float masterVolume = 1f;

    public void PlayWalkLoop()
    {
        if (audioSource.clip != walkClip || !audioSource.isPlaying)
        {
            audioSource.clip = walkClip;
            audioSource.volume = walkVolume * masterVolume;
            audioSource.pitch = walkPitch;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopWalkLoop()
    {
        if (audioSource.clip == walkClip && audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PlayRunLoop()
    {
        if (audioSource.clip != runClip || !audioSource.isPlaying)
        {
            audioSource.clip = runClip;
            audioSource.volume = runVolume * masterVolume;
            audioSource.pitch = runPitch;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopRunLoop()
    {
        if (audioSource.clip == runClip && audioSource.isPlaying)
            audioSource.Stop();
    }

    // Para sons únicos (ataque, dash, morte) mantenha PlayOneShot:
    public void PlayAttack()
    {
        audioSource.pitch = attackPitch;
        audioSource.PlayOneShot(attackClip, attackVolume * masterVolume);
    }
    public void PlayDash()
    {
        audioSource.pitch = dashPitch;
        audioSource.PlayOneShot(dashClip, dashVolume * masterVolume);
    }
    public void PlayDeath()
    {
        audioSource.pitch = deathPitch;
        audioSource.PlayOneShot(deathClip, deathVolume * masterVolume);
    }
}