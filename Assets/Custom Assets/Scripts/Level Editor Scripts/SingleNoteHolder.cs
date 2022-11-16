using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleNoteHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public SingleNoteData heldNote;

    private void Update()
    {
        indexOfThis = NoteListDisplay.Instance.singleNotes[laneID].single.IndexOf(heldNote);
    }
}
