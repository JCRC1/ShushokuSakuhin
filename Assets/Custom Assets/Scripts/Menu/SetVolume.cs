using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string parameterName;

    private void Start()
    {
        if (parameterName.Contains("BGM"))
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        } else if (parameterName.Contains("SFX"))
        {
            slider.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(parameterName, Mathf.Log10(sliderValue) * 20);
        if (parameterName.Contains("BGM"))
        {
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
        else if (parameterName.Contains("SFX"))
        {
            PlayerPrefs.SetFloat("SoundVolume", sliderValue);
        }
    }
}