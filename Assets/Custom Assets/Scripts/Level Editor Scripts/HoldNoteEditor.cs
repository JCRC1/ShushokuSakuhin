using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldNoteEditor : MonoBehaviour
{
    public HoldNoteData selectedNote;

    public InputField noteLane;
    public InputField noteBeat;
    public InputField noteDuration;

    public SelectedNoteEditor holder;

    public void SetNote(SelectedNoteEditor _holder)
    {
        if (_holder.selectedHoldNote != null)
        {
            holder = _holder;

            selectedNote = _holder.selectedHoldNote.heldNote;

            noteLane.text = _holder.selectedHoldNote.laneID.ToString();
            noteBeat.text = _holder.selectedHoldNote.heldNote.beat.ToString();
            noteDuration.text = _holder.selectedHoldNote.heldNote.duration.ToString();
        }
    }

    public void ConfirmEdit()
    {
        selectedNote.beat = float.Parse(noteBeat.text);
        selectedNote.duration = float.Parse(noteDuration.text);

        NoteListDisplay.Instance.EditNote(selectedNote, holder.selectedHoldNote);
    }
}
