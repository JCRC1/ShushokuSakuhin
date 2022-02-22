using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer m_mixer;
    public Slider m_slider;
    public string m_parameterName;

    private void Start()
    {
        if (m_parameterName.Contains("BGM"))
        {
            m_slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        } else if (m_parameterName.Contains("SFX"))
        {
            m_slider.value = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        }
    }

    public void SetLevel(float sliderValue)
    {
        m_mixer.SetFloat(m_parameterName, Mathf.Log10(sliderValue) * 20);
        if (m_parameterName.Contains("BGM"))
        {
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
        else if (m_parameterName.Contains("SFX"))
        {
            PlayerPrefs.SetFloat("SoundVolume", sliderValue);
        }
    }
}