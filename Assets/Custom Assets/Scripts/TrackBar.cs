using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackBar : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Slider m_slider;

    // Start is called before the first frame update
    void Start()
    {
        m_slider = GetComponent<Slider>();
        m_audioSource = GameManager.Instance.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.m_initialized)
        {
            m_slider.value = m_audioSource.time / m_audioSource.clip.length;
        }
    }
}
