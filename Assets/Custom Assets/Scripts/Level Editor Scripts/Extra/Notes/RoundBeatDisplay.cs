using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundBeatDisplay : MonoBehaviour
{
    public Toggle[] m_toggles;

    public void ToggleOne(Toggle _toggle)
    {
        _toggle.interactable = !_toggle.interactable;
    }

    public void Update()
    {
        for (int i = 0; i < m_toggles.Length; i++)
        {
            if (m_toggles[i].isOn)
            {
                Seekbar.Instance.m_currentBeatText[1].text = (Mathf.Round(LevelEditorManager.Instance.m_trackPosInBeats * Mathf.Pow(2.0f, i)) / Mathf.Pow(2.0f, i)).ToString();
            }
        }
    }
}
