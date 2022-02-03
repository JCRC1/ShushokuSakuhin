using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seekbar : MonoBehaviour
{
    public static Seekbar Instance;

    private AudioSource m_audioSource;
    private Slider m_slider;

    public InputField[] m_currentBeatText;

    private float m_trackTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_audioSource = LevelEditorManager.Instance.GetComponent<AudioSource>();
    }

    public void ChangeAudioTime()
    {
        if (m_slider.value < 1.0f)
        {
            m_audioSource.time = (m_audioSource.clip.length * m_slider.value);
        }
    }

    public void SetTimeSeconds(Text _text)
    {
        LevelEditorManager.Instance.m_audioSource.time = float.Parse(_text.text);
    }

    public void SetTimeBeats(Text _text)
    {
        LevelEditorManager.Instance.m_audioSource.time = LevelEditorManager.Instance.m_secPerBeat * float.Parse(_text.text);
    }

    public void Update()
    {
        if (LevelEditorManager.Instance.m_initialized)
        {
            m_slider.value = m_audioSource.time / m_audioSource.clip.length;
        }

        // Keep track of the beats of the track
        if (!ManualButton.m_manualBeatInput)
        {
            if (!CreateLaneEvent.m_creatingMovement && !CreateLaneEvent.m_creatingRotation)
            {
                for (int i = 0; i < m_currentBeatText.Length; i++)
                {
                    m_currentBeatText[i].text = LevelEditorManager.Instance.m_trackPosInBeats.ToString("00.00");
                }
            }
        }

        if (LevelEditorManager.Instance.m_audioSource.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelEditorManager.Instance.m_audioSource.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelEditorManager.Instance.m_audioSource.UnPause();
            }
        }
    }
}
