using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackBar : MonoBehaviour
{
    private AudioSource audioSource;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        audioSource = GameManager.Instance.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.initialized)
        {
            slider.value = audioSource.time / audioSource.clip.length;
        }
    }
}
