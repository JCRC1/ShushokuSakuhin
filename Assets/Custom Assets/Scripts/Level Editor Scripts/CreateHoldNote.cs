using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateHoldNote : MonoBehaviour
{
    public HoldNoteData newNote;

    public InputField m_durationInput;

    public void NewNote()
    {
        newNote = new HoldNoteData();
        float.TryParse(Seekbar.Instance.m_currentBeatText[2].text, out newNote.m_beat);
        float.TryParse(m_durationInput.text, out newNote.m_duration);
        int id = LaneEditor.Instance.m_selectedLane.m_identifier;

        LevelEditorManager.Instance.m_chartData.m_lane[id].m_holdNote.Add(newNote);
    }
}
