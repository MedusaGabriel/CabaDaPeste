using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Referências de UI")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Fontes de Áudio")]
    public AudioSource musicSource; // Música de fundo
    public AudioSource[] sfxSources; // Todos os efeitos sonoros

    void Start()
    {
        // Carrega valores salvos (se existirem)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // Aplica os volumes ao iniciar
        UpdateMusicVolume(musicSlider.value);
        UpdateSFXVolume(sfxSlider.value);

        // Adiciona os listeners
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    public void UpdateMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void UpdateSFXVolume(float value)
    {
        foreach (AudioSource sfx in sfxSources)
        {
            sfx.volume = value;
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
