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
        float.TryParse(Seekbar.Instance.currentBeatText[1].text, out newNote.beat);
        int id = LaneEditor.Instance.selectedLane.identifier;

        LevelEditorManager.Instance.chartData.lane[id].singleNote.Add(newNote);
        LevelEditorManager.Instance.chartData.lane[id].singleNote = LevelEditorManager.Instance.chartData.lane[id].singleNote.OrderBy(lst => lst.beat).ToList();

        NoteListDisplay.Instance.AddNoteToList(newNote, id);
    }
}
