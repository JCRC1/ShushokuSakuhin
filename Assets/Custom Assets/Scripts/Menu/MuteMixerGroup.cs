using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MuteMixerGroup : MonoBehaviour
{
    public AudioMixerSnapshot m_hitSoundsOn;
    public AudioMixerSnapshot m_hitSoundsOff;

    private void Start()
    {
        if (PlayerPrefs.GetInt("MuteHitSound") == 0)
        {
            GetComponent<Toggle>().isOn = false;
            MuteUnmute(false);
        }
        else
        {
            GetComponent<Toggle>().isOn = true;
            MuteUnmute(true);
        }
    }

    public void MuteUnmute(bool _mute)
    {
        if (_mute)
        {
            PlayerPrefs.SetInt("MuteHitSound", 1);
            m_hitSoundsOff.TransitionTo(0);
        }
        else
        {
            PlayerPrefs.SetInt("MuteHitSound", 0);
            m_hitSoundsOn.TransitionTo(0);
        }
    }
}
