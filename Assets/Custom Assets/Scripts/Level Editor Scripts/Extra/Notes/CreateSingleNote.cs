using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateSingleNote : MonoBehaviour
{
    public SingleNoteData newNote;

    public void NewNote()
    {
        newNote = new SingleNoteData();
        float.TryParse(Seekbar.Instance.m_currentBeatText[1].text, out newNote.m_beat);
        int id = LaneEditor.Instance.m_selectedLane.m_identifier;

        LevelEditorManager.Instance.m_chartData.m_lane[id].m_singleNote.Add(newNote);
        LevelEditorManager.Instance.m_chartData.m_lane[id].m_singleNote = LevelEditorManager.Instance.m_chartData.m_lane[id].m_singleNote.OrderBy(lst => lst.m_beat).ToList();

        NoteListDisplay.Instance.AddNoteToList(newNote, id);
    }
}
