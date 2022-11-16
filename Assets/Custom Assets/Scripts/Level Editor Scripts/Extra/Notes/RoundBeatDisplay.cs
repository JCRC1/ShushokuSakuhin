using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundBeatDisplay : MonoBehaviour
{
    public Toggle[] toggles;

    public void ToggleOne(Toggle _toggle)
    {
        _toggle.interactable = !_toggle.interactable;
    }

    public void Update()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                Seekbar.Instance.currentBeatText[1].text = (Mathf.Round(LevelEditorManager.Instance.trackPosInBeats * Mathf.Pow(2.0f, i)) / Mathf.Pow(2.0f, i)).ToString();
                Seekbar.Instance.currentBeatText[2].text = (Mathf.Round(LevelEditorManager.Instance.trackPosInBeats * Mathf.Pow(2.0f, i)) / Mathf.Pow(2.0f, i)).ToString();
            }
        }
    }
}
