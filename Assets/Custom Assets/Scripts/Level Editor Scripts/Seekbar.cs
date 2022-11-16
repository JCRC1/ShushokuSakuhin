using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seekbar : MonoBehaviour
{
    public static Seekbar Instance;

    private AudioSource audioSource;
    private Slider slider;

    public InputField[] currentBeatText;

    private float trackTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        slider = GetComponent<Slider>();
        audioSource = LevelEditorManager.Instance.GetComponent<AudioSource>();
    }

    public void ChangeAudioTime()
    {
        if (slider.value < 1.0f)
        {
            audioSource.time = (audioSource.clip.length * slider.value);
        }
    }

    public void SetTimeSeconds(Text _text)
    {
        LevelEditorManager.Instance.audioSource.time = float.Parse(_text.text);
    }

    public void SetTimeBeats(Text _text)
    {
        LevelEditorManager.Instance.audioSource.time = LevelEditorManager.Instance.secPerBeat * float.Parse(_text.text);
    }

    public void Update()
    {
        if (LevelEditorManager.Instance.initialized)
        {
            slider.value = audioSource.time / audioSource.clip.length;
        }

        // Keep track of the beats of the track
        if (!ManualButton.manualBeatInput)
        {
            if (!CreateLaneEvent.creatingMovement && !CreateLaneEvent.creatingRotation)
            {
                for (int i = 0; i < currentBeatText.Length; i++)
                {
                    currentBeatText[i].text = LevelEditorManager.Instance.trackPosInBeats.ToString("00.00");
                }
            }
        }

        if (LevelEditorManager.Instance.audioSource.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelEditorManager.Instance.audioSource.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelEditorManager.Instance.audioSource.UnPause();
            }
        }
    }
}
