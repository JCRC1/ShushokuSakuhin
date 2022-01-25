using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackPositionDisplay : MonoBehaviour
{
    public InputField m_secDisplay;
    public InputField m_beatDisplay;

    private void Update()
    {
        if (!TrackPositionDisplayToggle.m_canSeek)
        {
            m_secDisplay.text = (LevelEditorManager.Instance.m_trackPos).ToString("00.00");
            m_beatDisplay.text = (LevelEditorManager.Instance.m_trackPosInBeats).ToString("00.00");
        }
    }
}
