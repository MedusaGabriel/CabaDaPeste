using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
        {
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);

            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.50f);
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.50f);

            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;

            UpdateMusicVolume(musicVolume);
            UpdateSFXVolume(sfxVolume);
        }


    public void UpdateMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
