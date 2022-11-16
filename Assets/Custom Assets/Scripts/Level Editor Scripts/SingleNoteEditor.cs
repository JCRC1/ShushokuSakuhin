using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleNoteEditor : MonoBehaviour
{
    public SingleNoteData selectedNote;

    public InputField noteLane;
    public InputField noteBeat;

    public SelectedNoteEditor holder;

    public void SetNote(SelectedNoteEditor _holder)
    {
        if (_holder.selectedSingleNote != null)
        {
            holder = _holder;

            selectedNote = _holder.selectedSingleNote.heldNote;

            noteLane.text = _holder.selectedSingleNote.laneID.ToString();
            noteBeat.text = _holder.selectedSingleNote.heldNote.beat.ToString();
        }
    }

    public void ConfirmEdit()
    {
        selectedNote.beat = float.Parse(noteBeat.text);

        NoteListDisplay.Instance.EditNote(selectedNote, holder.selectedSingleNote);
    }
}
