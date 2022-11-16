using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateHoldNote : MonoBehaviour
{
    public HoldNoteData newNote;

    public InputField durationInput;

    public void NewNote()
    {
        newNote = new HoldNoteData();
        float.TryParse(Seekbar.Instance.currentBeatText[2].text, out newNote.beat);
        float.TryParse(durationInput.text, out newNote.duration);
        int id = LaneEditor.Instance.selectedLane.identifier;

        LevelEditorManager.Instance.chartData.lane[id].holdNote.Add(newNote);
        LevelEditorManager.Instance.chartData.lane[id].holdNote = LevelEditorManager.Instance.chartData.lane[id].holdNote.OrderBy(lst => lst.beat).ToList();

        NoteListDisplay.Instance.AddNoteToList(newNote, id);
    }
}
