using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackPositionDisplay : MonoBehaviour
{
    public InputField secDisplay;
    public InputField beatDisplay;

    private void Update()
    {
        if (!TrackPositionDisplayToggle.canSeek)
        {
            secDisplay.text = (LevelEditorManager.Instance.trackPos).ToString("00.00");
            beatDisplay.text = (LevelEditorManager.Instance.trackPosInBeats).ToString("00.00");
        }
    }
}
