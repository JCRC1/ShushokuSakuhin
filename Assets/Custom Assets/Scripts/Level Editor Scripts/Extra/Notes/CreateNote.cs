using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNote : MonoBehaviour
{
    public NoteData newNote;

    public void NewNote()
    {
        newNote = new NoteData();
        float.TryParse(Seekbar.Instance.m_currentBeatText[1].text, out newNote.m_beat);
        int id = LaneEditor.Instance.m_selectedLane.m_identifier;

        LevelEditorManager.Instance.m_chartData.m_lane[id].m_notes.Add(newNote);
    }
}
