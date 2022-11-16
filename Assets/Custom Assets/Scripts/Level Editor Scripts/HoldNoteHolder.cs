using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteHolder : MonoBehaviour
{
    // Which lane it belongs to
    public int laneID;
    public int indexOfThis;
    public HoldNoteData heldNote;

    private void Update()
    {
        indexOfThis = NoteListDisplay.Instance.holdNotes[laneID].hold.IndexOf(heldNote);
    }
}
