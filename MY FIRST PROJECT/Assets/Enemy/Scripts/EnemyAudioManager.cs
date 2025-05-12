using UnityEngine;
using System.Collections;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Clipes de Áudio")]
    public AudioClip runClip;
    public AudioClip attackClip;
    public AudioClip dashClip;
    public AudioClip deathClip;

    [Header("Volumes Individuais")]
    [Range(0f, 1f)] public float runVolume = 1f;
    [Range(0f, 1f)] public float attackVolume = 1f;
    [Range(0f, 1f)] public float dashVolume = 1f;
    [Range(0f, 1f)] public float deathVolume = 1f;

    [Header("Pitch Individual")]
    [Range(0.5f, 2f)] public float runPitch = 1f;
    [Range(0.5f, 2f)] public float attackPitch = 1f;
    [Range(0.5f, 2f)] public float dashPitch = 1f;
    [Range(0.5f, 2f)] public float deathPitch = 1f;

    [Header("Volume Geral")]
    [Range(0f, 1f)] public float masterVolume = 1f;

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

    public void PlayAttack()
    {
        audioSource.pitch = attackPitch;
        audioSource.PlayOneShot(attackClip, attackVolume * masterVolume);
        StartCoroutine(NotifyAttackSoundEnd(attackClip.length));
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

    private IEnumerator NotifyAttackSoundEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (EnemyAudioGlobalManager.Instance != null)
            EnemyAudioGlobalManager.Instance.OnAttackSoundEnd();
    }
}